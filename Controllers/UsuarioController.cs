using inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;

namespace inmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly RepositorioUsuario repo;
        private readonly IConfiguration configuration;

        public UsuarioController(RepositorioUsuario repo, IConfiguration configuration) // <--- Lo inyectas en el constructor
        {
            this.repo = repo;
            this.configuration = configuration;
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var usuarios = repo.obtenerTodos();
            return View(usuarios);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public IActionResult Create(Usuario u)
        {
            if (ModelState.IsValid)
            {
                // Hashear la contraseña
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: u.password_usuario,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                u.password_usuario = hashed;

                // falta la implementacion de avatar
                int res = repo.agregarUsuario(u);
                if (res != 0)
                {

                    return RedirectToAction("Index");
                }

            }
            return View();

        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repo.obtenerUsuarioPorEmail(login.Usuario);
                    if (e == null || e.password_usuario != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.email_usuario),
                        new Claim("FullName", e.nombre_usuario + " " + e.apellido_usuario),
                        new Claim(ClaimTypes.Role, e.rol_usuario),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            int res = repo.borrarUsuario(id);
            if (res == 0)
            {
                Console.WriteLine("Usuario no encontrado");
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
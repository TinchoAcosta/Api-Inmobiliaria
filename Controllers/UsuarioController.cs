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
        private readonly IWebHostEnvironment environment;

        public UsuarioController(RepositorioUsuario repo, IConfiguration configuration, IWebHostEnvironment environment) // <--- Lo inyectas en el constructor
        {
            this.repo = repo;
            this.environment = environment;
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
                // Verificar email existente
                var usuario = repo.obtenerUsuarioPorEmail(u.email_usuario);
                if (usuario != null)
                {
                    ModelState.AddModelError("", "El email ya existe");
                    return View();
                }

                // Hashear la contraseña
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: u.password_usuario,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                u.password_usuario = hashed;

                if (u.avatar_form != null && u.avatar_form.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/usuarios");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(u.avatar_form.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        u.avatar_form.CopyTo(stream);
                    }

                    // Guardar la ruta relativa
                    u.avatar_usuario = "/uploads/usuarios/" + fileName;

                }
                else
                {
                    u.avatar_usuario = null; // Ruta por defecto si no se sube avatar
                }

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
                        new Claim("Avatar", e.avatar_usuario ?? "/images/default-avatar.png"),
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
        [Authorize(Policy = "Administrador")]
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



        [Authorize]
        public IActionResult Edit(int id)
        {
            var usuario = repo.obtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Administrador"))
            {
                var emailLogueado = User.Identity?.Name;
                if (usuario.email_usuario != emailLogueado)
                {
                    return Forbid();
                }
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Usuario u, bool EliminarAvatar)
        {
            if (ModelState.IsValid)
            {

                var usuario = repo.obtenerUsuarioPorEmail(u.email_usuario);
                if (!User.IsInRole("Administrador") && usuario != null)
                {
                    var emailLogueado = User.Identity?.Name;
                    if (usuario.email_usuario != emailLogueado)
                    {
                        return Forbid();
                    }
                }

                //verificar si el email ya existe
                if (usuario != null && usuario.id_usuario != u.id_usuario)
                {
                    //Existe?? bien, tengo que ver si es mi propio email
                    ModelState.AddModelError("", "El email ya existe");
                    return View();
                }


                // Hashear la contraseña
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: u.password_usuario,
                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
                u.password_usuario = hashed;


                if (u.avatar_form != null && u.avatar_form.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/usuarios");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(u.avatar_form.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        u.avatar_form.CopyTo(stream);
                    }

                    // Guardar la ruta relativa
                    u.avatar_usuario = "/uploads/usuarios/" + fileName;

                }
                else
                {
                    if (EliminarAvatar)
                    {
                        u.avatar_usuario = "/images/default-avatar.png";
                    }
                    else
                    {
                        // Mantener el avatar existente si no se sube uno nuevo
                        var usuarioExistente = repo.obtenerUsuarioPorId(u.id_usuario);
                        u.avatar_usuario = usuarioExistente.avatar_usuario;
                    }
                }
                int res = repo.modificarUsuario(u);
                if (res != 0)
                {

                    return RedirectToAction("Index");
                }

            }
            return View();
        }


        [Authorize]
        public IActionResult ModificarPerfil()
        {
            var email = User.Identity.Name;
            var usuario = repo.obtenerUsuarioPorEmail(email);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }


        public IActionResult Details(int id)
        {
            var user = repo.obtenerUsuarioPorId(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }



    }
}
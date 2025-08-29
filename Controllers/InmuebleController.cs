using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly RepositorioPropietario repoPropietario;
        private readonly RepositorioInmueble repo;

        public InmuebleController(RepositorioPropietario repoPropietario, RepositorioInmueble repo)
        {
            this.repoPropietario = repoPropietario ?? throw new ArgumentNullException(nameof(repoPropietario));
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public IActionResult Create()
        {
            var propietarios = repoPropietario.ObtenerTodos();
            var tiposInmueble = repo.obtenerTiposInmueble();
            ViewBag.propietarios = propietarios;
            ViewBag.tiposInmueble = tiposInmueble;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                int res = repo.AgregarInmueble(inmueble);
                if (res != 0)
                {
                    return RedirectToAction("Index");
                }
            }

            var propietarios = repoPropietario.ObtenerTodos();
            var tiposInmueble = repo.obtenerTiposInmueble();
            ViewBag.propietarios = propietarios;
            ViewBag.tiposInmueble = tiposInmueble;
            return View();
        }


        public IActionResult Index()
        {
            var inmuebles = repo.obtenerTodos();
            return View(inmuebles);
        }
    }
}

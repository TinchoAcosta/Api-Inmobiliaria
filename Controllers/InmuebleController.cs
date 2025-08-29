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

        public IActionResult Edit(int id)
        {
            var inmueble = repo.obtenerPorId(id);
            var propietarios = repoPropietario.ObtenerTodos();
            var tiposInmueble = repo.obtenerTiposInmueble();
            ViewBag.propietarios = propietarios;
            ViewBag.tiposInmueble = tiposInmueble;
            return View(inmueble);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inmueble inmueble)
        {
            int res = repo.modificarInmueble(inmueble);
            if (res != 0)
            {
                return RedirectToAction("Index");
            }
            var inm = repo.obtenerPorId(inmueble.id_inmueble);
            var propietarios = repoPropietario.ObtenerTodos();
            var tiposInmueble = repo.obtenerTiposInmueble();
            ViewBag.propietarios = propietarios;
            ViewBag.tiposInmueble = tiposInmueble;
            return View(inm);
        }

        public IActionResult Delete(int id)
        {
            int res = repo.BorrarInmueble(id);
            if (res == 0)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            int res = repo.BorrarInmueble(id);
            if (res == 0)
            {
                Console.WriteLine("Inmueble no encontrado");
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var inmueble = repo.obtenerPorId(id);
            return View(inmueble);
        }

    }
}

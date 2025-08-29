using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario repo;

        public PropietarioController(RepositorioPropietario repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            var propietarios = repo.ObtenerTodos();
            return View(propietarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Propietario p)
        {
            if (ModelState.IsValid)
            {
                int res = repo.agregarPropietario(p);
                if (res != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(p);
        }

        public IActionResult Delete(int id)
        {
            int res = repo.borrarPropietario(id);
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
            int res = repo.borrarPropietario(id);
            if (res == 0)
            {
                Console.WriteLine("Propietario no encontrado");
                return NotFound();
            }
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var propietario = repo.obtenerPropietarioPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Propietario p)
        {
            if (ModelState.IsValid)
            {
                int res = repo.editarPropietario(p);
                if (res != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(p);
        }

        public IActionResult Details(int id)
        {
            var propietario = repo.obtenerPropietarioPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

    }
}

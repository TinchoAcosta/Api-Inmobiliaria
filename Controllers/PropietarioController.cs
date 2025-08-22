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

    }
}

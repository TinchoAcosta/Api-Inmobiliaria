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
    }
}

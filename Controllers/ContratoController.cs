using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers
{
    public class ContratoController : Controller
    {
        private readonly RepositorioContrato repo;

        public ContratoController(RepositorioContrato repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            var contratos = repo.obtenerTodos();
            return View(contratos);
        }
    }
}

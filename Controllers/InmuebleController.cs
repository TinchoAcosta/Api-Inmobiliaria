using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble repo;

        public InmuebleController(RepositorioInmueble repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            var inmuebles = repo.obtenerTodos();
            return View(inmuebles);
        }
    }
}

using inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria.Controllers
{
    [Authorize]
    public class TipoInmuebleController : Controller
    {

        private readonly RepositorioTipoInmueble repo;
        private readonly RepositorioInmueble repoInmueble;

        public TipoInmuebleController(RepositorioTipoInmueble repo, RepositorioInmueble repoInmueble)
        {
            this.repoInmueble = repoInmueble;
            this.repo = repo;
        }

        public IActionResult Index()
        {
            var tiposInmueble = repoInmueble.obtenerTiposInmueble();
            return View(tiposInmueble);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoInmueble t)
        {
            if (ModelState.IsValid)
            {
                int res = repo.agregarTipo(t);
                if (res != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var t = repo.obtenerTipoPorId(id);
            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TipoInmueble t)
        {
            if (ModelState.IsValid)
            {
                int res = repo.editarTipo(t);
                if (res != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            var tipo = repo.obtenerTipoPorId(t.id);
            return View(tipo);
        }
    }
}
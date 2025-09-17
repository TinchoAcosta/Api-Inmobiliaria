using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria.Controllers
{
    public class ContratoController : Controller
    {
        private readonly RepositorioContrato repo;
        private readonly RepositorioInquilino repoInquilino;
        private readonly RepositorioInmueble repoInmueble;

        public ContratoController(RepositorioContrato repo, RepositorioInquilino repoInquilino, RepositorioInmueble repoInmueble)
        {
            this.repo = repo;
            this.repoInquilino = repoInquilino;
            this.repoInmueble = repoInmueble;
        }

        public IActionResult Index()
        {

            var contratos = repo.obtenerTodos();
            return View(contratos);
        }

        public IActionResult Create()
        {
            var inquilinos = repoInquilino.obtenerTodos();
            var inmuebles = repoInmueble.obtenerTodos();
            ViewBag.inquilinos = inquilinos;
            ViewBag.inmuebles = inmuebles;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                int res = repo.AgregarContrato(contrato);
                if (res != 0)
                {
                    return RedirectToAction("Index");
                }
            }

            var inquilinos = repoInquilino.obtenerTodos();
            var inmuebles = repoInmueble.obtenerTodos();
            ViewBag.inquilinos = inquilinos;
            ViewBag.inmuebles = inmuebles;
            return View();
        }

        public IActionResult Edit(int id)
        {
            var inquilinos = repoInquilino.obtenerTodos();
            var inmuebles = repoInmueble.obtenerTodos();
            ViewBag.inquilinos = inquilinos;
            ViewBag.inmuebles = inmuebles;
            var contrato = repo.obtenerPorId(id);
            return View(contrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Contrato contrato)
        {
            int res = repo.modificarContrato(contrato);
            if (res != 0)
            {
                return RedirectToAction("Index");
            }
            var inquilinos = repoInquilino.obtenerTodos();
            var inmuebles = repoInmueble.obtenerTodos();
            ViewBag.inquilinos = inquilinos;
            ViewBag.inmuebles = inmuebles;
            var contr = repo.obtenerPorId(contrato.id_contrato);
            return View(contr);
        }

        public IActionResult Delete(int id)
        {
            int res = repo.borrarContrato(id);
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
            int res = repo.borrarContrato(id);
            if (res == 0)
            {
                Console.WriteLine("Contrato no encontrado");
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var contrato = repo.obtenerPorId(id);
            return View(contrato);
        }

    }
}

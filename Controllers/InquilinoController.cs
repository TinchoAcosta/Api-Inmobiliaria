using inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace inmobiliaria.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino repo;
        public InquilinoController(RepositorioInquilino repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            var inquilinos = repo.obtenerTodos();
            return View(inquilinos);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inquilino i)
        {
            if (ModelState.IsValid)
            {
                int res = repo.agregarInquilino(i);
                if (res != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(i);
        }

        public IActionResult Edit(int id)
        {
            var inquilino = repo.obtenerInquilinoPorId(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inquilino i)
        {
            if (ModelState.IsValid)
            {
                int res = repo.editarInquilino(i);
                if (res != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(i);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id)
        {
            int res = repo.borrarInquilino(id);
            if (res == 0)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var inquilino = repo.obtenerInquilinoPorId(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

    }
}
using inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;




namespace inmobiliaria.Controllers
{

    public class PagoController : Controller
    {

        private readonly RepositorioPago repo;
        private readonly RepositorioContrato repositorioContrato;

        public PagoController(RepositorioPago repo, RepositorioContrato repositorioContrato)
        {
            this.repo = repo;
            this.repositorioContrato = repositorioContrato;
        }

        public IActionResult FiltrarPorContrato(int contratoId)
        {
            //TODO AGREGAR CONTRATOS QUE TENGAN PAGOS ASOCIADOS
            //FALTARIA SEEDEAR LA BASE DE DATOS PARA QUE TENGA PAGOS POR CONTRATO
            var pagos = repo.listarPagoPorContrato(contratoId);
            var contratos = repositorioContrato.obtenerTodos();
            ViewBag.contratos = contratos;
            return View("Index", pagos);
        }


        public IActionResult Index()
        {
            var pagos = repo.obtenerTodos();

            var contratos = repositorioContrato.obtenerTodos();

            ViewBag.contratos = contratos;
            return View(pagos);
        }

        public IActionResult Create()
        {
            var contratos = repositorioContrato.obtenerContratosActivos();
            ViewBag.contratos = contratos;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pago pago)
        {
            if (ModelState.IsValid)
            {
                int res = repo.agregarPago(pago);
                if (res != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            var contratos = repositorioContrato.obtenerContratosActivos();
            ViewBag.contratos = contratos;
            return View();
        }

    }



}
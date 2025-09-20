using inmobiliaria.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using inmobiliaria.Services;




namespace inmobiliaria.Controllers
{

    [Authorize]
    public class PagoController : Controller
    {

        private readonly RepositorioPago repo;
        private readonly RepositorioContrato repositorioContrato;
        private readonly AuditoriaHelper helper;
        private readonly RepositorioUsuario repoUsuario;
        private readonly RepositorioAuditoria repoAuditoria;

        public PagoController(RepositorioPago repo, RepositorioContrato repositorioContrato, AuditoriaHelper helper, RepositorioUsuario repoUsuario, RepositorioAuditoria repoAuditoria)
        {
            this.repo = repo;
            this.repositorioContrato = repositorioContrato;
            this.helper = helper;
            this.repoUsuario = repoUsuario;
            this.repoAuditoria = repoAuditoria;
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
                if (res > 0)
                {
                    var email_usuario = User.Identity?.Name;
                    Usuario u = repoUsuario.obtenerUsuarioPorEmail(email_usuario);
                    helper.RegistrarAuditoria("Pago", res, "Creacion", u.id_usuario);
                    return RedirectToAction("Index");
                }
            }
            var contratos = repositorioContrato.obtenerContratosActivos();
            ViewBag.contratos = contratos;
            return View();
        }


        public IActionResult Edit(int id)
        {
            var Pago = repo.buscarPagoPorId(id);
            return View(Pago);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id_pago, string detalle_pago)
        {
            Pago p = new Pago(id_pago, 0, detalle_pago, DateTime.Now, 0, false, 0, null);

            var res = repo.editarPago(p);

            if (res != 0)
            {
                var email_usuario = User.Identity?.Name;
                Usuario u = repoUsuario.obtenerUsuarioPorEmail(email_usuario);
                helper.RegistrarAuditoria("Pago", id_pago, "Modificacion", u.id_usuario);
                return RedirectToAction("Index");
            }
            var pagos = repo.obtenerTodos();
            var contratos = repositorioContrato.obtenerTodos();
            ViewBag.contratos = contratos;
            return View(pagos);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            int res = repo.anularPago(id);
            if (res == 0)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var pago = repo.buscarPagoPorId(id);
            if (pago == null)
            {
                return NotFound();
            }
            var auditorias = repoAuditoria.ObtenerPorEntidad("Pago",id);
            ViewBag.auditorias = auditorias;
            return View(pago);
        }

        public IActionResult AnularPago(int id)
        {
            int res = repo.anularPago(id);
            if (res == 0)
            {
                return NotFound();
            }
            var email_usuario = User.Identity?.Name;
            Usuario u = repoUsuario.obtenerUsuarioPorEmail(email_usuario);
            helper.RegistrarAuditoria("Pago", id, "Anulacion", u.id_usuario);
            return RedirectToAction("Index");
        }

    }



}
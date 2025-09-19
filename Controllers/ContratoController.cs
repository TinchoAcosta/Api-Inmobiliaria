using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using inmobiliaria.Services;

namespace inmobiliaria.Controllers
{
    [Authorize]
    public class ContratoController : Controller
    {
        private readonly RepositorioContrato repo;
        private readonly RepositorioInquilino repoInquilino;
        private readonly RepositorioInmueble repoInmueble;
        private readonly AuditoriaHelper helper;
        private readonly RepositorioUsuario repoUsuario;

        public ContratoController(RepositorioContrato repo, RepositorioInquilino repoInquilino, RepositorioInmueble repoInmueble, AuditoriaHelper helper, RepositorioUsuario repoUsuario)
        {
            this.repo = repo;
            this.repoInquilino = repoInquilino;
            this.repoInmueble = repoInmueble;
            this.helper = helper;
            this.repoUsuario = repoUsuario;
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
            if (contrato.fechaInicio_contrato >= contrato.fechaFin_contrato)
            {
                ModelState.AddModelError("fechaFin_contrato", "La fecha de inicio debe ser anterior a la fecha de fin.");
            }
            if (repo.ExisteSolapamiento(contrato.idInmueble, contrato.fechaInicio_contrato, contrato.fechaFin_contrato))
            {
                ModelState.AddModelError("", "Ya existe un contrato activo para este inmueble en el rango de fechas seleccionado.");
            }
            if (ModelState.IsValid)
            {
                int res = repo.AgregarContrato(contrato);
                if (res > 0)
                {
                    var email_usuario = User.Identity?.Name;
                    Usuario u = repoUsuario.obtenerUsuarioPorEmail(email_usuario);
                    helper.RegistrarAuditoria("Contrato", res, "Creacion", u.id_usuario);
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
            if (contrato.fechaInicio_contrato >= contrato.fechaFin_contrato)
            {
                ModelState.AddModelError("fechaFin_contrato", "La fecha de inicio debe ser anterior a la fecha de fin.");
            }
            if (repo.ExisteSolapamiento(contrato.idInmueble, contrato.fechaInicio_contrato, contrato.fechaFin_contrato, contrato.id_contrato))
            {
                ModelState.AddModelError("", "Ya existe un contrato activo para este inmueble en el rango de fechas seleccionado.");
            }

            if (ModelState.IsValid)
            {
                int res = repo.modificarContrato(contrato);
                if (res != 0)
                {
                    var email_usuario = User.Identity?.Name;
                    Usuario u = repoUsuario.obtenerUsuarioPorEmail(email_usuario);
                    helper.RegistrarAuditoria("Contrato", contrato.id_contrato, "Modificacion", u.id_usuario);
                    return RedirectToAction("Index");
                }
            }
            var inquilinos = repoInquilino.obtenerTodos();
            var inmuebles = repoInmueble.obtenerTodos();
            ViewBag.inquilinos = inquilinos;
            ViewBag.inmuebles = inmuebles;
            var contr = repo.obtenerPorId(contrato.id_contrato);
            return View(contr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            int res = repo.borrarContrato(id);
            if (res == 0)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var contrato = repo.obtenerPorId(id);
            return View(contrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contratar(int idInmueble, DateTime fechaInicio_contrato, DateTime fechaFin_contrato)
        {
            var inmueble = repoInmueble.obtenerPorId(idInmueble);


            var inquilinos = repoInquilino.obtenerTodos();
            ViewBag.fechaInicio = fechaInicio_contrato.ToString("yyyy-MM-dd");
            ViewBag.fechaFin = fechaFin_contrato.ToString("yyyy-MM-dd");
            ViewBag.inmueble = inmueble;
            ViewBag.inquilinos = inquilinos;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarContratacion(Contrato contrato, int idInmueble, DateTime fechaInicio_contrato, DateTime fechaFin_contrato)
        {

            if (contrato.fechaInicio_contrato >= contrato.fechaFin_contrato)
            {
                ModelState.AddModelError("fechaFin_contrato", "La fecha de inicio debe ser anterior a la fecha de fin.");
            }
            if (repo.ExisteSolapamiento(contrato.idInmueble, contrato.fechaInicio_contrato, contrato.fechaFin_contrato))
            {
                ModelState.AddModelError("", "Ya existe un contrato activo para este inmueble en el rango de fechas seleccionado.");
            }
            if (ModelState.IsValid)
            {
                int res = repo.AgregarContrato(contrato);
                if (res > 0)
                {
                    var email_usuario = User.Identity?.Name;
                    Usuario u = repoUsuario.obtenerUsuarioPorEmail(email_usuario);
                    helper.RegistrarAuditoria("Contrato", res, "Creacion", u.id_usuario);
                    return RedirectToAction("Index");
                }
            }

            var inmueble = repoInmueble.obtenerPorId(idInmueble);
            var inquilinos = repoInquilino.obtenerTodos();

            ViewBag.fechaInicio = fechaInicio_contrato.ToString("yyyy-MM-dd");
            ViewBag.fechaFin = fechaFin_contrato.ToString("yyyy-MM-dd");
            ViewBag.inmueble = inmueble;
            ViewBag.inquilinos = inquilinos;
            return View("Contratar", contrato);
        }

        public IActionResult BajaContrato(int id)
        {
            int res = repo.AnularContrato(id);
            if (res == 0)
            {
                return NotFound();
            }
            var email_usuario = User.Identity?.Name;
            Usuario u = repoUsuario.obtenerUsuarioPorEmail(email_usuario);
            helper.RegistrarAuditoria("Contrato", id, "Anulacion", u.id_usuario);
            return RedirectToAction("Index");
        }

        public IActionResult ContratosAnulados()
        {
            var contratosAnulados = repo.obtenerContratosAnulados();
            return View(contratosAnulados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Renovar(int idInmueble, int idInquilino, int idContrato)
        {
            var inmueble = repoInmueble.obtenerPorId(idInmueble);
            var inquilino = repoInquilino.obtenerInquilinoPorId(idInquilino);

            var contrato = new Contrato
            {
                id_contrato = idContrato,
                idInmueble = idInmueble,
                idInquilino = idInquilino
            };

            ViewBag.inmueble = inmueble;
            ViewBag.inquilino = inquilino;

            return View("Renovar", contrato);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarRenovacion(Contrato contrato)
        {
            if (contrato.fechaInicio_contrato >= contrato.fechaFin_contrato)
            {
                ModelState.AddModelError("fechaFin_contrato", "La fecha de inicio debe ser anterior a la fecha de fin.");
            }
            if (repo.ExisteSolapamiento(contrato.idInmueble, contrato.fechaInicio_contrato, contrato.fechaFin_contrato, contrato.id_contrato))
            {
                ModelState.AddModelError("", "Ya existe un contrato activo para este inmueble en el rango de fechas seleccionado.");
            }
            if (ModelState.IsValid)
            {
                int res = repo.renovarContrato(contrato.id_contrato, contrato.monto_contrato, contrato.fechaInicio_contrato, contrato.fechaFin_contrato);
                if (res != 0)
                {
                    var email_usuario = User.Identity?.Name;
                    Usuario u = repoUsuario.obtenerUsuarioPorEmail(email_usuario);
                    helper.RegistrarAuditoria("Contrato",contrato.id_contrato,"Renovacion",u.id_usuario);
                    return RedirectToAction("Index");
                }
            }

            var inmueble = repoInmueble.obtenerPorId(contrato.idInmueble);
            var inquilino = repoInquilino.obtenerInquilinoPorId(contrato.idInquilino);
            ViewBag.inmueble = inmueble;
            ViewBag.inquilino = inquilino;
            ViewBag.idContrato = contrato.id_contrato;
            return View("Renovar", contrato);
        }

    }
}

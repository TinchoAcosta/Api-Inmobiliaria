using inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace inmobiliaria.API_Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PagoController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment environment;

        public PagoController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
        {
            this.contexto = contexto;
            this.config = config;
            environment = env;
        }

        [HttpGet("contrato/{id}")]
        public async Task<ActionResult<List<Pago>>> Get(int id)
        {
            var idClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(idClaim))
                return Unauthorized("Token inválido");

            int idPropietario = int.Parse(idClaim);

            var contrato = await contexto.Contrato
         .FirstOrDefaultAsync(c => c.id_contrato == id);

            if (contrato == null)
                return NotFound("Contrato no encontrado.");

            var inmueble = await contexto.Inmueble.FindAsync(contrato.idInmueble);
            if (inmueble == null)
                return NotFound("Inmueble del contrato no encontrado.");

            if (inmueble.PropietarioId != idPropietario)
                return Unauthorized("No tiene permiso para acceder a este contrato.");

            var pagos = await contexto.Pago
                    .Where(p => p.contratoId == id)
                    .Include(p => p.Contrato)
                    .AsNoTracking()
                    .ToListAsync();

            if (pagos == null || pagos.Count == 0)
                return NotFound("No hay pagos asociados a este contrato.");
            return pagos;

        }
    }
}
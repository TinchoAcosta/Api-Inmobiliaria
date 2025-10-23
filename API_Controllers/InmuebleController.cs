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
    public class InmuebleController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment environment;

        public InmuebleController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
        {
            this.contexto = contexto;
            this.config = config;
            environment = env;
        }

        [HttpGet]
        public async Task<ActionResult<List<Inmueble>>> Get()
        {
            try
            {
                // 1. Obtener el ID del propietario desde el token
                var idClaim = User.FindFirst("Id")?.Value;
                if (string.IsNullOrEmpty(idClaim))
                    return Unauthorized("Token inválido");

                int idPropietario = int.Parse(idClaim);

                // 2. Filtrar los inmuebles por ese propietario
                var inmuebles = await contexto.Inmueble
            .Include(i => i.propietario_inmueble) // Incluye todos los datos del propietario
            .Where(i => i.PropietarioId == idPropietario)
            .ToListAsync();

                return inmuebles;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> cambiarDisponibilidad([FromBody] Inmueble inmuebleCliente)
        {
            try
            {
                // 1. Validar token y obtener ID del propietario
                var idClaim = User.FindFirst("Id")?.Value;
                if (string.IsNullOrEmpty(idClaim))
                    return Unauthorized("Token inválido");

                int idPropietario = int.Parse(idClaim);

                // 2. Buscar el inmueble en base al ID recibido y validar propiedad
                var inmuebleBD = await contexto.Inmueble
                    .FirstOrDefaultAsync(i => i.id_inmueble == inmuebleCliente.id_inmueble && i.PropietarioId == idPropietario);

                if (inmuebleBD == null)
                    return NotFound("Inmueble no encontrado o no pertenece al propietario");

                // 3. Actualizar solo el campo de disponibilidad
                inmuebleBD.disponibilidad_inmueble = inmuebleCliente.disponibilidad_inmueble;

                // 4. Guardar cambios
                contexto.Inmueble.Update(inmuebleBD);
                await contexto.SaveChangesAsync();

                return Ok(new { mensaje = "Disponibilidad actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("GetContratoVigente")]
        public async Task<ActionResult<List<Inmueble>>> obtenerInmueblesConContrato()
        {
            var idClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(idClaim))
                return Unauthorized("Token inválido");

            int idPropietario = int.Parse(idClaim);

            var inmuebles = await contexto.Inmueble
                .Where(i => i.PropietarioId == idPropietario && i.tieneContratoVigente)
                .Where(i => contexto.Contrato.Any(c =>
                    c.idInmueble == i.id_inmueble &&
                    c.estado==1 &&
                    c.fechaInicio_contrato <= DateTime.Now &&
                    c.fechaFin_contrato >= DateTime.Now))
                .ToListAsync();

            return inmuebles;
        }
    }
}
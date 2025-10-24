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
    public class ContratoController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment environment;

        public ContratoController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
        {
            this.contexto = contexto;
            this.config = config;
            environment = env;
        }

        [HttpGet("inmueble/{id}")]
        public async Task<ActionResult<Contrato>> Get(int id)
        {
            // 1. Obtener el ID del propietario logueado desde el token
            var idClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(idClaim))
                return Unauthorized("Token inválido");

            int idPropietario = int.Parse(idClaim);

            // 2. Buscar el inmueble por ID
            var inmueble = await contexto.Inmueble.FindAsync(id);
            if (inmueble == null)
                return NotFound("Inmueble no encontrado.");

            // 3. Validar que el inmueble pertenezca al propietario logueado
            if (inmueble.PropietarioId != idPropietario)
                return Unauthorized("No tiene permiso para acceder a este inmueble.");

            // 4. Verificar si el inmueble tiene contrato vigente
            if (!inmueble.tieneContratoVigente)
                return NotFound("Inmueble sin contrato.");

            // 5. Buscar el contrato asociado al inmueble
            var contrato = await contexto.Contrato
            .Where(c => c.estado && c.idInmueble == id)
     .Include(c => c.Inquilino)
     .Include(c => c.Inmueble)
         .ThenInclude(i => i.propietario_inmueble)
     .FirstOrDefaultAsync(c => c.idInmueble == id);


            if (contrato == null)
                return NotFound("Inmueble sin contrato.");

            // 6. Retornar el contrato
            return contrato;
        }
    }
}
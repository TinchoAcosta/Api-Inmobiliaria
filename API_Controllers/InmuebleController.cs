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
    }
}
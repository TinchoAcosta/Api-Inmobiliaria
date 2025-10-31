using inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


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
                    c.estado &&
                    c.fechaInicio_contrato <= DateTime.Now &&
                    c.fechaFin_contrato >= DateTime.Now))
                .ToListAsync();

            return inmuebles;
        }

        [HttpPost("cargar")]
        public async Task<IActionResult> cargarInmueble([FromForm] IFormFile imagen, [FromForm] string inmueble)
        {
            if (imagen == null || imagen.Length == 0)
                return BadRequest("No se ha seleccionado ninguna imagen.");

            if (string.IsNullOrWhiteSpace(inmueble))
                return BadRequest("El JSON del inmueble es requerido.");

            var idClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(idClaim))
                return Unauthorized("Token inválido");

            if (!int.TryParse(idClaim, out int idPropietario))
                return Unauthorized("Id de propietario inválido");

            // Deserializar el JSON
            Inmueble nuevoInmueble;
            try
            {
                nuevoInmueble = JsonSerializer.Deserialize<Inmueble>(inmueble);
            }
            catch (JsonException ex)
            {
                return BadRequest($"Error al deserializar el JSON del inmueble: {ex.Message}");
            }

            // Asignar el propietario al inmueble
            nuevoInmueble.PropietarioId = idPropietario;
            contexto.Inmueble.Add(nuevoInmueble);
            await contexto.SaveChangesAsync();

            //subir la imagen a mi carpeta wwwroot/uploads/inmuebles
            var uploadsFolder = Path.Combine(environment.WebRootPath, "uploads/inmuebles");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }//poner nombre de archivo como "inmueble_foto"+id del inmueble
            var fileName = $"inmueble_foto{nuevoInmueble.id_inmueble}.jpg";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imagen.CopyToAsync(fileStream);
            }

            // Actualizar inmueble con la ruta de imagen
            nuevoInmueble.imagen = $"/uploads/inmuebles/{fileName}";
            contexto.Inmueble.Update(nuevoInmueble);
            await contexto.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Inmueble creado correctamente",
                id = nuevoInmueble.id_inmueble,
                imagen = nuevoInmueble.imagen
            });


        }
    }
}
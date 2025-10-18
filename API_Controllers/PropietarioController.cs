using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using inmobiliaria.Models;


namespace inmobiliaria.API_Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PropietarioController : ControllerBase
    {

        private readonly DataContext contexto;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment environment;

        public PropietarioController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
        {
            this.contexto = contexto;
            this.config = config;
            environment = env;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPerfil()
        {
            try
            {
                // Obtener el ID del token
                var idClaim = User.FindFirst("Id")?.Value;
                if (string.IsNullOrEmpty(idClaim))
                    return Unauthorized("Token inválido");

                int id = int.Parse(idClaim);

                // Buscar el propietario en la base de datos
                var p = await contexto.Propietario
                    .FirstOrDefaultAsync(x => x.id_propietario == id);

                if (p == null)
                    return NotFound("Propietario no encontrado");

                // Construir el JSON de respuesta
                var perfil = new
                {
                    idPropietario = p.id_propietario,
                    nombre = p.nombre_propietario,
                    apellido = p.apellido_propietario,
                    dni = p.dni_propietario,
                    telefono = p.telefono_propietario,
                    email = p.email_propietario,
                };

                return Ok(perfil);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"] ?? ""),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = await contexto.Propietario.FirstOrDefaultAsync(x => x.email_propietario == loginView.Usuario);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var secreto = config["TokenAuthentication:SecretKey"];
                    if (string.IsNullOrEmpty(secreto))
                        throw new Exception("Falta configurar TokenAuthentication:Secret");
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secreto));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.email_propietario),
                        new Claim("Id", p.id_propietario.ToString()),
                        new Claim("FullName", p.nombre_propietario + " " + p.apellido_propietario),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("actualizar")]
        public async Task<ActionResult<Propietario>> ActualizarPropietario([FromBody] Propietario propietarioActualizado)
        {
            try
            {
                // 1. Obtener el ID del propietario desde el token
                var idClaim = User.FindFirst("Id")?.Value;
                if (string.IsNullOrEmpty(idClaim))
                    return Unauthorized("Token inválido");

                int idPropietarioToken = int.Parse(idClaim);

                // 2. Validar que el ID del body coincida con el del token
                if (propietarioActualizado.id_propietario != idPropietarioToken)
                    return Unauthorized("Modificación no autorizada");

                // 3. Buscar el propietario en la base
                var propietarioDb = await contexto.Propietario
                    .FirstOrDefaultAsync(p => p.id_propietario == idPropietarioToken);

                if (propietarioDb == null)
                    return NotFound("Propietario no encontrado");

                // 4. Actualizar campos permitidos
                propietarioDb.nombre_propietario = propietarioActualizado.nombre_propietario;
                propietarioDb.apellido_propietario = propietarioActualizado.apellido_propietario;
                propietarioDb.dni_propietario = propietarioActualizado.dni_propietario;
                propietarioDb.telefono_propietario = propietarioActualizado.telefono_propietario;
                propietarioDb.email_propietario = propietarioActualizado.email_propietario;

                // 5. Guardar cambios
                await contexto.SaveChangesAsync();

                // 6. Retornar el objeto actualizado (sin contraseña)
                return Ok(propietarioDb);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> cambiarContra([FromForm] String currentPassword, [FromForm] String newPassword)
        {
             try
    {
        // 1. Obtener el ID del propietario desde el token
        var idClaim = User.FindFirst("Id")?.Value;
        if (string.IsNullOrEmpty(idClaim))
            return Unauthorized("Token inválido");

        int idPropietario = int.Parse(idClaim);

        // 2. Buscar al propietario en la base
        var propietario = await contexto.Propietario
            .FirstOrDefaultAsync(p => p.id_propietario == idPropietario);

        if (propietario == null)
            return NotFound("Propietario no encontrado");

        // 3. Hashear la contraseña actual ingresada
        string hashedCurrent = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: currentPassword,
            salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"] ?? ""),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 1000,
            numBytesRequested: 256 / 8));

        // 4. Verificar que coincida con la almacenada
        if (hashedCurrent != propietario.Clave)
            return StatusCode(403, "La contraseña actual es incorrecta");

        // 5. Hashear la nueva contraseña
        string hashedNew = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: newPassword,
            salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"] ?? ""),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 1000,
            numBytesRequested: 256 / 8));

        // 6. Guardar la nueva contraseña
        propietario.Clave = hashedNew;
        await contexto.SaveChangesAsync();

        return Ok("Contraseña actualizada correctamente");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error interno: {ex.Message}");
    }
        }


    }
}
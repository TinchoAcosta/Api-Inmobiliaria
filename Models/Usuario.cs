using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models

{
    public class Usuario
    {
        public enum Roles
        {
            Administrador,
            Empleado
        }
        public int id_usuario { get; set; }

        [Required]
        public string nombre_usuario { get; set; } = string.Empty;

        [Required]
        public string apellido_usuario { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string email_usuario { get; set; } = string.Empty;

        public string password_usuario { get; set; } = string.Empty;

        public string rol_usuario { get; set; } = Roles.Empleado.ToString();

        public string avatar_usuario { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? avatar_form { get; set; }
    }
}

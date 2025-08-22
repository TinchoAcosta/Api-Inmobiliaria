using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class Inquilino
    {
        public int id_inquilino { get; set; }

        [Required(ErrorMessage = "El DNI es requerido")]
        public int dni_inquilino { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string nombre_inquilino { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        public string apellido_inquilino { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string email_inquilino { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        public string telefono_inquilino { get; set; }

        public Inquilino()
        {
            id_inquilino = 0;
            dni_inquilino = 0;
            nombre_inquilino = string.Empty;
            apellido_inquilino = string.Empty;
            email_inquilino = string.Empty;
            telefono_inquilino = string.Empty;


        } // Constructor vacío para EF / Model Binding

        public Inquilino(int dni, string nombre, string apellido, string email, string telefono)
        {
            dni_inquilino = dni;
            nombre_inquilino = nombre;
            apellido_inquilino = apellido;
            email_inquilino = email;
            telefono_inquilino = telefono;
        }
    }
}

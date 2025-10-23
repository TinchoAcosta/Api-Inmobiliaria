using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{
    public class Inquilino
    {
        [Key]
    [Column("id_inquilino")]
    public int id_inquilino { get; set; }

    [Required(ErrorMessage = "El DNI es requerido")]
    [Column("dni_inquilino")]
    public int dni_inquilino { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [Column("nombre_inquilino")]
    public string nombre_inquilino { get; set; }

    [Required(ErrorMessage = "El apellido es requerido")]
    [Column("apellido_inquilino")]
    public string apellido_inquilino { get; set; }

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [Column("email")]
    public string email_inquilino { get; set; }

    [Required(ErrorMessage = "El teléfono es requerido")]
    [Column("telefono_inquilino")]
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

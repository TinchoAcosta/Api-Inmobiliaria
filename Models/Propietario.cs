

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace inmobiliaria.Models
{
    public class Propietario
    {
        [Key]
        [JsonPropertyName("idPropietario")]
        public int id_propietario { get; set; }

        [JsonPropertyName("dni")]
        [Required(ErrorMessage = "El DNI es requerido")] // Indica que este campo es obligatorio
        public int? dni_propietario { get; set; }

        [JsonPropertyName("nombre")]
        [Required(ErrorMessage = "El nombre es requerido")] // Campo obligatorio
        public string nombre_propietario { get; set; }

        [JsonPropertyName("apellido")]
        [Required(ErrorMessage = "El apellido es requerido")] // Campo obligatorio
        public string apellido_propietario { get; set; }

        [JsonPropertyName("email")]
        [Required(ErrorMessage = "El email es requerido")] // Campo obligatorio
        [EmailAddress(ErrorMessage = "El formato del email no es válido")] // Valida que sea un formato de email válido
        public string email_propietario { get; set; }

        [JsonPropertyName("telefono")]
        [Required(ErrorMessage = "El teléfono es requerido")] // Campo obligatorio
        public string telefono_propietario { get; set; }

        [JsonIgnore]
        [Column("password_propietario")]
        public string? Clave { get; set; }

        // Esta es la collection de inmuebles asociados al propietario
        [JsonIgnore]
        public ICollection<Inmueble> Inmuebles { get; set; }




        public Propietario()
        {
            nombre_propietario = "";
            apellido_propietario = "";
            email_propietario = "";
            telefono_propietario = "";
            Inmuebles = new List<Inmueble>();
        }

        public Propietario(int dni, string nombre, string apellido, string email, string telefono)
        {
            dni_propietario = dni;
            nombre_propietario = nombre;
            apellido_propietario = apellido;
            email_propietario = email;
            telefono_propietario = telefono;
            Inmuebles = new List<Inmueble>();
        }

        public Propietario(int dni, string nombre, string apellido, string email, string telefono, string clave)
        {
            dni_propietario = dni;
            nombre_propietario = nombre;
            apellido_propietario = apellido;
            email_propietario = email;
            telefono_propietario = telefono;
            Inmuebles = new List<Inmueble>();
            Clave = clave;
        }





    }
}


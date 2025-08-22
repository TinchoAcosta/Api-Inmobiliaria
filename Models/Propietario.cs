

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace inmobiliaria.Models
{
    public class Propietario
    {
        [Key] // Indica que esta propiedad es la clave primaria de la tabla
        public int id_propietario { get; set; }

        [Required(ErrorMessage = "El DNI es requerido")] // Indica que este campo es obligatorio
        [Display(Name = "DNI")] // Define el nombre que se mostrará en las vistas
        public int dni_propietario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")] // Campo obligatorio
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 50 caracteres")] // Valida la longitud del texto
        [Display(Name = "Contraseña")] // Nombre para mostrar
        public string contrasena_propietario { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")] // Campo obligatorio
        [StringLength(50, ErrorMessage = "El nombre no puede ser mayor a 50 caracteres")] // Máximo 50 caracteres
        [Display(Name = "Nombre")] // Nombre para mostrar
        public string nombre_propietario { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")] // Campo obligatorio
        [StringLength(50, ErrorMessage = "El apellido no puede ser mayor a 50 caracteres")] // Máximo 50 caracteres
        [Display(Name = "Apellido")] // Nombre para mostrar
        public string apellido_propietario { get; set; }

        [Required(ErrorMessage = "El email es requerido")] // Campo obligatorio
        [EmailAddress(ErrorMessage = "El formato del email no es válido")] // Valida que sea un formato de email válido
        [Display(Name = "Email")] // Nombre para mostrar
        public string email_propietario { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")] // Campo obligatorio
        [Display(Name = "Teléfono")] // Nombre para mostrar
        public string telefono_propietario { get; set; }

        // Esta es la collection de inmuebles asociados al propietario
        public ICollection<Inmueble> Inmuebles { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede ser mayor a 50 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, ErrorMessage = "El apellido no puede ser mayor a 50 caracteres")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }




        public Propietario()
        {
            contrasena_propietario = "";
            nombre_propietario = "";
            apellido_propietario = "";
            email_propietario = "";
            telefono_propietario = "";
            Inmuebles = new List<Inmueble>();
        }

        public Propietario(int dni, string contrasena, string nombre, string apellido, string email, string telefono)
        {
            dni_propietario = dni;
            contrasena_propietario = contrasena;
            nombre_propietario = nombre;
            apellido_propietario = apellido;
            email_propietario = email;
            telefono_propietario = telefono;
            Inmuebles = new List<Inmueble>();
        }





    }
}


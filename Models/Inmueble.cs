
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace inmobiliaria.Models
{
    public class Inmueble
    {

        [Key]
        [Display(Name = "Codigo-Inmueble")]
        public int id_inmueble { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [Display(Name = "Dirección")]
        public string? direccion_inmueble { get; set; }

        [Required(ErrorMessage = "Los ambientes son requeridos")]
        [Display(Name = "Ambientes")]
        public int ambientes_inmueble { get; set; }


        [Required(ErrorMessage = "La superficie es requerida")]
        public int superficie_inmueble { get; set; }

        [Required(ErrorMessage = "La latitud es requerida")]
        public decimal lat_inmueble { get; set; }

        [Required(ErrorMessage = "La longitud es requerida")]
        public decimal long_inmueble { get; set; }

        [Display(Name = "Propietario")]
        public int PropietarioId { get; set; }

        //[ForeignKey(nameof(PropietarioId))]
        //[BindNever] // esto hace que el modelo no se vincule automáticamente con el formulario
        public Propietario? propietario_inmueble { get; set; }
        public string? portada_inmueble { get; set; }

        //[NotMapped] // Indica que esta propiedad no se mapea a una columna de la base de datos
        //public IFormFile? PortadaFile { get; set; }

        //[ForeignKey(nameof(id_inmueble))]
        public IList<Imagen> imagenes_inmueble { get; set; } = new List<Imagen>();

        //[NotMapped]
        public bool estaActivoInmueble { get; set; } = true;
    }

}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace inmobiliaria.Models
{
    public class Inmueble
    {
        public int id_inmueble { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(100, ErrorMessage = "La dirección no puede superar los 100 caracteres")]
        public string? direccion_inmueble { get; set; }

        [Required(ErrorMessage = "Los ambientes son requeridos")]
        [Range(1, 50, ErrorMessage = "Debe ingresar entre 1 y 50 ambientes")]
        public int ambientes_inmueble { get; set; }


        [Required(ErrorMessage = "La superficie es requerida")]
        [Range(1, 10000, ErrorMessage = "Debe ingresar una superficie válida")]
        public int superficie_inmueble { get; set; }
        [Required(ErrorMessage = "La latitud es requerida")]
        [Range(typeof(decimal), "-90", "90", ErrorMessage = "La latitud debe estar entre -90 y 90")]
        [Column(TypeName = "decimal(9,6)")]
        public decimal lat_inmueble { get; set; }
        [Required(ErrorMessage = "La longitud es requerida")]
        [Range(typeof(decimal), "-180", "180", ErrorMessage = "La longitud debe estar entre -180 y 180")]
        [Column(TypeName = "decimal(9,6)")]
        public decimal long_inmueble { get; set; }
        public int PropietarioId { get; set; }

        [Required(ErrorMessage = "El uso del inmueble es requerido")]
        public String uso_inmueble { get; set; }
        [Required(ErrorMessage = "El tipo de inmueble es requerido")]
        public int tipo_inmueble { get; set; }
        public TipoInmueble? tipoInmueble { get; set; }
        public Propietario? propietario_inmueble { get; set; }
        public string? portada_inmueble { get; set; }
        public IList<Imagen> imagenes_inmueble { get; set; } = new List<Imagen>();

        //borrado
        public bool estaActivoInmueble { get; set; } = true;
        //disponibilidad
        public bool disponibilidad_inmueble { get; set; } = true;



        // El error daba porque no teniamos el constructor vacio 
        public Inmueble() { }

        public Inmueble(string? direccion_inmueble, int ambientes_inmueble, int superficie_inmueble, decimal lat_inmueble, decimal long_inmueble, int PropietarioId, string uso_inmueble, int tipo_inmueble, Propietario? propietario_inmueble)
        {
            this.direccion_inmueble = direccion_inmueble;
            this.ambientes_inmueble = ambientes_inmueble;
            this.superficie_inmueble = superficie_inmueble;
            this.lat_inmueble = lat_inmueble;
            this.long_inmueble = long_inmueble;
            this.uso_inmueble = uso_inmueble;
            this.tipo_inmueble = tipo_inmueble;
            this.PropietarioId = PropietarioId;
            this.estaActivoInmueble = true;
            this.propietario_inmueble = propietario_inmueble;
        }

        public Inmueble(string? direccion_inmueble, int ambientes_inmueble, int superficie_inmueble, decimal lat_inmueble, decimal long_inmueble, int PropietarioId, string uso_inmueble, int tipo_inmueble, Propietario? propietario_inmueble, TipoInmueble? tipoInmueble)
        {
            this.direccion_inmueble = direccion_inmueble;
            this.ambientes_inmueble = ambientes_inmueble;
            this.superficie_inmueble = superficie_inmueble;
            this.lat_inmueble = lat_inmueble;
            this.long_inmueble = long_inmueble;
            this.uso_inmueble = uso_inmueble;
            this.tipo_inmueble = tipo_inmueble;
            this.PropietarioId = PropietarioId;
            this.estaActivoInmueble = true;
            this.propietario_inmueble = propietario_inmueble;
            this.tipoInmueble = tipoInmueble;
        }
        public Inmueble(string? direccion_inmueble, int ambientes_inmueble, int superficie_inmueble, decimal lat_inmueble, decimal long_inmueble, int PropietarioId, string uso_inmueble, int tipo_inmueble, Propietario? propietario_inmueble, TipoInmueble? tipoInmueble, bool disponibilidad_inmueble)
        {
            this.direccion_inmueble = direccion_inmueble;
            this.ambientes_inmueble = ambientes_inmueble;
            this.superficie_inmueble = superficie_inmueble;
            this.lat_inmueble = lat_inmueble;
            this.long_inmueble = long_inmueble;
            this.uso_inmueble = uso_inmueble;
            this.tipo_inmueble = tipo_inmueble;
            this.PropietarioId = PropietarioId;
            this.estaActivoInmueble = true;
            this.propietario_inmueble = propietario_inmueble;
            this.tipoInmueble = tipoInmueble;
            this.disponibilidad_inmueble = disponibilidad_inmueble;
        }

    };
}
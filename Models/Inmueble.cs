
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
        public string? direccion_inmueble { get; set; }
        public int ambientes_inmueble { get; set; }
        public int superficie_inmueble { get; set; }
        public decimal lat_inmueble { get; set; }
        public decimal long_inmueble { get; set; }
        public int PropietarioId { get; set; }

        public String uso_inmueble { get; set; }

        public Propietario? propietario_inmueble { get; set; }
        public string? portada_inmueble { get; set; }
        public IList<Imagen> imagenes_inmueble { get; set; } = new List<Imagen>();
        public bool estaActivoInmueble { get; set; } = true;



        // El error daba porque no teniamos el constructor vacio 
        public Inmueble() { }

        public Inmueble(string? direccion_inmueble, int ambientes_inmueble, int superficie_inmueble, decimal lat_inmueble, decimal long_inmueble, int PropietarioId, string uso_inmueble, Propietario? propietario_inmueble)
        {
            this.direccion_inmueble = direccion_inmueble;
            this.ambientes_inmueble = ambientes_inmueble;
            this.superficie_inmueble = superficie_inmueble;
            this.lat_inmueble = lat_inmueble;
            this.long_inmueble = long_inmueble;
            this.uso_inmueble = uso_inmueble;
            this.PropietarioId = PropietarioId;
            this.estaActivoInmueble = true;
            this.propietario_inmueble = propietario_inmueble;
        }

    };





}
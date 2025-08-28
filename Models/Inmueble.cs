
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

        public Propietario? propietario_inmueble { get; set; }
        public string? portada_inmueble { get; set; }

        public IList<Imagen> imagenes_inmueble { get; set; } = new List<Imagen>();
        public bool estaActivoInmueble { get; set; } = true;
    }

}
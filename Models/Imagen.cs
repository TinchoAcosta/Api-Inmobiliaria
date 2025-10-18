
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace inmobiliaria.Models
{

    public class Imagen
    {
        [Key]
        public int id_imagen { get; set; }
        public int inmuebleId { get; set; }
        public string url_imagen { get; set; } = "";
        [NotMapped]
        public IFormFile? archivo_imagen { get; set; } = null;
    }
}
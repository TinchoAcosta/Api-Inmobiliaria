
namespace inmobiliaria.Models
{

    public class Imagen
    {
        public int id_imagen { get; set; }
        public int inmuebleId { get; set; }
        public string url_imagen { get; set; } = "";
        public IFormFile? archivo_imagen { get; set; } = null;
    }
}
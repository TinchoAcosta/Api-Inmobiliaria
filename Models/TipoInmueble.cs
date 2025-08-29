
namespace inmobiliaria.Models
{
    public class TipoInmueble
    {
        public int id { get; set; }
        public string descripcion { get; set; }


        public TipoInmueble(int id, string descripcion)
        {
            this.id = id;
            this.descripcion = descripcion;
        }

    }
}
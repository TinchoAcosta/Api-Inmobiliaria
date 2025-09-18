
namespace inmobiliaria.Models
{
    public class TipoInmueble
    {
        public int id { get; set; }
        public string descripcion { get; set; }


        public TipoInmueble() {  }

        public TipoInmueble(string descripcion)
        {
            this.descripcion = descripcion;
        }

        public TipoInmueble(int id,string descripcion)
        {
            this.id = id;
            this.descripcion = descripcion;
        }

    }
}
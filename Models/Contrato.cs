

namespace inmobiliaria.Models
{
    public class Contrato
    {
        public int id_contrato { get; set; }
        public DateTime FechaInicio_contrato { get; set; }
        public DateTime FechaFin_contrato { get; set; }
        public int IdInmueble { get; set; }
        public int IdPropietario { get; set; }
        public Inmueble? Inmueble { get; set; }
        public Propietario? Propietario { get; set; }


        public Contrato(int id_contrato, DateTime fechaInicio_contrato, DateTime fechaFin_contrato, int idInmueble, int idPropietario)
        {
            this.id_contrato = id_contrato;
            this.FechaInicio_contrato = fechaInicio_contrato;
            this.FechaFin_contrato = fechaFin_contrato;
            this.IdInmueble = idInmueble;
            this.IdPropietario = idPropietario;
        }

    }


}


namespace inmobiliaria.Models
{
    public class Contrato
    {
        public int id_contrato { get; set; }
        public DateTime fechaInicio_contrato { get; set; }
        public DateTime fechaFin_contrato { get; set; }
        public int monto_contrato { get; set; }
        public int idInmueble { get; set; }
        public int idInquilino { get; set; }
        public Inmueble? Inmueble { get; set; }
        public Inquilino? Inquilino { get; set; }


        public Contrato(int id_contrato, DateTime fechaInicio_contrato, DateTime fechaFin_contrato, int monto_contrato, int idInmueble, int idInquilino)
        {
            this.id_contrato = id_contrato;
            this.fechaInicio_contrato = fechaInicio_contrato;
            this.fechaFin_contrato = fechaFin_contrato;
            this.idInmueble = idInmueble;
            this.idInquilino = idInquilino;
            this.monto_contrato = monto_contrato;
        }

    }


}
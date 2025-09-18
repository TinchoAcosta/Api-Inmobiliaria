

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace inmobiliaria.Models
{
    public class Contrato
    {
        public int id_contrato { get; set; }
        public DateTime fechaInicio_contrato { get; set; }
        public DateTime fechaFin_contrato { get; set; }
        public int monto_contrato { get; set; }
        public int idInmueble { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un inquilino.")]
        public int idInquilino { get; set; }
        public Inmueble? Inmueble { get; set; }
        public Inquilino? Inquilino { get; set; } 

        //preguntar al profe
        //public List<Pago> pagos_contrato { get; set; }

        public Contrato() { }

        public Contrato(int id_contrato, DateTime fechaInicio_contrato, DateTime fechaFin_contrato, int monto_contrato, int idInmueble, int idInquilino, Inmueble? Inmueble, Inquilino? Inquilino)
        {
            this.id_contrato = id_contrato;
            this.fechaInicio_contrato = fechaInicio_contrato;
            this.fechaFin_contrato = fechaFin_contrato;
            this.idInmueble = idInmueble;
            this.idInquilino = idInquilino;
            this.monto_contrato = monto_contrato;
            this.Inmueble = Inmueble;
            this.Inquilino = Inquilino;
        }

    }


}
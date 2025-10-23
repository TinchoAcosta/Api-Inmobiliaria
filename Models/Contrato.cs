

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace inmobiliaria.Models
{
    public class Contrato
    {
        [Key]
        [Column("idContrato")]
        public int id_contrato { get; set; }

        [Column("fechaInicio")]
        public DateTime fechaInicio_contrato { get; set; }

        [Column("fechaFinalizacion")]
        public DateTime fechaFin_contrato { get; set; }

        [Required(ErrorMessage = "El monto del contrato es obligatorio.")]
        [Column("montoAlquiler")]
        public int monto_contrato { get; set; }

        [ForeignKey("Inmueble")]
        [Column("idInmueble")]
        public int idInmueble { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un inquilino.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un inquilino válido.")]
        [ForeignKey("Inquilino")]
        [Column("idInquilino")]
        public int idInquilino { get; set; }

        public Inmueble? Inmueble { get; set; }
        public Inquilino? Inquilino { get; set; }

        [Column("estado")]
        public int estado { get; set; }

        [NotMapped]
        public bool anulado_contrato { get; set; } = false;
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
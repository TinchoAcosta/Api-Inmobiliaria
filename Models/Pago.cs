
namespace inmobiliaria.Models
{
    public class Pago
    {
        public int id_pago;
        public int numero_pago { get; set; }
        public String detalle_pago { get; set; }
        public DateTime fecha_de_pago { get; set; }
        public Double monto_pago { get; set; }
        public Boolean esta_anulado { get; set; }
        public int contratoId { get; set; }
        public Contrato? Contrato { get; set; }


        public Pago() { }

        public Pago(int id_pago, int numero_pago, String detalle_pago, DateTime fecha_de_pago, Double monto_pago, Boolean esta_anulado, int contratoId, Contrato contrato)
        {
            this.id_pago = id_pago;
            this.numero_pago = numero_pago;
            this.detalle_pago = detalle_pago;
            this.fecha_de_pago = fecha_de_pago;
            this.monto_pago = monto_pago;
            this.esta_anulado = esta_anulado;
            this.contratoId = contratoId;
            this.Contrato = contrato;
        }

    }
}
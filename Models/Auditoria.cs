namespace inmobiliaria.Models
{
    public class Auditoria
    {
        public int id_auditoria { get; set; }
        public string entidad { get; set; }
        public int id_entidad { get; set; }
        public string accion { get; set; }
        public DateTime fecha { get; set; }
        public int usuario_id { get; set; }
        public Usuario usuario { get; set; }

        public Auditoria(int id_auditoria, string entidad, int id_entidad, string accion, DateTime fecha, int usuario_id, Usuario usuario)
        {
            this.id_auditoria = id_auditoria;
            this.entidad = entidad;
            this.id_entidad = id_entidad;
            this.accion = accion;
            this.fecha = fecha;
            this.usuario_id = usuario_id;
            this.usuario = usuario;
        }
         
        public Auditoria() { }
    }
}
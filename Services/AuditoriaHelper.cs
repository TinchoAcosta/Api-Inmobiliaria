using inmobiliaria.Models;

namespace inmobiliaria.Services
{
    public class AuditoriaHelper
    {
        private Auditoria auditoria = new Auditoria();
        private RepositorioAuditoria repositorioAuditoria = new RepositorioAuditoria();


        public AuditoriaHelper() { }

        public void RegistrarAuditoria(string entidad, int idEntidad, string accion, int usuarioId)
        {
            auditoria.fecha = DateTime.Now;
            auditoria.entidad = entidad;
            auditoria.id_entidad = idEntidad;
            auditoria.accion = accion;
            auditoria.usuario_id = usuarioId;

            repositorioAuditoria.Registrar(auditoria);
        }
    }
}
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{
    public class RepositorioAuditoria : RepositorioBase
    {

        public IList<Auditoria> obtenerTodas()
        {

            IList<Auditoria> res = new List<Auditoria>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT a.id_auditoria, a.entidad, a.id_entidad, a.accion, a.usuario_id, a.fecha,
                              u.nombre_usuario
                       FROM auditoria a
                       INNER JOIN usuario u ON a.usuario_id = u.id_usuario
                       ORDER BY a.fecha DESC;";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var auditoria = new Auditoria(
                            reader.GetInt32("id_auditoria"),
                            reader.GetString("entidad"),
                            reader.GetInt32("id_entidad"),
                            reader.GetString("accion"),
                            reader.GetDateTime("fecha"),
                            reader.GetInt32("usuario_id"),
                            new Usuario
                            {
                                nombre_usuario = reader.GetString("nombre_usuario"),
                            }
                        );
                        res.Add(auditoria);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public int Registrar(Auditoria auditoria)
        {
            int res = -1;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO auditoria (entidad, id_entidad, accion, usuario_id, fecha)
                       VALUES (@entidad, @idEntidad, @accion, @usuarioId, @fecha);";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@entidad", auditoria.entidad);
                    command.Parameters.AddWithValue("@idEntidad", auditoria.id_entidad);
                    command.Parameters.AddWithValue("@accion", auditoria.accion);
                    command.Parameters.AddWithValue("@usuarioId", auditoria.usuario_id);
                    command.Parameters.AddWithValue("@fecha", auditoria.fecha);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return res;
        }

        public IList<Auditoria> ObtenerPorEntidad(string entidad, int idEntidad)
        {
            var lista = new List<Auditoria>();
            using var conn = new MySqlConnection(connectionString);
            string sql = @"SELECT a.id_auditoria, a.entidad, a.id_entidad, a.accion, a.usuario_id, a.fecha,
                              u.nombre_usuario, u.apellido_usuario
                       FROM auditoria a
                       INNER JOIN usuario u ON a.usuario_id = u.id_usuario
                       WHERE a.entidad = @entidad AND a.id_entidad = @idEntidad
                       ORDER BY a.fecha DESC;";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@entidad", entidad);
            cmd.Parameters.AddWithValue("@idEntidad", idEntidad);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Auditoria(
                            reader.GetInt32("id_auditoria"),
                            reader.GetString("entidad"),
                            reader.GetInt32("id_entidad"),
                            reader.GetString("accion"),
                            reader.GetDateTime("fecha"),
                            reader.GetInt32("usuario_id"),
                            new Usuario
                            {
                                nombre_usuario = reader.GetString("nombre_usuario"),
                                apellido_usuario = reader.GetString("apellido_usuario"),
                            }
                        ));
            }
            return lista;
        }
    }
}
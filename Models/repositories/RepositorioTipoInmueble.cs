using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{
    public class RepositorioTipoInmueble : RepositorioBase
    {
        public IList<TipoInmueble> obtenerTodos()
        {
            IList<TipoInmueble> res = new List<TipoInmueble>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @";";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new TipoInmueble(
                            reader.GetString("descripcion")


                        )
                        {
                            id = reader.GetInt32("id")
                        });
                    }
                    connection.Close();

                }
                return res;
            }
        }

        public int agregarTipo(TipoInmueble t)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO `tipo_inmueble`(`descripcion`) VALUES (@descripcion);";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@descripcion", t.descripcion);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public TipoInmueble obtenerTipoPorId(int id)
        {
            TipoInmueble tipo = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT `id`, `descripcion` FROM `tipo_inmueble` WHERE `id` = @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tipo = new TipoInmueble(
                            reader.GetString("descripcion")
                        )
                        {
                            id = reader.GetInt32("id")
                        };
                    }
                    connection.Close();
                }
                return tipo;
            }
        }

        public int editarTipo(TipoInmueble t)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `tipo_inmueble` SET `descripcion` = @descripcion WHERE `id` = @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", t.id);
                    command.Parameters.AddWithValue("@descripcion", t.descripcion);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public bool EstaEnUso(int idTipo)
        {
            bool enUso = false;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT COUNT(*) 
                               FROM inmueble 
                               WHERE tipo_inmueble = @idTipo;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idTipo", idTipo);
                    conn.Open();
                    var result = Convert.ToInt32(cmd.ExecuteScalar());
                    enUso = true ? result > 0 : false;
                }
            }
            return enUso;
        }

        public int borrarTipo(int id)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM `tipo_inmueble` WHERE `id` = @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
    }
}
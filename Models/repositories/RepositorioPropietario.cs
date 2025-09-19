

using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{
    public class RepositorioPropietario : RepositorioBase
    {

        public int agregarPropietario(Propietario p)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO `propietario`(`dni_propietario`, `nombre_propietario`, `apellido_propietario`, `email_propietario`, `telefono_propietario`) 
                VALUES 
                (@dni_propietario,
                @nombre_propietario,
                @apellido_propietario,
                @email_propietario,
                @telefono_propietario);";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@dni_propietario", p.dni_propietario);
                    command.Parameters.AddWithValue("@nombre_propietario", p.nombre_propietario);
                    command.Parameters.AddWithValue("@apellido_propietario", p.apellido_propietario);
                    command.Parameters.AddWithValue("@email_propietario", p.email_propietario);
                    command.Parameters.AddWithValue("@telefono_propietario", p.telefono_propietario);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int borrarPropietario(int id)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `propietario` SET `borrado_propietario`=0 WHERE `id_propietario` = @id";
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

        public int editarPropietario(Propietario p)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE propietario 
                SET 
                dni_propietario=@dni_propietario,
                nombre_propietario=@nombre_propietario,
                apellido_propietario=@apellido_propietario,
                email_propietario=@email_propietario,
                telefono_propietario=@telefono_propietario 
                WHERE id_propietario = @id_propietario;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@dni_propietario", p.dni_propietario);
                    command.Parameters.AddWithValue("@nombre_propietario", p.nombre_propietario);
                    command.Parameters.AddWithValue("@apellido_propietario", p.apellido_propietario);
                    command.Parameters.AddWithValue("@email_propietario", p.email_propietario);
                    command.Parameters.AddWithValue("@telefono_propietario", p.telefono_propietario);
                    command.Parameters.AddWithValue("@id_propietario", p.id_propietario);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
                return res;
            }
        }

        public Propietario obtenerPropietarioPorId(int id)
        {
            Propietario p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_propietario, dni_propietario, nombre_propietario, apellido_propietario, email_propietario, telefono_propietario FROM propietario WHERE id_propietario = @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Propietario(
                            reader.GetInt32("dni_propietario"),
                            reader.GetString("nombre_propietario"),
                            reader.GetString("apellido_propietario"),
                            reader.GetString("email_propietario"),
                            reader.GetString("telefono_propietario")
                        )
                        {
                            id_propietario = reader.GetInt32("id_propietario")
                        };
                    }
                    connection.Close();
                }
                return p;
            }
        }

        // ====================== LISTAR TODOS ======================
        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> res = new List<Propietario>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_propietario, dni_propietario, nombre_propietario, apellido_propietario, email_propietario, telefono_propietario
                               FROM Propietario
                               WHERE borrado_propietario=1;";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario entidad = new Propietario
                        {
                            id_propietario = reader.GetInt32("id_propietario"),
                            dni_propietario = reader.GetInt32("dni_propietario"),
                            nombre_propietario = reader["nombre_propietario"]?.ToString() ?? "",
                            apellido_propietario = reader["apellido_propietario"]?.ToString() ?? "",
                            email_propietario = reader["email_propietario"]?.ToString() ?? "",
                            telefono_propietario = reader["telefono_propietario"]?.ToString() ?? "",
                        };
                        res.Add(entidad);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}



using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{
    public class RepositorioPropietario : RepositorioBase
    {

        // ====================== ALTA ======================
        public int Alta(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"
                    INSERT INTO Propietario (dni_propietario, contrasena_propietario, nombre_propietario, apellido_propietario, email_propietario, telefono_propietario)
                    VALUES (@dni, @contrasena, @nombre, @apellido, @email, @telefono);
                    SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@dni", p.dni_propietario);
                    command.Parameters.AddWithValue("@contrasena", p.contrasena_propietario);
                    command.Parameters.AddWithValue("@nombre", p.nombre_propietario);
                    command.Parameters.AddWithValue("@apellido", p.apellido_propietario);
                    command.Parameters.AddWithValue("@email", p.email_propietario);
                    command.Parameters.AddWithValue("@telefono", p.telefono_propietario);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.id_propietario = res;
                    connection.Close();
                }
            }
            return res;
        }

        // ====================== BAJA ======================
        public int Baja(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM Propietario WHERE id_propietario=@id;";
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

        // ====================== MODIFICACION ======================
        public int Modificacion(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"
                    UPDATE Propietario 
                    SET dni_propietario=@dni, contrasena_propietario=@contrasena, nombre_propietario=@nombre,
                        apellido_propietario=@apellido, email_propietario=@email, telefono_propietario=@telefono
                    WHERE id_propietario=@id;";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@dni", p.dni_propietario);
                    command.Parameters.AddWithValue("@contrasena", p.contrasena_propietario);
                    command.Parameters.AddWithValue("@nombre", p.nombre_propietario);
                    command.Parameters.AddWithValue("@apellido", p.apellido_propietario);
                    command.Parameters.AddWithValue("@email", p.email_propietario);
                    command.Parameters.AddWithValue("@telefono", p.telefono_propietario);
                    command.Parameters.AddWithValue("@id", p.id_propietario);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        // ====================== OBTENER POR ID ======================
        public Propietario? ObtenerPorId(int id)
        {
            Propietario? entidad = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_propietario, dni_propietario, contrasena_propietario, nombre_propietario, apellido_propietario, email_propietario, telefono_propietario
                               FROM Propietario WHERE id_propietario=@id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        entidad = new Propietario
                        {
                            id_propietario = reader.GetInt32("id_propietario"),
                            dni_propietario = reader.GetInt32("dni_propietario"),
                            contrasena_propietario = reader["contrasena_propietario"]?.ToString() ?? "",
                            nombre_propietario = reader["nombre_propietario"]?.ToString() ?? "",
                            apellido_propietario = reader["apellido_propietario"]?.ToString() ?? "",
                            email_propietario = reader["email_propietario"]?.ToString() ?? "",
                            telefono_propietario = reader["telefono_propietario"]?.ToString() ?? ""
                        };
                    }
                    connection.Close();
                }
            }
            return entidad;
        }

        // ====================== LISTAR TODOS ======================
        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_propietario, dni_propietario, contrasena_propietario, nombre_propietario, apellido_propietario, email_propietario, telefono_propietario
                               FROM Propietario;";

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
                            contrasena_propietario = reader["contrasena_propietario"]?.ToString() ?? "",
                            nombre_propietario = reader["nombre_propietario"]?.ToString() ?? "",
                            apellido_propietario = reader["apellido_propietario"]?.ToString() ?? "",
                            email_propietario = reader["email_propietario"]?.ToString() ?? "",
                            telefono_propietario = reader["telefono_propietario"]?.ToString() ?? ""
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

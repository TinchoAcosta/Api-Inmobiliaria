
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{
    public class RepositorioInmueble : RepositorioBase
    {

        // =============== ABM + LISTAR TODOS LOS INMUEBLES=================

        // Alta (INSERT)
        public int Alta(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inmueble
                               (direccion_inmueble, ambientes_inmueble, superficie_inmueble, lat_inmueble, long_inmueble, PropietarioId, portada_inmueble)
                               VALUES (@direccion, @ambientes, @superficie, @latitud, @longitud, @propietario, @portada);
                               SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", i.direccion_inmueble);
                    command.Parameters.AddWithValue("@ambientes", i.ambientes_inmueble);
                    command.Parameters.AddWithValue("@superficie", i.superficie_inmueble);
                    command.Parameters.AddWithValue("@latitud", i.lat_inmueble);
                    command.Parameters.AddWithValue("@longitud", i.long_inmueble);
                    command.Parameters.AddWithValue("@propietario", i.PropietarioId);
                    command.Parameters.AddWithValue("@portada", i.portada_inmueble ?? (object)DBNull.Value);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.id_inmueble = res; // asigna el id generado al objeto
                    connection.Close();
                }
            }
            return res;
        }

        // Baja (DELETE)
        public int Baja(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM Inmueble WHERE id_inmueble = @id;";
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

        // Modificación (UPDATE)
        public int Modificacion(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inmueble 
                               SET direccion_inmueble=@direccion, ambientes_inmueble=@ambientes, superficie_inmueble=@superficie,
                                   lat_inmueble=@latitud, long_inmueble=@longitud, PropietarioId=@propietario, portada_inmueble=@portada
                               WHERE id_inmueble=@id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", i.direccion_inmueble);
                    command.Parameters.AddWithValue("@ambientes", i.ambientes_inmueble);
                    command.Parameters.AddWithValue("@superficie", i.superficie_inmueble);
                    command.Parameters.AddWithValue("@latitud", i.lat_inmueble);
                    command.Parameters.AddWithValue("@longitud", i.long_inmueble);
                    command.Parameters.AddWithValue("@propietario", i.PropietarioId);
                    command.Parameters.AddWithValue("@portada", i.portada_inmueble ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@id", i.id_inmueble);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        // Obtener por Id (SELECT ONE)
        public Inmueble? ObtenerPorId(int id)
        {
            Inmueble? entidad = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_inmueble, direccion_inmueble, ambientes_inmueble, superficie_inmueble, 
                                      lat_inmueble, long_inmueble, PropietarioId, portada_inmueble
                               FROM Inmueble WHERE id_inmueble=@id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        entidad = new Inmueble
                        {
                            id_inmueble = reader.GetInt32("id_inmueble"),
                            direccion_inmueble = reader["direccion_inmueble"] == DBNull.Value ? "" : reader.GetString("direccion_inmueble"),
                            ambientes_inmueble = reader.GetInt32("ambientes_inmueble"),
                            superficie_inmueble = reader.GetInt32("superficie_inmueble"),
                            lat_inmueble = reader.GetDecimal("lat_inmueble"),
                            long_inmueble = reader.GetDecimal("long_inmueble"),
                            PropietarioId = reader.GetInt32("PropietarioId"),
                            portada_inmueble = reader["portada_inmueble"] == DBNull.Value ? null : reader.GetString("portada_inmueble")
                        };
                    }
                    connection.Close();
                }
            }
            return entidad;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"
            SELECT i.id_inmueble, i.direccion_inmueble, i.ambientes_inmueble, i.superficie_inmueble,
                   i.lat_inmueble, i.long_inmueble, i.PropietarioId, i.portada_inmueble,
                   p.id_propietario, p.nombre_propietario, p.apellido_propietario, p.dni_propietario
            FROM Inmueble i
            LEFT JOIN Propietario p ON i.PropietarioId = p.id_propietario;";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // Crear objeto Propietario si existe
                        Propietario? propietario = null;
                        if (reader["id_propietario"] != DBNull.Value)
                        {
                            propietario = new Propietario
                            {
                                id_propietario = reader.GetInt32("id_propietario"),
                                nombre_propietario = reader["nombre_propietario"] == DBNull.Value ? "" : reader.GetString("nombre_propietario"),
                                apellido_propietario = reader["apellido_propietario"] == DBNull.Value ? "" : reader.GetString("apellido_propietario"),
                                dni_propietario = reader["dni_propietario"] == DBNull.Value ? 0 : reader.GetInt32("dni_propietario")
                            };
                        }

                        Inmueble entidad = new Inmueble
                        {
                            id_inmueble = reader.GetInt32("id_inmueble"),
                            direccion_inmueble = reader["direccion_inmueble"] == DBNull.Value ? "" : reader.GetString("direccion_inmueble"),
                            ambientes_inmueble = reader.GetInt32("ambientes_inmueble"),
                            superficie_inmueble = reader.GetInt32("superficie_inmueble"),
                            lat_inmueble = reader.GetDecimal("lat_inmueble"),
                            long_inmueble = reader.GetDecimal("long_inmueble"),
                            PropietarioId = reader.GetInt32("PropietarioId"),
                            portada_inmueble = reader["portada_inmueble"] == DBNull.Value ? null : reader.GetString("portada_inmueble"),
                            propietario_inmueble = propietario
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

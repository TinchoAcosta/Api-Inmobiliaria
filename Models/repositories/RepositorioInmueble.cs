
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{
    public class RepositorioInmueble : RepositorioBase
    {

        //Listar
        public IList<Inmueble> obtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT 
    i.id_inmueble,
    i.direccion_inmueble,
    i.ambientes_inmueble,
    i.superficie_inmueble,
    i.lat_inmueble,
    i.long_inmueble,
    i.PropietarioId,
    i.portada_inmueble,
    i.tipo_inmueble,
    i.uso_inmueble,
    i.estaActivoInmueble,
    p.nombre_propietario,
    p.id_propietario
FROM inmueble i
INNER JOIN propietario p ON i.PropietarioId = p.id_propietario
WHERE i.estaActivoInmueble = 1;
";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        res.Add(new Inmueble(
                            reader.GetString("direccion_inmueble"),
                            reader.GetInt32("ambientes_inmueble"),
                            reader.GetInt32("superficie_inmueble"),
                            reader.GetDecimal("lat_inmueble"),
                            reader.GetDecimal("long_inmueble"),
                            reader.GetInt32("PropietarioId"),
                            reader.GetString("uso_inmueble"),
                            new Propietario
                            {
                                nombre_propietario = reader.GetString("nombre_propietario")
                            }

                        )
                        {
                            id_inmueble = reader.GetInt32("id_inmueble")
                        });
                    }
                    connection.Close();

                }
                return res;
            }
        }


        //Alta falta probar jaja

        public int AgregarInmueble(Inmueble inmueble)
        {
            int res = -1;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO inmueble 
                      (direccion_inmueble, ambientes_inmueble, superficie_inmueble, lat_inmueble, long_inmueble, propietarioId, uso_inmueble) 
                      VALUES (@direccion, @ambientes, @superficie, @lat, @long, @propietarioId, @uso)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", inmueble.direccion_inmueble);
                    command.Parameters.AddWithValue("@ambientes", inmueble.ambientes_inmueble);
                    command.Parameters.AddWithValue("@superficie", inmueble.superficie_inmueble);
                    command.Parameters.AddWithValue("@lat", inmueble.lat_inmueble);
                    command.Parameters.AddWithValue("@long", inmueble.long_inmueble);
                    command.Parameters.AddWithValue("@propietarioId", inmueble.PropietarioId);
                    command.Parameters.AddWithValue("@uso", inmueble.uso_inmueble);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return res;
        }

        //Baja
        public int BorrarInmueble(int id)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `inmueble` SET `estaActivoInmueble`=0 WHERE `id_inmueble` = @id";
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


        //Modificar

        public int modificarInmueble(Inmueble inmueble)
        {

            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `inmueble` 
                SET
                `direccion_inmueble`=@direccion_inmueble,
                `ambientes_inmueble`=@ambientes_inmueble,
                `superficie_inmueble`=@superficie_inmueble,
                `lat_inmueble`=@lat_inmueble,
                `long_inmueble`=@long_inmueble,
                `uso_inmueble`=@uso_inmueble
                WHERE `id_inmueble` = @id_inmueble;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion_inmueble", inmueble.direccion_inmueble);
                    command.Parameters.AddWithValue("@ambientes_inmueble", inmueble.ambientes_inmueble);
                    command.Parameters.AddWithValue("@superficie_inmueble", inmueble.superficie_inmueble);
                    command.Parameters.AddWithValue("@lat_inmueble", inmueble.lat_inmueble);
                    command.Parameters.AddWithValue("@long_inmueble", inmueble.long_inmueble);
                    command.Parameters.AddWithValue("@uso_inmueble", inmueble.uso_inmueble);
                    command.Parameters.AddWithValue("@id_inmueble", inmueble.id_inmueble);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
                return res;
            }

        }


        // ObtenerTipoInmueble


    }
}
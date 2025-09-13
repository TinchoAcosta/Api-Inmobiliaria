using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{
    public class RepositorioContrato : RepositorioBase
    {

        public IList<Contrato> obtenerTodos()
        {

            IList<Contrato> res = new List<Contrato>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT 
            c.id_contrato,
            c.monto_contrato,
            c.fechaInicio_contrato,
            c.fechaFin_contrato,
            i.id_inquilino,
            i.nombre_inquilino,
            inm.id_inmueble,
            inm.direccion_inmueble
        FROM contrato c
        INNER JOIN inquilino i ON c.idInquilino_contrato = i.id_inquilino
        INNER JOIN inmueble inm ON c.idInmueble_contrato = inm.id_inmueble;";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var contrato = new Contrato(
                            reader.GetInt32("id_contrato"),
                            reader.GetDateTime("fechaInicio_contrato"),
                            reader.GetDateTime("fechaFin_contrato"),
                            reader.GetInt32("monto_contrato"),
                            reader.GetInt32("id_inmueble"),
                            reader.GetInt32("id_inquilino"),
                            new Inmueble
                            {
                                id_inmueble = reader.GetInt32("id_inmueble"),
                                direccion_inmueble = reader.GetString("direccion_inmueble")
                            },
                            new Inquilino
                            {
                                id_inquilino = reader.GetInt32("id_inquilino"),
                                nombre_inquilino = reader.GetString("nombre_inquilino")
                            }
                        );
                        res.Add(contrato);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public int AgregarContrato(Contrato contrato)
        {
            int res = -1;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO `contrato`(`monto_contrato`, `fechaInicio_contrato`, `fechaFin_contrato`, `idInmueble_contrato`, `idInquilino_contrato`) 
                VALUES (@monto,@fechaI,@fechaF,@idInmueble,@idInquilino);";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@monto", contrato.monto_contrato);
                    command.Parameters.AddWithValue("@fechaI", contrato.fechaInicio_contrato);
                    command.Parameters.AddWithValue("@fechaF", contrato.fechaFin_contrato);
                    command.Parameters.AddWithValue("@idInmueble", contrato.idInmueble);
                    command.Parameters.AddWithValue("@idInquilino", contrato.idInquilino);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return res;
        }

        public Contrato obtenerPorId(int id)
        {
            Contrato contrato = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT `id_contrato`, `monto_contrato`, `fechaInicio_contrato`, `fechaFin_contrato`, `idInmueble_contrato`, `idInquilino_contrato` FROM `contrato` WHERE `id_contrato` = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        contrato = new Contrato(
                            reader.GetInt32("id_contrato"),
                            reader.GetDateTime("fechaInicio_contrato"),
                            reader.GetDateTime("fechaFin_contrato"),
                            reader.GetInt32("monto_contrato"),
                            reader.GetInt32("idInmueble_contrato"),
                            reader.GetInt32("idInquilino_contrato"),
                            null,
                            null

                        );
                    }
                    connection.Close();
                }
                return contrato;
            }
        }

        public int modificarContrato(Contrato contrato)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `contrato` SET 
                `monto_contrato`=@monto,
                `fechaInicio_contrato`=@fechaI,
                `fechaFin_contrato`=@fechaF,
                `idInmueble_contrato`=@idInmueble,
                `idInquilino_contrato`=@idInquilino 
                WHERE `id_contrato` = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@monto", contrato.monto_contrato);
                    command.Parameters.AddWithValue("@fechaI", contrato.fechaInicio_contrato);
                    command.Parameters.AddWithValue("@fechaF", contrato.fechaFin_contrato);
                    command.Parameters.AddWithValue("@idInmueble", contrato.idInmueble);
                    command.Parameters.AddWithValue("@idInquilino", contrato.idInquilino);
                    command.Parameters.AddWithValue("@id", contrato.id_contrato);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
                return res;
            }

        }

        public int borrarContrato(int id)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `contrato` SET `borrado_contrato`= 0 WHERE `id_contrato` = @id";
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
        
        public IList<Contrato> obtenerContratosActivos()
        {

            IList<Contrato> res = new List<Contrato>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT 
            c.id_contrato,
            c.monto_contrato,
            c.fechaInicio_contrato,
            c.fechaFin_contrato,
            i.id_inquilino,
            i.nombre_inquilino,
            inm.id_inmueble,
            inm.direccion_inmueble
        FROM contrato c
        INNER JOIN inquilino i ON c.idInquilino_contrato = i.id_inquilino
        INNER JOIN inmueble inm ON c.idInmueble_contrato = inm.id_inmueble
        WHERE c.borrado_contrato = 0;";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var contrato = new Contrato(
                            reader.GetInt32("id_contrato"),
                            reader.GetDateTime("fechaInicio_contrato"),
                            reader.GetDateTime("fechaFin_contrato"),
                            reader.GetInt32("monto_contrato"),
                            reader.GetInt32("id_inmueble"),
                            reader.GetInt32("id_inquilino"),
                            new Inmueble
                            {
                                id_inmueble = reader.GetInt32("id_inmueble"),
                                direccion_inmueble = reader.GetString("direccion_inmueble")
                            },
                            new Inquilino
                            {
                                id_inquilino = reader.GetInt32("id_inquilino"),
                                nombre_inquilino = reader.GetString("nombre_inquilino")
                            }
                        );
                        res.Add(contrato);
                    }
                    connection.Close();
                }
            }
            return res;
        }

    }
}
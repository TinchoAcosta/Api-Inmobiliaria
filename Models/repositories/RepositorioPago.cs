
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{
    public class RepositorioPago : RepositorioBase
    {

        public List<Pago> obtenerTodos()
        {
            List<Pago> res = new List<Pago>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT `id`, `numero_pago`, `fecha_pago`, `monto_pago`, `detalle_pago`, `esta_anulado`, `contratoId` FROM `pago`";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(
                            new Pago(
                                reader.GetInt32("id"),
                                reader.GetInt32("numero_pago"),
                                reader.GetString("detalle_pago"),
                                reader.GetDateTime("fecha_pago"),
                                reader.GetDouble("monto_pago"),
                                reader.GetBoolean("esta_anulado"),
                                reader.GetInt32("contratoId")
                            )
                        );
                    }
                    connection.Close();
                }
            }
            return res;
        }


        public int agregarPago(Pago p)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO `pago`(`numero_pago`, `fecha_pago`, `monto_pago`, `detalle_pago`, `esta_anulado`, `contratoId`) 
                VALUES 
                (@numero_pago,
                @fecha_pago,
                @monto_pago,
                @detalle_pago,
                @esta_anulado,
                @contratoId)";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@numero_pago", p.numero_pago);
                    command.Parameters.AddWithValue("@fecha_pago", p.fecha_de_pago);
                    command.Parameters.AddWithValue("@monto_pago", p.monto_pago);
                    command.Parameters.AddWithValue("@detalle_pago", p.detalle_pago);
                    command.Parameters.AddWithValue("@esta_anulado", p.esta_anulado);
                    command.Parameters.AddWithValue("@contratoId", p.idContrato);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return res;

        }

        public int anularPago(int id_pago)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `pago` SET `esta_anulado`= 1 WHERE `id_pago` = @id_pago";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_pago", id_pago);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }


    }
}
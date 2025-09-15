
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
                string sql = @"SELECT 
    p.id AS id_pago,
    p.numero_pago,
    p.fecha_pago,
    p.monto_pago,
    p.detalle_pago,
    p.esta_anulado,
    p.contratoId,
    
    c.id_contrato,
    c.monto_contrato,
    c.fechaInicio_contrato,
    c.fechaFin_contrato,
    
    i.id_inquilino,
    i.nombre_inquilino,
    i.apellido_inquilino,
    
    inm.direccion_inmueble,
    inm.id_inmueble
    
FROM pago p
INNER JOIN contrato c ON p.contratoId = c.id_contrato
INNER JOIN inquilino i ON c.idInquilino_contrato = i.id_inquilino
INNER JOIN inmueble inm ON c.idInmueble_contrato = inm.id_inmueble;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(
                            new Pago(
                                reader.GetInt32("id_pago"),
                                reader.GetInt32("numero_pago"),
                                reader.GetString("detalle_pago"),
                                reader.GetDateTime("fecha_pago"),
                                reader.GetDouble("monto_pago"),
                                reader.GetBoolean("esta_anulado"),
                                reader.GetInt32("contratoId"),
                                new Contrato(
                                    reader.GetInt32("id_contrato"),
                                    reader.GetDateTime("fechaInicio_contrato"),
                                    reader.GetDateTime("fechaFin_contrato"),
                                    reader.GetInt32("monto_contrato"),
                                    reader.GetInt32("id_inmueble"),
                                    reader.GetInt32("id_inquilino"),
                                    new Inmueble(
                                        reader.GetString("direccion_inmueble"),
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        "",
                                        0,
                                        null
                                    ),
                                    new Inquilino(
                                        0,
                                        reader.GetString("nombre_inquilino"),
                                        reader.GetString("apellido_inquilino"),
                                        "",
                                        ""
                                    )
                                )
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
                    command.Parameters.AddWithValue("@contratoId", p.contratoId);
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
                string sql = @"UPDATE `pago` SET `esta_anulado`= 1 WHERE `id` = @id_pago";
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

        public List<Pago> listarPagoPorContrato(int id_contrato)
        {
            List<Pago> res = new List<Pago>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT 
    p.id AS id_pago,
    p.numero_pago,
    p.fecha_pago,
    p.monto_pago,
    p.detalle_pago,
    p.esta_anulado,
    p.contratoId,
    
    c.id_contrato,
    c.monto_contrato,
    c.fechaInicio_contrato,
    c.fechaFin_contrato,
    
    i.id_inquilino,
    i.nombre_inquilino,
    i.apellido_inquilino,
    
    inm.direccion_inmueble,
    inm.id_inmueble
    
FROM pago p
INNER JOIN contrato c ON p.contratoId = c.id_contrato
INNER JOIN inquilino i ON c.idInquilino_contrato = i.id_inquilino
INNER JOIN inmueble inm ON c.idInmueble_contrato = inm.id_inmueble 
WHERE p.contratoId = @id_contrato;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_contrato", id_contrato);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(
                            new Pago(
                                reader.GetInt32("id_pago"),
                                reader.GetInt32("numero_pago"),
                                reader.GetString("detalle_pago"),
                                reader.GetDateTime("fecha_pago"),
                                reader.GetDouble("monto_pago"),
                                reader.GetBoolean("esta_anulado"),
                                reader.GetInt32("contratoId"),
                                new Contrato(
                                    reader.GetInt32("id_contrato"),
                                    reader.GetDateTime("fechaInicio_contrato"),
                                    reader.GetDateTime("fechaFin_contrato"),
                                    reader.GetInt32("monto_contrato"),
                                    reader.GetInt32("id_inmueble"),
                                    reader.GetInt32("id_inquilino"),
                                    new Inmueble(
                                        reader.GetString("direccion_inmueble"),
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        "",
                                        0,
                                        null
                                    ),
                                    new Inquilino(
                                        0,
                                        reader.GetString("nombre_inquilino"),
                                        reader.GetString("apellido_inquilino"),
                                        "",
                                        ""
                                    )
                                )
                            )
                            );
                    }
                    connection.Close();
                }
                return res;


            }

        }


        public Pago buscarPagoPorId(int id)
        {
            Pago p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT 
    p.id AS id_pago,
    p.numero_pago,
    p.fecha_pago,
    p.monto_pago,
    p.detalle_pago,
    p.esta_anulado,
    p.contratoId,
    
    c.id_contrato,
    c.monto_contrato,
    c.fechaInicio_contrato,
    c.fechaFin_contrato,
    
    i.id_inquilino,
    i.nombre_inquilino,
    i.apellido_inquilino,
    
    inm.direccion_inmueble,
    inm.id_inmueble
    
FROM pago p
INNER JOIN contrato c ON p.contratoId = c.id_contrato
INNER JOIN inquilino i ON c.idInquilino_contrato = i.id_inquilino
INNER JOIN inmueble inm ON c.idInmueble_contrato = inm.id_inmueble
WHERE p.id = @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Pago(
                                reader.GetInt32("id_pago"),
                                reader.GetInt32("numero_pago"),
                                reader.GetString("detalle_pago"),
                                reader.GetDateTime("fecha_pago"),
                                reader.GetDouble("monto_pago"),
                                reader.GetBoolean("esta_anulado"),
                                reader.GetInt32("contratoId"),
                                new Contrato(
                                    reader.GetInt32("id_contrato"),
                                    reader.GetDateTime("fechaInicio_contrato"),
                                    reader.GetDateTime("fechaFin_contrato"),
                                    reader.GetInt32("monto_contrato"),
                                    reader.GetInt32("id_inmueble"),
                                    reader.GetInt32("id_inquilino"),
                                    new Inmueble(
                                        reader.GetString("direccion_inmueble"),
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        "",
                                        0,
                                        null
                                    ),
                                    new Inquilino(
                                        0,
                                        reader.GetString("nombre_inquilino"),
                                        reader.GetString("apellido_inquilino"),
                                        "",
                                        ""
                                    )
                                )
                            );
                    }
                    connection.Close();
                }
                return p;
            }
        }


        public int editarPago(Pago p)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `pago` 
                       SET `detalle_pago` = @detalle_pago
                       WHERE id = @id_pago";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@detalle_pago", p.detalle_pago);
                    command.Parameters.AddWithValue("@id_pago", p.id_pago);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }


    }
}
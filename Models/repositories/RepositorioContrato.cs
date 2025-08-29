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
        INNER JOIN inmueble inm ON c.idInmueble_contrato = inm.id_inmueble
        WHERE c.borrado_contrato = 1;";

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
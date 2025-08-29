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
                string sql = "SELECT `id_contrato`, `monto_contrato`, `fechaInicio_contrato`, `fechaFin_contrato`, `idInmueble_contrato`, `idInquilino_contrato` FROM `contrato` WHERE `borrado_contrato` = 1";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        res.Add(new Contrato(
                            reader.GetInt32("id_contrato"),
                            reader.GetDateTime("fechaInicio_contrato"),
                            reader.GetDateTime("fechaFin_contrato"),
                            reader.GetInt32("monto_contrato"),
                           
                        )
                        );
                    }
                    connection.Close();

                }
                return res;
            }
        }

    }
}

using System.Dynamic;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using MySql.Data.MySqlClient;


namespace inmobiliaria.Models
{
    public class RepositorioInquilino : RepositorioBase
    {

        public List<Inquilino> obtenerTodos()
        {
            var inquilinos = new List<Inquilino>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT `id_inquilino`, `nombre_inquilino`, `apellido_inquilino`, `dni_inquilino`, `telefono_inquilino`, `email_inquilino` FROM `inquilino`;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Inquilino(
                        reader.GetInt32("dni_inquilino"),
                        reader.GetString("nombre_inquilino"),
                        reader.GetString("apellido_inquilino"),
                        reader.GetString("email_inquilino"),
                        reader.GetString("telefono_inquilino")
                    )
                        {
                            id_inquilino = reader.GetInt32("id_inquilino")
                        };
                        inquilinos.Add(i);
                    }
                    connection.Close();
                }
                return inquilinos;
            }
        }


    }
}
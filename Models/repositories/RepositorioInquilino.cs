
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
                string sql = @"SELECT `id_inquilino`, `nombre_inquilino`, `apellido_inquilino`, `dni_inquilino`, `telefono_inquilino`, `email_inquilino` FROM `inquilino` WHERE borrado_inquilino = 1;";
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

        public int agregarInquilino(Inquilino i)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO `inquilino`(`nombre_inquilino`, `apellido_inquilino`, `dni_inquilino`, `telefono_inquilino`, `email_inquilino`) 
                VALUES 
                (@nombre_inquilino,
                @apellido_inquilino,
                @dni_inquilino,
                @telefono_inquilino,
                @email_inquilino)";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre_inquilino", i.nombre_inquilino);
                    command.Parameters.AddWithValue("@apellido_inquilino", i.apellido_inquilino);
                    command.Parameters.AddWithValue("@dni_inquilino", i.dni_inquilino);
                    command.Parameters.AddWithValue("@telefono_inquilino", i.telefono_inquilino);
                    command.Parameters.AddWithValue("@email_inquilino", i.email_inquilino);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
                return res;
            }
        }

        public Inquilino obtenerInquilinoPorId(int id)
        {
            Inquilino i = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT `id_inquilino`, `nombre_inquilino`, `apellido_inquilino`, `dni_inquilino`, `telefono_inquilino`, `email_inquilino` FROM `inquilino` WHERE `id_inquilino` = @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        i = new Inquilino(
                        reader.GetInt32("dni_inquilino"),
                        reader.GetString("nombre_inquilino"),
                        reader.GetString("apellido_inquilino"),
                        reader.GetString("email_inquilino"),
                        reader.GetString("telefono_inquilino")
                    )
                        {
                            id_inquilino = reader.GetInt32("id_inquilino")
                        };
                    }
                    connection.Close();
                }
                return i;
            }
        }

        public int editarInquilino(Inquilino i)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `inquilino` 
                SET 
                `nombre_inquilino`=@nombre_inquilino,
                `apellido_inquilino`=@apellido_inquilino,
                `dni_inquilino`=@dni_inquilino,
                `telefono_inquilino`=@telefono_inquilino,
                `email_inquilino`=@email_inquilino 
                WHERE `id_inquilino` = @id_inquilino;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre_inquilino", i.nombre_inquilino);
                    command.Parameters.AddWithValue("@apellido_inquilino", i.apellido_inquilino);
                    command.Parameters.AddWithValue("@dni_inquilino", i.dni_inquilino);
                    command.Parameters.AddWithValue("@telefono_inquilino", i.telefono_inquilino);
                    command.Parameters.AddWithValue("@email_inquilino", i.email_inquilino);
                    command.Parameters.AddWithValue("@id_inquilino", i.id_inquilino);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
                return res;
            }
        }

        public int borrarInquilino(int id)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `inquilino` 
                SET 
                `borrado_inquilino`= 0 
                WHERE `id_inquilino` = @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
                return res;
            }
        }

    }

}

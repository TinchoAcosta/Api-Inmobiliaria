
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{
    public class RepositorioUsuario : RepositorioBase
    {

        /* 
        CREATE TABLE `usuario` (
          `id_usuario` int(11) NOT NULL,
          `nombre_usuario` varchar(50) NOT NULL,
          `apellido_usuario` varchar(50) NOT NULL,
          `email_usuario` varchar(100) NOT NULL,
          `password_usuario` varchar(255) NOT NULL,
          `rol_usuario` varchar(20) NOT NULL,
          `avatar_usuario` varchar(255) DEFAULT NULL,
          `borrado_usuario` tinyint(1) NOT NULL DEFAULT 1
        )
         */

        public int agregarUsuario(Usuario u)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO `usuario`(`nombre_usuario`, `apellido_usuario`, `email_usuario`, `password_usuario`, `rol_usuario`, `avatar_usuario`) 
                VALUES 
                (@nombre_usuario,
                @apellido_usuario,
                @email_usuario,
                @password_usuario,
                @rol_usuario,
                @avatar_usuario);";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre_usuario", u.nombre_usuario);
                    command.Parameters.AddWithValue("@apellido_usuario", u.apellido_usuario);
                    command.Parameters.AddWithValue("@email_usuario", u.email_usuario);
                    command.Parameters.AddWithValue("@password_usuario", u.password_usuario);
                    command.Parameters.AddWithValue("@rol_usuario", u.rol_usuario);
                    command.Parameters.AddWithValue("@avatar_usuario", u.avatar_usuario);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int borrarUsuario(int id)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE `usuario` SET `borrado_usuario`=0 WHERE `id_usuario` = @id";
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

        public Usuario obtenerUsuarioPorEmail(String email)
        {
            Usuario? u = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM usuario WHERE email_usuario = @id AND borrado_usuario = 1";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", email);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            u = new Usuario
                            {
                                id_usuario = reader.GetInt32("id_usuario"),
                                nombre_usuario = reader.GetString("nombre_usuario"),
                                apellido_usuario = reader.GetString("apellido_usuario"),
                                email_usuario = reader.GetString("email_usuario"),
                                password_usuario = reader.GetString("password_usuario"),
                                rol_usuario = reader.GetString("rol_usuario"),
                                avatar_usuario = reader.IsDBNull(reader.GetOrdinal("avatar_usuario")) ? null : reader.GetString("avatar_usuario")
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return u!;
        }

        public IList<Usuario> obtenerTodos()
        {
            IList<Usuario> res = new List<Usuario>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM `usuario` WHERE borrado_usuario = 1;";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Usuario entidad = new Usuario
                        {
                            id_usuario = reader.GetInt32("id_usuario"),
                            nombre_usuario = reader.GetString("nombre_usuario"),
                            apellido_usuario = reader.GetString("apellido_usuario"),
                            email_usuario = reader.GetString("email_usuario"),
                            password_usuario = reader.GetString("password_usuario"),
                            rol_usuario = reader.GetString("rol_usuario"),
                            avatar_usuario = reader.IsDBNull(reader.GetOrdinal("avatar_usuario")) ? null : reader.GetString("avatar_usuario")
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
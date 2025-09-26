using MySql.Data.MySqlClient;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class RepositorioTipoInmueble : RepositorioBase
    {
        public RepositorioTipoInmueble(IConfiguration configuration) : base(configuration){}

        public List<TipoInmueble> ObtenerTodos()
        {
            var lista = new List<TipoInmueble>();
            using (var conn = new MySqlConnection(connectionString))
            {
                var sql = "SELECT Id, Nombre FROM TipoInmueble";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new TipoInmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                        });
                    }
                    conn.Close();
                }
            }
            return lista;
        }

        public TipoInmueble ObtenerPorId(int id)
        {
            TipoInmueble tipo = null;
            using (var conn = new MySqlConnection(connectionString))
            {
                var sql = "SELECT Id, Nombre FROM TipoInmueble WHERE Id=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        tipo = new TipoInmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                        };
                    }
                    conn.Close();
                }
            }
            return tipo;
        }

        public int Alta(TipoInmueble tipo)
        {
            int res = -1;
            using (var conn = new MySqlConnection(connectionString))
            {
                var sql = @"INSERT INTO TipoInmueble (Nombre) 
                            VALUES (@nombre);
                            SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", tipo.Nombre);
                    conn.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            return res;
        }

        public int Modificacion(TipoInmueble tipo)
        {
            int res = -1;
            using (var conn = new MySqlConnection(connectionString))
            {
                var sql = @"UPDATE TipoInmueble SET Nombre=@nombre WHERE Id=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", tipo.Nombre);
                    cmd.Parameters.AddWithValue("@id", tipo.Id);
                    conn.Open();
                    res = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (var conn = new MySqlConnection(connectionString))
            {
                var sql = @"DELETE FROM TipoInmueble WHERE Id=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    res = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
    }
}

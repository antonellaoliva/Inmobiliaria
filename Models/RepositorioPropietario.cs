using MySql.Data.MySqlClient;


namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class RepositorioPropietario : RepositorioBase
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration) { }

        public IList<Propietario> ObtenerPaginado(int pagina, int tamPagina)
        {
            var lista = new List<Propietario>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var sql = @"
                    SELECT id, dni, nombre, apellido, telefono, email
                    FROM propietario
                    ORDER BY apellido, nombre
                    LIMIT @offset, @tamPagina;
                ";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);
                    cmd.Parameters.AddWithValue("@tamPagina", tamPagina);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var prop = new Propietario
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                DNI = reader["dni"].ToString() ?? string.Empty,
                                Nombre = reader["nombre"].ToString() ?? string.Empty,
                                Apellido = reader["apellido"].ToString() ?? string.Empty,
                                Telefono = reader["telefono"] != DBNull.Value ? reader["telefono"].ToString() : null,
                                Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null
                            };
                            lista.Add(prop);
                        }
                    }
                }
            }

            return lista;
        }

        public int ContarPropietarios()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = "SELECT COUNT(*) FROM propietario;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }


        public List<Propietario> ObtenerTodos()
        {
            var lista = new List<Propietario>();
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand("SELECT id, DNI, nombre, apellido, telefono, email FROM propietario", conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Propietario
                {
                    Id = r.GetInt32("id"),
                    DNI = r.GetString("dni"),
                    Nombre = r.GetString("nombre"),
                    Apellido = r.GetString("apellido"),
                    Telefono = r.IsDBNull(r.GetOrdinal("telefono")) ? null : r.GetString("telefono"),
                    Email = r.IsDBNull(r.GetOrdinal("email")) ? null : r.GetString("email")
                });
            }
            return lista;
        }

        

        public Propietario ObtenerPorId(int id)
        {
            Propietario p = null;
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand("SELECT id, DNI, nombre, apellido, telefono, email FROM propietario WHERE id=@id LIMIT 1", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                p = new Propietario
                {
                    Id = r.GetInt32("id"),
                    DNI = r.GetString("dni"),
                    Nombre = r.GetString("nombre"),
                    Apellido = r.GetString("apellido"),
                    Telefono = r.IsDBNull(r.GetOrdinal("telefono")) ? null : r.GetString("telefono"),
                    Email = r.IsDBNull(r.GetOrdinal("email")) ? null : r.GetString("email")
                };
            }
            return p;
        }


        public bool ExisteDNI(string dni, int idActual = 0)
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var query = "SELECT COUNT(*) FROM propietario WHERE DNI = @dni";

                    if (idActual > 0)
                        query += " AND id <> @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dni", dni);
                        if (idActual > 0)
                            cmd.Parameters.AddWithValue("@id", idActual);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }


        public int Alta(Propietario p)
        {
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(@"INSERT INTO propietario (dni, nombre, apellido, telefono, email)
                                            VALUES (@dni, @nombre,@apellido,@telefono,@email);
                                            SELECT LAST_INSERT_ID();", conn);
            cmd.Parameters.AddWithValue("@dni", p.DNI);
            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@apellido", p.Apellido);
            cmd.Parameters.AddWithValue("@telefono", (object?)p.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)p.Email ?? DBNull.Value);
            conn.Open();
            p.Id = Convert.ToInt32(cmd.ExecuteScalar());
            return p.Id;

        }

        public int Modificar(Propietario p)
        {
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(@"UPDATE propietario SET DNI=@dni, nombre=@nombre, apellido=@apellido, telefono=@telefono, email=@email
                                             WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", p.Id);
            cmd.Parameters.AddWithValue("@DNI", p.DNI);
            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@apellido", p.Apellido);
            cmd.Parameters.AddWithValue("@telefono", (object?)p.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)p.Email ?? DBNull.Value);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Baja(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand("DELETE FROM propietario WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}
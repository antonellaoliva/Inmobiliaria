using MySql.Data.MySqlClient;


namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class RepositorioInquilino : RepositorioBase
    {
         public RepositorioInquilino(IConfiguration configuration) : base(configuration) { }

        public List<Inquilino> ObtenerTodos()
        {
            var lista = new List<Inquilino>();
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand("SELECT id, DNI, nombre, apellido, telefono, email FROM inquilino", conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Inquilino
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

        public Inquilino ObtenerPorId(int id)
        {
            Inquilino x = null;
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand("SELECT id, DNI, nombre, apellido, telefono, email FROM inquilino WHERE id=@id LIMIT 1", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                x = new Inquilino
                {
                    Id = r.GetInt32("id"),
                    DNI = r.GetString("dni"),
                    Nombre = r.GetString("nombre"),
                    Apellido = r.GetString("apellido"),
                    Telefono = r.IsDBNull(r.GetOrdinal("telefono")) ? null : r.GetString("telefono"),
                    Email = r.IsDBNull(r.GetOrdinal("email")) ? null : r.GetString("email")
                };
            }
            return x;
        }

        public bool ExisteDNI(string dni, int idActual = 0)
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var query = "SELECT COUNT(*) FROM inquilino WHERE DNI = @dni";

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

        public int Alta(Inquilino x)
        {
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(@"INSERT INTO inquilino (dni, nombre, apellido, telefono, email)
                                            VALUES (@dni, @nombre,@apellido,@telefono,@email);
                                            SELECT LAST_INSERT_ID();", conn);
            cmd.Parameters.AddWithValue("@dni", x.DNI);
            cmd.Parameters.AddWithValue("@nombre", x.Nombre);
            cmd.Parameters.AddWithValue("@apellido", x.Apellido);
            cmd.Parameters.AddWithValue("@telefono", (object?)x.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)x.Email ?? DBNull.Value);
            conn.Open();
            x.Id = Convert.ToInt32(cmd.ExecuteScalar());
            return x.Id;

        }

        public int Modificar(Inquilino x)
        {
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(@"UPDATE inquilino SET DNI=@dni, nombre=@nombre, apellido=@apellido, telefono=@telefono, email=@email
                                             WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", x.Id);
            cmd.Parameters.AddWithValue("@DNI", x.DNI);
            cmd.Parameters.AddWithValue("@nombre", x.Nombre);
            cmd.Parameters.AddWithValue("@apellido", x.Apellido);
            cmd.Parameters.AddWithValue("@telefono", (object?)x.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)x.Email ?? DBNull.Value);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Baja(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand("DELETE FROM inquilino WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}
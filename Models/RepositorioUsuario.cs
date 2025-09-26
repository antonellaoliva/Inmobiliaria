using MySql.Data.MySqlClient;
using INMOBILIARIA__Oliva_Perez.Models;

public class RepositorioUsuario : RepositorioBase
{

    public RepositorioUsuario(IConfiguration configuration) : base(configuration) { }

    public int Alta(Usuario u, out string error)
    {
        error = "";
        int id = 0;

        using var conn = new MySqlConnection(connectionString);
        conn.Open();

        var sqlCheck = "SELECT COUNT(*) FROM usuario WHERE email = @email";
        using (var cmdCheck = new MySqlCommand(sqlCheck, conn))
        {
            cmdCheck.Parameters.AddWithValue("@email", u.Email);
            long count = (long)cmdCheck.ExecuteScalar();
            if (count > 0)
            {
                error = "El email ya existe";
                return 0;
            }
        }

        
        var sqlInsert = @"INSERT INTO usuario (nombre, apellido, email, password, avatar, rol)
                        VALUES (@nombre, @apellido, @email, @password, @avatar, @rol);
                        SELECT LAST_INSERT_ID();";
        using var cmd = new MySqlCommand(sqlInsert, conn);
        cmd.Parameters.AddWithValue("@nombre", u.Nombre);
        cmd.Parameters.AddWithValue("@apellido", u.Apellido);
        cmd.Parameters.AddWithValue("@email", u.Email);
        cmd.Parameters.AddWithValue("@password", u.Password);
        cmd.Parameters.AddWithValue("@avatar", u.Avatar ?? "");
        cmd.Parameters.AddWithValue("@rol", u.Rol);

        id = Convert.ToInt32(cmd.ExecuteScalar());

        return id;
    }


    public int Baja(int id)
    {
        using var conn = new MySqlConnection(connectionString);
        var sql = "DELETE FROM usuario WHERE id = @id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public int Modificacion(Usuario u, out string error)
    {
        error = "";

        using var conn = new MySqlConnection(connectionString);
        conn.Open();

        var checkSql = "SELECT COUNT(*) FROM usuario WHERE email = @Email AND id != @Id";
        using var checkCmd = new MySqlCommand(checkSql, conn);
        checkCmd.Parameters.AddWithValue("@Email", u.Email);
        checkCmd.Parameters.AddWithValue("@Id", u.Id);

        var count = Convert.ToInt32(checkCmd.ExecuteScalar());
        if (count > 0)
        {
            error = "El email ya está registrado por otro usuario.";
            return 0; 
        }

        var sql = @"UPDATE usuario SET 
                        nombre = @nombre,
                        apellido = @apellido,
                        email = @email,
                        password = @password,
                        avatar = @avatar,
                        rol = @rol
                    WHERE Id = @id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nombre", u.Nombre);
        cmd.Parameters.AddWithValue("@apellido", u.Apellido);
        cmd.Parameters.AddWithValue("@email", u.Email);
        cmd.Parameters.AddWithValue("@password", u.Password);
        cmd.Parameters.AddWithValue("@avatar", u.Avatar ?? "");
        cmd.Parameters.AddWithValue("@rol", u.Rol);
        cmd.Parameters.AddWithValue("@id", u.Id);
        
        return cmd.ExecuteNonQuery();
    }

    public List<Usuario> ObtenerTodos()
    {
        var usuarios = new List<Usuario>();
        using var conn = new MySqlConnection(connectionString);
        var sql = "SELECT * FROM usuario";
        using var cmd = new MySqlCommand(sql, conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            usuarios.Add(new Usuario
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Apellido = reader["apellido"].ToString()!,
                Email = reader["email"].ToString()!,
                Password = reader["password"].ToString()!,
                Avatar = reader["avatar"].ToString()!,
                Rol = reader["rol"].ToString()!
            });
        }
        return usuarios;
    }

    public Usuario? ObtenerPorId(int id)
    {
        Usuario? u = null;
        using var conn = new MySqlConnection(connectionString);
        var sql = "SELECT * FROM usuario WHERE id = @id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            u = new Usuario
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Apellido = reader["apellido"].ToString()!,
                Email = reader["email"].ToString()!,
                Password = reader["password"].ToString()!,
                Avatar = reader["avatar"].ToString()!,
                Rol = reader["rol"].ToString()!
            };
        }
        return u;
    }

    public Usuario? ObtenerPorEmail(string email)
    {
        Usuario? u = null;
        using var conn = new MySqlConnection(connectionString);
        var sql = "SELECT * FROM usuario WHERE email = @email";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@email", email);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            u = new Usuario
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Apellido = reader["apellido"].ToString()!,
                Email = reader["email"].ToString()!,
                Password = reader["password"].ToString()!,
                Avatar = reader["avatar"].ToString()!,
                Rol = reader["rol"].ToString()!
            };
        }
        return u;
    }

        public Dictionary<int, string> ObtenerDiccionarioUsuarios()
    {
        var dict = new Dictionary<int, string>();
        foreach (var u in ObtenerTodos())
        {
            dict[u.Id] = $"{u.Nombre} {u.Apellido}";
        }
        return dict;
    }
    
}


        

       



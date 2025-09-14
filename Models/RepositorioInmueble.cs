using MySql.Data.MySqlClient;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class RepositorioInmueble : RepositorioBase
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration) { }

        public int Alta(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = @"INSERT INTO inmueble (direccion, uso, tipo, ambientes, latitud, longitud, precio, estado, propietario_id)
                            VALUES (@direccion, @uso, @tipo, @ambientes, @latitud, @longitud, @precio, @estado, @propietario_id);
                            SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@uso", i.Uso.ToString());
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@latitud", i.Latitud ?? 0m); 
                    command.Parameters.AddWithValue("@longitud", i.Longitud ?? 0m); 
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@estado", i.Estado ? 1 : 0);
                    command.Parameters.AddWithValue("@propietario_id", i.PropietarioId);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                }
            }
            return res;
        }

        public int Modificacion(Inmueble i)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = @"UPDATE inmueble SET direccion=@direccion, uso=@uso, tipo=@tipo, ambientes=@ambientes,
                            latitud=@latitud, longitud=@longitud, precio=@precio, estado=@estado, propietario_id=@propietario_id
                            WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", i.Id);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@uso", i.Uso.ToString());
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@latitud", i.Latitud ?? 0m);
                    command.Parameters.AddWithValue("@longitud", i.Longitud ?? 0m);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@estado", i.Estado ? 1 : 0);
                    command.Parameters.AddWithValue("@propietario_id", i.PropietarioId);
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int Baja(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = @"UPDATE inmueble SET estado = 0 WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble i = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = @"SELECT i.*, p.nombre, p.apellido FROM inmueble i 
                            INNER JOIN propietario p ON i.propietario_id = p.id 
                            WHERE i.id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            i = new Inmueble
                            {
                                Id = reader.GetInt32("id"),
                                Direccion = reader["direccion"]?.ToString() ?? string.Empty,
                                Uso = Enum.Parse<UsoInmueble>(reader["uso"].ToString()),
                                Tipo = reader["tipo"]?.ToString() ?? string.Empty,
                                Ambientes = reader["ambientes"] != DBNull.Value ? Convert.ToInt32(reader["ambientes"]) : 0,
                                Latitud = reader["latitud"] != DBNull.Value ? Convert.ToDecimal(reader["latitud"]) : (decimal?)null,
                                Longitud = reader["longitud"] != DBNull.Value ? Convert.ToDecimal(reader["longitud"]) : (decimal?)null,
                                Precio = reader["precio"] != DBNull.Value ? Convert.ToDecimal(reader["precio"]) : 0m,
                                Estado = reader["estado"] != DBNull.Value ? Convert.ToInt32(reader["estado"]) == 1 : false,
                                PropietarioId = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                Propietario = new Propietario
                                {
                                    Id = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                    Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                                    Apellido = reader["apellido"]?.ToString() ?? string.Empty
                                }
                            };
                        }
                    }
                }
            }
            return i;
        }

        public List<Inmueble> ObtenerTodos()
        {
            var lista = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = @"SELECT i.*, p.nombre, p.apellido FROM inmueble i
                            INNER JOIN propietario p ON i.propietario_id = p.id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Inmueble
                            {
                                Id = reader.GetInt32("id"),
                                Direccion = reader["direccion"]?.ToString() ?? string.Empty,
                                Uso = Enum.Parse<UsoInmueble>(reader["uso"].ToString()),
                                Tipo = reader["tipo"]?.ToString() ?? string.Empty,
                                Ambientes = reader["ambientes"] != DBNull.Value ? Convert.ToInt32(reader["ambientes"]) : 0,
                                Latitud = reader["latitud"] != DBNull.Value ? Convert.ToDecimal(reader["latitud"]) : (decimal?)null,
                                Longitud = reader["longitud"] != DBNull.Value ? Convert.ToDecimal(reader["longitud"]) : (decimal?)null,
                                Precio = reader["precio"] != DBNull.Value ? Convert.ToDecimal(reader["precio"]) : 0m,
                                Estado = reader["estado"] != DBNull.Value ? Convert.ToInt32(reader["estado"]) == 1 : false,
                                PropietarioId = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                Propietario = new Propietario
                                {
                                    Id = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                    Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                                    Apellido = reader["apellido"]?.ToString() ?? string.Empty
                                }
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}

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
                var sql = @"INSERT INTO inmueble (direccion, uso, ambientes, latitud, longitud, precio, estado, propietario_id, tipoInmuebleId)
                            VALUES (@direccion, @uso, @ambientes, @latitud, @longitud, @precio, @estado, @propietario_id, @tipoInmuebleId);
                            SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@uso", i.Uso.ToString());
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@latitud", i.Latitud ?? 0m);
                    command.Parameters.AddWithValue("@longitud", i.Longitud ?? 0m);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@estado", i.Estado ? 1 : 0);
                    command.Parameters.AddWithValue("@propietario_id", i.PropietarioId);
                    command.Parameters.AddWithValue("@tipoInmuebleId", i.TipoInmuebleId);
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
                var sql = @"UPDATE inmueble SET direccion=@direccion, uso=@uso, ambientes=@ambientes,
                            latitud=@latitud, longitud=@longitud, precio=@precio, estado=@estado, propietario_id=@propietario_id, tipoInmuebleId=@tipoInmuebleId
                            WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", i.Id);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@uso", i.Uso.ToString());
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@latitud", i.Latitud ?? 0m);
                    command.Parameters.AddWithValue("@longitud", i.Longitud ?? 0m);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@estado", i.Estado ? 1 : 0);
                    command.Parameters.AddWithValue("@propietario_id", i.PropietarioId);
                    command.Parameters.AddWithValue("@tipoInmuebleId", i.TipoInmuebleId);
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
                var sql = @"SELECT i.*, p.nombre AS PropietarioNombre, p.apellido AS PropietarioApellido, t.nombre AS TipoNombre
                            FROM inmueble i
                            INNER JOIN propietario p ON i.propietario_id = p.id
                            INNER JOIN tipoInmueble t ON i.tipoInmuebleId = t.id 
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
                                Ambientes = reader["ambientes"] != DBNull.Value ? Convert.ToInt32(reader["ambientes"]) : 0,
                                Latitud = reader["latitud"] != DBNull.Value ? Convert.ToDecimal(reader["latitud"]) : (decimal?)null,
                                Longitud = reader["longitud"] != DBNull.Value ? Convert.ToDecimal(reader["longitud"]) : (decimal?)null,
                                Precio = reader["precio"] != DBNull.Value ? Convert.ToDecimal(reader["precio"]) : 0m,
                                Estado = reader["estado"] != DBNull.Value ? Convert.ToInt32(reader["estado"]) == 1 : false,
                                PropietarioId = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                Propietario = new Propietario
                                {
                                    Id = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                    Nombre = reader["PropietarioNombre"]?.ToString() ?? string.Empty,
                                    Apellido = reader["PropietarioApellido"]?.ToString() ?? string.Empty
                                },
                                TipoInmuebleId = reader["tipoInmuebleId"] != DBNull.Value ? Convert.ToInt32(reader["tipoInmuebleId"]) : 0,
                                Tipo = new TipoInmueble
                                {
                                    Id = reader["tipoInmuebleId"] != DBNull.Value ? Convert.ToInt32(reader["tipoInmuebleId"]) : 0,
                                    Nombre = reader["TipoNombre"]?.ToString() ?? string.Empty
                                }
                            };
                        }
                    }
                }
            }
            return i;
        }

        public IList<Inmueble> ObtenerPaginado(int pagina, int tamPagina)
            {
                IList<Inmueble> lista = new List<Inmueble>();
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var sql = @"SELECT i.id, i.direccion, i.uso, i.ambientes, i.latitud, i.longitud, i.precio, 
                                        i.estado, i.propietario_id, i.tipoInmuebleId,
                                        p.nombre AS PropietarioNombre, 
                                        p.apellido AS PropietarioApellido,
                                        t.nombre AS TipoNombre
                                FROM inmueble i
                                INNER JOIN propietario p ON i.propietario_id = p.id
                                INNER JOIN tipoinmueble t ON i.tipoInmuebleId = t.id
                                LIMIT @offset, @tamPagina";
                    
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);
                        cmd.Parameters.AddWithValue("@tamPagina", tamPagina);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var i = new Inmueble
                                {
                                    Id = reader.GetInt32("id"),
                                    Direccion = reader["direccion"]?.ToString() ?? string.Empty,
                                    Uso = Enum.Parse<UsoInmueble>(reader["uso"].ToString()),
                                    Ambientes = reader["ambientes"] != DBNull.Value ? Convert.ToInt32(reader["ambientes"]) : 0,
                                    Latitud = reader["latitud"] != DBNull.Value ? Convert.ToDecimal(reader["latitud"]) : (decimal?)null,
                                    Longitud = reader["longitud"] != DBNull.Value ? Convert.ToDecimal(reader["longitud"]) : (decimal?)null,
                                    Precio = reader["precio"] != DBNull.Value ? Convert.ToDecimal(reader["precio"]) : 0m,
                                    Estado = reader["estado"] != DBNull.Value ? Convert.ToInt32(reader["estado"]) == 1 : false,
                                    PropietarioId = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                    Propietario = new Propietario
                                    {
                                        Id = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                        Nombre = reader["PropietarioNombre"]?.ToString() ?? string.Empty,
                                        Apellido = reader["PropietarioApellido"]?.ToString() ?? string.Empty
                                    },
                                    TipoInmuebleId = reader["tipoInmuebleId"] != DBNull.Value ? Convert.ToInt32(reader["tipoInmuebleId"]) : 0,
                                    Tipo = new TipoInmueble
                                    {
                                        Id = reader["tipoInmuebleId"] != DBNull.Value ? Convert.ToInt32(reader["tipoInmuebleId"]) : 0,
                                        Nombre = reader["TipoNombre"]?.ToString() ?? string.Empty
                                    }
                                };
                                lista.Add(i);
                            }
                        }
                    }
                }
                return lista;
            }

            public int Contar()
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var sql = "SELECT COUNT(*) FROM Inmueble";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }

        public IList<Inmueble> ObtenerDisponibles(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var lista = new List<Inmueble>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = @"
                    SELECT id, direccion, precio, ambientes, uso
                    FROM inmueble i
                    WHERE i.estado = 1
                ";

                
                if (fechaInicio.HasValue && fechaFin.HasValue)
                {
                    sql += @"
                        AND i.id NOT IN (
                            SELECT c.inmuebleId
                            FROM contrato c
                            WHERE c.estado = 'Activo'
                            AND NOT (c.fechaFin < @fechaInicio OR c.fechaInicio > @fechaFin)
                        )
                    ";
                }

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (fechaInicio.HasValue && fechaFin.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio.Value);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin.Value);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var inmueble = new Inmueble
                            {
                                Id = reader.GetInt32("id"),
                                Direccion = reader["direccion"]?.ToString() ?? string.Empty,
                                Precio = reader["precio"] != DBNull.Value ? Convert.ToDecimal(reader["precio"]) : 0m,
                                Ambientes = reader["ambientes"] != DBNull.Value ? Convert.ToInt32(reader["ambientes"]) : 0,
                                Uso = Enum.Parse<UsoInmueble>(reader["uso"].ToString())
                            };
                            lista.Add(inmueble);
                        }
                    }
                }
            }
            return lista;
        }


        public List<Inmueble> ObtenerTodos()
        {
            var lista = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = @"SELECT i.*, p.nombre AS PropietarioNombre, p.apellido AS PropietarioApellido, t.nombre AS TipoNombre
                            FROM inmueble i
                            INNER JOIN propietario p ON i.propietario_id = p.id
                            INNER JOIN tipoInmueble t ON i.tipoInmuebleId = t.id";
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
                                Ambientes = reader["ambientes"] != DBNull.Value ? Convert.ToInt32(reader["ambientes"]) : 0,
                                Latitud = reader["latitud"] != DBNull.Value ? Convert.ToDecimal(reader["latitud"]) : (decimal?)null,
                                Longitud = reader["longitud"] != DBNull.Value ? Convert.ToDecimal(reader["longitud"]) : (decimal?)null,
                                Precio = reader["precio"] != DBNull.Value ? Convert.ToDecimal(reader["precio"]) : 0m,
                                Estado = reader["estado"] != DBNull.Value ? Convert.ToInt32(reader["estado"]) == 1 : false,
                                PropietarioId = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                Propietario = new Propietario
                                {
                                    Id = reader["propietario_id"] != DBNull.Value ? Convert.ToInt32(reader["propietario_id"]) : 0,
                                    Nombre = reader["PropietarioNombre"]?.ToString() ?? string.Empty,
                                    Apellido = reader["PropietarioApellido"]?.ToString() ?? string.Empty
                                },
                                TipoInmuebleId = reader["tipoInmuebleId"] != DBNull.Value ? Convert.ToInt32(reader["tipoInmuebleId"]) : 0,
                                Tipo = new TipoInmueble
                                {
                                    Id = reader["tipoInmuebleId"] != DBNull.Value ? Convert.ToInt32(reader["tipoInmuebleId"]) : 0,
                                    Nombre = reader["TipoNombre"]?.ToString() ?? string.Empty
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

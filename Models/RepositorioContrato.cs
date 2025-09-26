using MySql.Data.MySqlClient;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class RepositorioContrato : RepositorioBase
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration) { }

        public IList<Contrato> ObtenerPaginado(int pagina, int tamPagina)
        {
            var lista = new List<Contrato>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var sql = @"
                    SELECT c.id, c.fechaInicio, c.fechaFin, c.monto,
                           c.inmuebleId, c.inquilinoId, c.estado,
                           i.direccion AS InmuebleDireccion,
                           inq.nombre AS InquilinoNombre, inq.apellido AS InquilinoApellido
                    FROM contrato c
                    INNER JOIN inmueble i ON c.inmuebleId = i.id
                    INNER JOIN inquilino inq ON c.inquilinoId = inq.id
                    ORDER BY c.fechaInicio DESC
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
                            var contrato = new Contrato
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                FechaInicio = Convert.ToDateTime(reader["fechaInicio"]),
                                FechaFin = Convert.ToDateTime(reader["fechaFin"]),
                                Monto = Convert.ToDecimal(reader["monto"]),
                                InmuebleId = Convert.ToInt32(reader["inmuebleId"]),
                                InquilinoId = Convert.ToInt32(reader["inquilinoId"]),
                                Estado = reader["estado"] != DBNull.Value
                                    ? Enum.Parse<EstadoContrato>(reader["estado"].ToString(), ignoreCase: true)
                                    : EstadoContrato.Activo,
                                Inmueble = new Inmueble
                                {
                                    Id = Convert.ToInt32(reader["inmuebleId"]),
                                    Direccion = reader["InmuebleDireccion"].ToString()
                                },
                                Inquilino = new Inquilino
                                {
                                    Id = Convert.ToInt32(reader["inquilinoId"]),
                                    Nombre = reader["InquilinoNombre"].ToString(),
                                    Apellido = reader["InquilinoApellido"].ToString()
                                }
                            };
                            lista.Add(contrato);
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
                var sql = "SELECT COUNT(*) FROM contrato;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public IList<Inmueble> ObtenerTodosInmuebles()
        {
            var repoInmueble = new RepositorioInmueble(configuration);
            return repoInmueble.ObtenerTodos();
        }

        public IList<Inquilino> ObtenerTodosInquilinos()
        {
            var repoInquilino = new RepositorioInquilino(configuration);
            return repoInquilino.ObtenerTodos();
        }

        public List<Contrato> ObtenerTodos()
        {
            var lista = new List<Contrato>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, fechaInicio, fechaFin, monto, InmuebleId, InquilinoId FROM contrato", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Contrato
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            FechaInicio = Convert.ToDateTime(reader["fechaInicio"]),
                            FechaFin = Convert.ToDateTime(reader["fechaFin"]),
                            Monto = Convert.ToDecimal(reader["monto"]),
                            InmuebleId = Convert.ToInt32(reader["InmuebleId"]),
                            InquilinoId = Convert.ToInt32(reader["InquilinoId"])
                        });
                    }
                }
            }
            return lista;
        }


        public Contrato ObtenerPorId(int id)
        {
            Contrato contrato = null;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var sql = @"
                    SELECT c.id, c.fechaInicio, c.fechaFin, c.monto, c.inmuebleId, c.inquilinoId, c.estado,
                        c.creado_por, c.creado_en, c.terminado_por, c.terminado_en,
                        i.direccion AS InmuebleDireccion,
                        inq.nombre AS InquilinoNombre, inq.apellido AS InquilinoApellido
                    FROM contrato c
                    INNER JOIN inmueble i ON c.inmuebleId = i.id
                    INNER JOIN inquilino inq ON c.inquilinoId = inq.id
                    WHERE c.id = @id;
                ";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contrato = new Contrato
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                FechaInicio = Convert.ToDateTime(reader["fechaInicio"]),
                                FechaFin = Convert.ToDateTime(reader["fechaFin"]),
                                Monto = Convert.ToDecimal(reader["monto"]),
                                InmuebleId = Convert.ToInt32(reader["inmuebleId"]),
                                InquilinoId = Convert.ToInt32(reader["inquilinoId"]),
                                Estado = reader["estado"] != DBNull.Value
                                    ? Enum.Parse<EstadoContrato>(reader["estado"].ToString(), ignoreCase: true)
                                    : EstadoContrato.Activo,
                                Inmueble = new Inmueble
                                {
                                    Id = Convert.ToInt32(reader["inmuebleId"]),
                                    Direccion = reader["InmuebleDireccion"].ToString()
                                },
                                Inquilino = new Inquilino
                                {
                                    Id = Convert.ToInt32(reader["inquilinoId"]),
                                    Nombre = reader["InquilinoNombre"].ToString(),
                                    Apellido = reader["InquilinoApellido"].ToString()
                                },
                                // Auditoría
                                CreadoPor = reader["creado_por"] != DBNull.Value ? Convert.ToInt32(reader["creado_por"]) : (int?)null,
                                CreadoEn = reader["creado_en"] != DBNull.Value ? Convert.ToDateTime(reader["creado_en"]) : (DateTime?)null,
                                TerminadoPor = reader["terminado_por"] != DBNull.Value ? Convert.ToInt32(reader["terminado_por"]) : (int?)null,
                                TerminadoEn = reader["terminado_en"] != DBNull.Value ? Convert.ToDateTime(reader["terminado_en"]) : (DateTime?)null
                            };
                        }
                    }
                }
            }

            return contrato;
        }


        public bool ExisteSuperposicion(int inmuebleId, DateTime fechaInicio, DateTime fechaFin, int? contratoIdExcluido = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var sql = @"
            SELECT COUNT(*) 
            FROM contrato
            WHERE inmuebleId = @inmuebleId
              AND estado = @estadoActivo
              AND NOT (fechaFin < @fechaInicio OR fechaInicio > @fechaFin)
        ";


                if (contratoIdExcluido.HasValue)
                {
                    sql += " AND id != @contratoIdExcluido";
                }

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@inmuebleId", inmuebleId);
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("@estadoActivo", EstadoContrato.Activo.ToString());
                    if (contratoIdExcluido.HasValue)
                        cmd.Parameters.AddWithValue("@contratoIdExcluido", contratoIdExcluido.Value);

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public void Alta(Contrato contrato, int usuarioId)
        {
            if (contrato.Monto <= 0)
                throw new Exception("El monto debe ser mayor a 0.");

            if (contrato.FechaFin < contrato.FechaInicio)
                throw new Exception("La fecha de fin no puede ser anterior a la fecha de inicio.");

            if (ExisteSuperposicion(contrato.InmuebleId, contrato.FechaInicio, contrato.FechaFin))
                throw new Exception("El inmueble ya tiene un contrato que se superpone en esas fechas.");

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = @"
                    INSERT INTO contrato (fechaInicio, fechaFin, monto, inmuebleId, inquilinoId, estado, creado_por, creado_en)
                    VALUES (@fechaInicio, @fechaFin, @monto, @inmuebleId, @inquilinoId, @estado, @creado_por, @creado_en);
                ";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                    cmd.Parameters.AddWithValue("@monto", contrato.Monto);
                    cmd.Parameters.AddWithValue("@inmuebleId", contrato.InmuebleId);
                    cmd.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
                    cmd.Parameters.AddWithValue("@estado", contrato.Estado.ToString());
                    cmd.Parameters.AddWithValue("@creado_por", usuarioId);
                    cmd.Parameters.AddWithValue("@creado_en", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void Modificacion(Contrato contrato)
        {
            if (contrato.Monto <= 0)
                throw new Exception("El monto debe ser mayor a 0.");

            if (contrato.FechaFin < contrato.FechaInicio)
                throw new Exception("La fecha de fin no puede ser anterior a la fecha de inicio.");

            if (ExisteSuperposicion(contrato.InmuebleId, contrato.FechaInicio, contrato.FechaFin, contrato.Id))
                throw new Exception("El inmueble ya tiene un contrato que se superpone en esas fechas.");

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = @"
                    UPDATE contrato 
                    SET fechaInicio=@fechaInicio, fechaFin=@fechaFin, monto=@monto,
                        inmuebleId=@inmuebleId, inquilinoId=@inquilinoId, estado=@estado, 
                        terminado_por=@terminadoPor, terminado_en=@terminadoEn
                    WHERE id=@id;
                ";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                    cmd.Parameters.AddWithValue("@monto", contrato.Monto);
                    cmd.Parameters.AddWithValue("@inmuebleId", contrato.InmuebleId);
                    cmd.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
                    cmd.Parameters.AddWithValue("@estado", contrato.Estado.ToString());
                    cmd.Parameters.AddWithValue("@terminadoPor", (object?)contrato.TerminadoPor ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@terminadoEn", (object?)contrato.TerminadoEn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", contrato.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void Baja(int id, int usuarioId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = @"UPDATE contrato
                            SET estado=@estado, terminado_por=@terminado_por, terminado_en=@terminado_en
                            WHERE id=@id;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@estado", EstadoContrato.Cancelado.ToString());
                    cmd.Parameters.AddWithValue("@terminado_por", usuarioId);
                    cmd.Parameters.AddWithValue("@terminado_en", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}







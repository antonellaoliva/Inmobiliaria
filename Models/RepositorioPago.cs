using MySql.Data.MySqlClient;


namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class RepositorioPago : RepositorioBase
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration) { }

        public List<Pago> ObtenerTodos()
        {
            var lista = new List<Pago>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM pago";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new Pago
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NumeroPago = Convert.ToInt32(reader["numeroPago"]),
                            FechaPago = Convert.ToDateTime(reader["fechaPago"]),
                            Monto = Convert.ToDecimal(reader["monto"]),
                            ContratoId = Convert.ToInt32(reader["contratoId"]),
                            FechaUpdate = Convert.ToDateTime(reader["fechaUpdate"]),
                            Estado = reader["estado"].ToString(),
                            CreadoPor = reader["creado_por"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["creado_por"]),
                            CreadoEn = reader["creado_en"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["creado_en"]),
                            AnuladoPor = reader["anulado_por"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["anulado_por"]),
                            AnuladoEn = reader["anulado_en"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["anulado_en"])
                        };

                        lista.Add(p);
                    }
                }
            }

            return lista;
        }

        public Pago ObtenerPorId(int id)
        {
            Pago p = null;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM pago WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            p = new Pago
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                NumeroPago = Convert.ToInt32(reader["numeroPago"]),
                                FechaPago = Convert.ToDateTime(reader["fechaPago"]),
                                Monto = Convert.ToDecimal(reader["monto"]),
                                ContratoId = Convert.ToInt32(reader["contratoId"]),
                                FechaUpdate = Convert.ToDateTime(reader["fechaUpdate"]),
                                Estado = reader["estado"].ToString(),

                                CreadoPor = reader["creado_por"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["creado_por"]),
                                CreadoEn = reader["creado_en"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["creado_en"]),
                                AnuladoPor = reader["anulado_por"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["anulado_por"]),
                                AnuladoEn = reader["anulado_en"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["anulado_en"])
                            };
                        }
                    }
                }
            }

            return p;
        }

        public void Crear(Pago pago)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"INSERT INTO pago (numeroPago, fechaPago, monto, contratoId, fechaUpdate, estado, creado_por, creado_en)
                              VALUES (@numeroPago, @fechaPago, @monto, @contratoId, @fechaUpdate, @estado, @creadoPor, @creadoEn)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@numeroPago", pago.NumeroPago);
                    cmd.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
                    cmd.Parameters.AddWithValue("@monto", pago.Monto);
                    cmd.Parameters.AddWithValue("@contratoId", pago.ContratoId);
                    cmd.Parameters.AddWithValue("@fechaUpdate", pago.FechaUpdate = DateTime.Now);
                    cmd.Parameters.AddWithValue("@estado", pago.Estado);
                    cmd.Parameters.AddWithValue("@creadoPor", pago.CreadoPor.HasValue ? pago.CreadoPor.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@creadoEn", pago.CreadoEn.HasValue ? pago.CreadoEn.Value : (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }



        public void Editar(Pago pago)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"UPDATE pago 
                              SET numeroPago=@numeroPago, fechaPago=@fechaPago, monto=@monto, contratoId=@contratoId, 
                                  fechaUpdate=@fechaUpdate, estado=@estado
                              WHERE id=@id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@numeroPago", pago.NumeroPago);
                    cmd.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
                    cmd.Parameters.AddWithValue("@monto", pago.Monto);
                    cmd.Parameters.AddWithValue("@contratoId", pago.ContratoId);
                    cmd.Parameters.AddWithValue("@fechaUpdate", pago.FechaUpdate = DateTime.Now);
                    cmd.Parameters.AddWithValue("@estado", pago.Estado);
                    cmd.Parameters.AddWithValue("@id", pago.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Anular(int id, int usuarioId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"UPDATE pago 
                              SET estado='anulado', anulado_por=@usuarioId, anulado_en=@fecha
                              WHERE id=@id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
       

    }

}
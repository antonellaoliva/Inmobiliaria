using MySql.Data.MySqlClient;
using INMOBILIARIA__Oliva_Perez.Models;

public class RepositorioContrato : RepositorioBase
{
    public RepositorioContrato(IConfiguration configuration) : base(configuration) { }

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
                var cmd = new MySqlCommand("SELECT id, fechaInicio, fechaFin, monto, InmuebleId, InquilinoId FROM contrato WHERE id=@id", conn);
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
                            InmuebleId = Convert.ToInt32(reader["InmuebleId"]),
                            InquilinoId = Convert.ToInt32(reader["InquilinoId"])
                        };
                    }
                }
            }
            return contrato;
        }

        public void Alta(Contrato contrato)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO contrato (fechaInicio, fechaFin, monto, InmuebleId, InquilinoId) VALUES (@fechaInicio, @fechaFin, @monto, @InmuebleId, @InquilinoId)", 
                    conn);
                cmd.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                cmd.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                cmd.Parameters.AddWithValue("@monto", contrato.Monto);
                cmd.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
                cmd.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Modificacion(Contrato contrato)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "UPDATE contrato SET fechaInicio=@fechaInicio, fechaFin=@fechaFin, monto=@monto, InmuebleId=@InmuebleId, InquilinoId=@InquilinoId WHERE id=@id", 
                    conn);
                cmd.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                cmd.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                cmd.Parameters.AddWithValue("@monto", contrato.Monto);
                cmd.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
                cmd.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
                cmd.Parameters.AddWithValue("@id", contrato.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Baja(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM contrato WHERE id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }


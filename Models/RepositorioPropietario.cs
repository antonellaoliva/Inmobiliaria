using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cms;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class RepositorioPropietario
    {
        public readonly string connectionString;
        public RepositorioPropietario(string connectionString) => this.connectionString = connectionString;

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
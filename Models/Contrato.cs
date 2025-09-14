namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Monto { get; set; }
        public int InmuebleId { get; set; }
        public int InquilinoId { get; set; }

        public Inmueble? Inmueble { get; set; }
        public Inquilino? Inquilino { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class Contrato
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un inmueble")]
        public int InmuebleId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un inquilino")]
        public int InquilinoId { get; set; }

        [Display(Name = "Inmueble")]
        public Inmueble? Inmueble { get; set; }

        [Display(Name = "Inquilino")]
        public Inquilino? Inquilino { get; set; }

        [Display(Name = "Creado por")]
        public int? CreadoPor { get; set; }

        [Display(Name = "Fecha creación")]
        public DateTime? CreadoEn { get; set; } = DateTime.Now;

        [Display(Name = "Terminado por")]
        public int? TerminadoPor { get; set; }

        [Display(Name = "Fecha de terminación")]
        public DateTime? TerminadoEn { get; set; }

        [Required]
        public EstadoContrato Estado { get; set; } = EstadoContrato.Activo;

        public string? UsuarioCreadorNombre { get; set; }

        public string? UsuarioTerminadorNombre { get; set; }
    }

    public enum EstadoContrato
    {
        Activo = 1,
        Finalizado = 2,
        Cancelado = 3,
        
    }
}
    

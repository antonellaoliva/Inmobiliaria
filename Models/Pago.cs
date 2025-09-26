using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class Pago
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El numero de pago es obligatorio")]
        [Display(Name = "Número de pago")]
        public int NumeroPago { get; set; }

        [Required(ErrorMessage = "La fecha de pago es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de pago no es valida")]
        [Display(Name = "Fecha de pago")]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "Debe tener un contrato asociado")]
        [Display(Name = "Contrato")]
        public int ContratoId { get; set; }

        [ForeignKey("ContratoId")]
        public Contrato? Contrato { get; set; }

        [Display(Name = "Última actualización")]
        public DateTime FechaUpdate { get; set; }


        [Display(Name = "Estado")]
        public string Estado { get; set; } = "activo";

        [Display(Name = "Creado por (usuario)")]
        public int? CreadoPor { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime? CreadoEn { get; set; }

        [Display(Name = "Anulado por (usuario)")]
        public int? AnuladoPor { get; set; }

        [Display(Name = "Fecha de anulación")]
        public DateTime? AnuladoEn { get; set; }
        
        public string? UsuarioCreadorNombre { get; set; }
   
        public string? UsuarioAnuladorNombre { get; set; }
    }
}

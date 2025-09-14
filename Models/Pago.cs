using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class Pago
    {

        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Número de pago")]
        public int NumeroPago { get; set; }

        [Required]
        [Display(Name = "Fecha de pago")]
        public DateTime FechaPago { get; set; }

        [Required]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Required]
        [Display(Name = "Contrato")]
        public int ContratoId { get; set; }

        [ForeignKey("ContratoId")]
        public required Contrato Contrato { get; set; }

        [Display(Name = "Última actualización")]
        public DateTime? FechaUpdate { get; set; }
    }
}
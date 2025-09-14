using System.ComponentModel.DataAnnotations;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class Inmueble
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "El uso es obligatorio.")]
        [Display(Name = "Uso")]
        public UsoInmueble Uso { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio.")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe indicar la cantidad de ambientes.")]
        [Range(1, 20, ErrorMessage = "La cantidad de ambientes debe ser entre 1 y 20.")]
        [Display(Name = "Ambientes")]
        public int Ambientes { get; set; }

        [Required(ErrorMessage = "La latitud es obligatoria.")]
        [Display(Name = "Latitud")]
        public decimal? Latitud { get; set; }

        [Required(ErrorMessage = "La longitud es obligatoria.")]
        [Display(Name = "Longitud")]
        public decimal? Longitud { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, 99999999.99, ErrorMessage = "El precio debe ser un valor positivo.")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Display(Name = "Disponible")]
        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "Debe seleccionar un propietario.")]
        [Display(Name = "Propietario")]
        public int PropietarioId { get; set; } 

        public Propietario? Propietario { get; set; }
    }

    public enum UsoInmueble
    {
        Residencial = 0,
        Comercial = 1,

        Otro = 2
    }
}

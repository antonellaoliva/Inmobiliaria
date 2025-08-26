using System.ComponentModel.DataAnnotations;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class Propietario
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string DNI { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }


    }
}


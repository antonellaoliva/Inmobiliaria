using System.ComponentModel.DataAnnotations;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class Propietario
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El dni es obligatorio"), StringLength(20)]
        public string DNI { get; set; } = string.Empty;

        [Required(ErrorMessage ="El nombre el obligatorio"), StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage ="El apellido el obligatorio"), StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage ="El email no tiene formato valido"), StringLength(100)]
        public string? Email { get; set; }


    }
}


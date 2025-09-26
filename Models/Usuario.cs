using System.ComponentModel.DataAnnotations;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class Usuario
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Avatar { get; set; } = "/Uploads/default-avatar.png";

        [Required]
        public string Rol { get; set; } = "Empleado";

        public IFormFile? AvatarFile { get; set; }

        public string AvatarUrl => string.IsNullOrWhiteSpace(Avatar) ? "/Uploads/default-avatar.png" : Avatar;
    }
}
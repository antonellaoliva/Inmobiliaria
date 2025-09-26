using System.ComponentModel.DataAnnotations;

namespace INMOBILIARIA__Oliva_Perez.Models
{

    public class TipoInmueble
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de inmueble es obligatorio")]
        public string Nombre { get; set; }
    }
}
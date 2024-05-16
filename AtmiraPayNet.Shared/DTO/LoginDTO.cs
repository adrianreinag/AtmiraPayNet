using System.ComponentModel.DataAnnotations;

namespace AtmiraPayNet.Shared.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder los 50 caracteres")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Password { get; set; }
    }
}

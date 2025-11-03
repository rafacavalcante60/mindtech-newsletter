using System.ComponentModel.DataAnnotations;

namespace mindtechNewsletter.Server.DTOs
{
    public class SubscriberCreateDTO
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        [MaxLength(255, ErrorMessage = "O email não pode ter mais de 255 caracteres.")]
        public string Email { get; set; } = string.Empty;
    }
}

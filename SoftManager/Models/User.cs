using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SoftManager.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome completo não pode exceder 100 caracteres.")]
        [Display(Name = "Nome Completo")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date, ErrorMessage = "A data de nascimento deve ser uma data válida.")]
        [Display(Name = "Data de Nascimento")]
        public DateTime BirthDate { get; set; }

        [Phone(ErrorMessage = "O número de telefone fixo deve ser válido.")]
        [Display(Name = "Telefone")]
        public string? PhoneNumber { get; set; }

        [Phone(ErrorMessage = "O número de celular deve ser válido.")]
        [Display(Name = "Celular")]
        public string? MobileNumber { get; set; }

        [StringLength(200, ErrorMessage = "O endereço não pode exceder 200 caracteres.")]
        [Display(Name = "Endereço")]
        public string? Address { get; set; }

        [DataType(DataType.ImageUrl, ErrorMessage = "A foto de perfil deve ser uma URL de imagem válida.")]
        [Display(Name = "Foto")]
        public string? ProfilePicture { get; set; }

        [Required(ErrorMessage = "A identificação do gestor é obrigatória.")]
        [Display(Name = "Gestor")]
        public bool IsManager { get; set; }
    }
}

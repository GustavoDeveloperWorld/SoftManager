using System.ComponentModel.DataAnnotations;

namespace SoftManager.Models
{
    public class UserTask
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Mensagem")]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Vencimento")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Usuário Atribuído")]
        public string AssignedToUserId { get; set; }
        [Display(Name = "Usuário Atribuído")]
        public User AssignedToUser { get; set; }
    }
}

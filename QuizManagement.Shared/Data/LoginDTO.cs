using System.ComponentModel.DataAnnotations;

namespace QuizManagement.Shared.Data
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;

        public int? QuizId { get; set; } // Optional for admin
    }
}

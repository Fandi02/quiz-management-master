using System.ComponentModel.DataAnnotations;

namespace QuizManagement.Shared.Data
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;

        [Required]
        public int QuizId { get; set; } = default!;
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Data
{
    public class PasswordDTO
    {
        [Required]
        public string OldPassword { get; set; } = default!;

        [Required]
        public string NewPassword { get; set; } = default!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace QuizManagement.Shared.Data
{
    public class QuizDTO
    {
        [Required]
        public string Name { get; set; } = default!;
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Data
{
    public class QuestionDTO
    {
        public int? Id { get; set; }

        [Required]
        public string Question { get; set; } = default!;

        public List<AnswerDTO>? Answers { get; set; }

        public int? QuizId { get; set; }
    }
}

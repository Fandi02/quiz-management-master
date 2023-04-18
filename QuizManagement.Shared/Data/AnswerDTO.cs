using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Data
{
    public class AnswerDTO
    {
        public int? Id { get; set; }

        [Required]
        public string Answer { get; set; } = default!;

        [Required]
        public bool Value { get; set; } = default!;
    }
}

using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Models
{
    public class Answer : BaseEntity
    {
        public int Id { get; set; } = default!;

        public string Text { get; set; } = default!;
        public bool Value { get; set; }

        [JsonIgnore]
        public int QuestionId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Question Question { get; set; } = null!;
    }
}

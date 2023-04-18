using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Models
{
    public class Question : BaseEntity
    {
        public int Id { get; set; } = default!;

        public string Text { get; set; } = default!;

        [JsonIgnore]
        public int QuizId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Quiz Quiz { get; set; } = null!;

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}

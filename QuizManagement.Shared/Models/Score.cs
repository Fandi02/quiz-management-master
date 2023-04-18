using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Models
{
    public class Score : BaseEntity
    {
        public int Id { get; set; }

        [JsonIgnore]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        [JsonIgnore]
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        [JsonIgnore]
        public int AnswerId { get; set; }
        public Answer Answer { get; set; } = null!;

        [JsonIgnore]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}

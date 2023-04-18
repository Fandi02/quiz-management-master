using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Models
{
    public class Quiz : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Question> Questions { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<User> Users { get; set; } = null!;
    }
}

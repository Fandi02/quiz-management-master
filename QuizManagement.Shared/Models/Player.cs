using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Models
{
    public class Player : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        [JsonIgnore]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [JsonIgnore]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
    }
}

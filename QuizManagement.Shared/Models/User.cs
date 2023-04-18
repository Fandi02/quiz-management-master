using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Models
{
    public enum Role
    {
        Admin = 1,
        User = 2,
    }

    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public Role Role { get; set; }

        [JsonIgnore]
        public string Password { get; set; } = default!;

        [JsonIgnore]
        public int? QuizId { get; set; }
        public Quiz? Quiz { get; set; }
    }
}

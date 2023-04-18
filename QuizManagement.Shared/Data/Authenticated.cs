namespace QuizManagement.Shared.Data
{
    public class Authenticated
    {
        public int UserID { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string Token { get; set; } = default!;
        public int? QuizId { get; set; } = default!;
    }
}

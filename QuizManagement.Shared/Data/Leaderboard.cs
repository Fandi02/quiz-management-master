namespace QuizManagement.Shared.Data
{
    public class Leaderboard
    {
        public int UserId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Score { get; set; } = default!;
    }
}

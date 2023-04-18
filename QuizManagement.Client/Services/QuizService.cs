using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;

namespace QuizManagement.Client.Services
{
    public interface IQuizService
    {
        Task<List<Quiz>> GetQuizzes();
        Task<Quiz> GetQuiz(int id);
        Task DeleteQuiz(int id);
        Task<Quiz> AddQuiz(Quiz quiz);
        Task UpdateQuiz(Quiz quiz);
        Task<List<Leaderboard>> GetLeaderboard(int id);
        Task<List<User>> GetPlayers(int id);
        Task PrintPlayers(int id);
    }

    public class QuizService : IQuizService
    {
        private IHttpService _httpService;

        public QuizService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Quiz>> GetQuizzes()
        {
            return await _httpService.Get<List<Quiz>>("/api/quiz");
        }

        public async Task<Quiz> GetQuiz(int id)
        {
            return await _httpService.Get<Quiz>($"api/quiz/{id}");
        }

        public async Task DeleteQuiz(int id)
        {
            await _httpService.Delete($"api/quiz/{id}");
        }

        public async Task<Quiz> AddQuiz(Quiz quiz)
        {
            return await _httpService.Post<Quiz>($"api/quiz", quiz);
        }

        public async Task UpdateQuiz(Quiz quiz)
        {
            await _httpService.Put($"api/quiz/{quiz.Id}", new QuizDTO
            {
                Name = quiz.Name
            });
        }

        public async Task<List<Leaderboard>> GetLeaderboard(int id)
        {
            return await _httpService.Get<List<Leaderboard>>($"api/quiz/{id}/leaderboard");
        }

        public async Task<List<User>> GetPlayers(int id)
        {
            return await _httpService.Get<List<User>>($"api/quiz/{id}/player");
        }
        public async Task PrintPlayers(int id)
        {
            // _httpService.
            var awe = await _httpService.Get<string>($"api/quiz/{id}/player/csv");
            Console.WriteLine(awe);
        }
    }
}
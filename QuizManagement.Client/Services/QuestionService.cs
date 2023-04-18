using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;

namespace QuizManagement.Client.Services
{
    public interface IQuestionService
    {
        Task<PagedResult<Question>> GetQuestions(int page);
        Task<Question> GetQuestion(int id);
        Task DeleteQuestion(int id);
        Task<Question> AddQuestion(QuestionDTO question);
        Task UpdateQuestion(QuestionDTO question);
    }

    public class QuestionService : IQuestionService
    {
        private IHttpService _httpService;

        public QuestionService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<PagedResult<Question>> GetQuestions(int page)
        {
            return await _httpService.Get<PagedResult<Question>>("/api/question");
        }

        public async Task<Question> GetQuestion(int id)
        {
            return await _httpService.Get<Question>($"api/question/{id}");
        }

        public async Task DeleteQuestion(int id)
        {
            await _httpService.Delete($"api/question/{id}");
        }

        public async Task<Question> AddQuestion(QuestionDTO question)
        {
            return await _httpService.Post<Question>($"api/question", question);
        }

        public async Task UpdateQuestion(QuestionDTO question)
        {
            await _httpService.Put($"api/question/{question.Id}", question);
        }
    }
}
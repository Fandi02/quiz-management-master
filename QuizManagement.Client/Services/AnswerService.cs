using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;

namespace QuizManagement.Client.Services
{
  public interface IAnswerService
  {
    Task<PagedResult<Answer>> GetAnswers(int page);
    Task<Answer> GetAnswer(int id);
    Task DeleteAnswer(int id);
    Task AddAnswer(Answer answer);
    Task UpdateAnswer(Answer answer);
  }

  public class AnswerService : IAnswerService
  {
    private IHttpService _httpService;

    public AnswerService(IHttpService httpService)
    {
      _httpService = httpService;
    }

    public async Task<PagedResult<Answer>> GetAnswers(int page)
    {
      return await _httpService.Get<PagedResult<Answer>>("/api/answer");
    }

    public async Task<Answer> GetAnswer(int id)
    {
      return await _httpService.Get<Answer>($"api/answer/{id}");
    }

    public async Task DeleteAnswer(int id)
    {
      await _httpService.Delete($"api/answer/{id}");
    }

    public async Task AddAnswer(Answer answer)
    {
      await _httpService.Post($"api/answer", answer);
    }

    public async Task UpdateAnswer(Answer answer)
    {
      await _httpService.Put($"api/answer", answer);
    }
  }
}
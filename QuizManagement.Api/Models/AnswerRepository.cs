using Microsoft.EntityFrameworkCore;
using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;

namespace QuizManagement.Api.Models
{
    public interface IAnswerRepository
    {
        PagedResult<Answer> GetAnswers(Pagination query);
        Task<Answer> GetAnswer(int id);
        Task<Answer> AddAnswer(Answer answer);
        Task<Answer> UpdateAnswer(Answer answer);
        Task<Answer> DeleteAnswer(int id);
    }

    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _appDbContext;

        public AnswerRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Answer> AddAnswer(Answer answer)
        {
            var result = await _appDbContext.Answers.AddAsync(answer);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Answer> DeleteAnswer(int id)
        {
            var result = await this.GetAnswer(id);

            _appDbContext.Answers.Remove(result);
            await _appDbContext.SaveChangesAsync();

            return result;
        }

        public async Task<Answer> GetAnswer(int id)
        {
            var result = await _appDbContext.Answers.FirstOrDefaultAsync(answer => answer.Id == id);
            if (result == null) throw new KeyNotFoundException("Answer not found");

            return result!;
        }

        public PagedResult<Answer> GetAnswers(Pagination query)
        {
            var builder = _appDbContext.Answers;

            return builder
              .OrderBy(i => i.CreatedAt)
              .GetPaged(query.Page, query.Limit);
        }

        public async Task<Answer> UpdateAnswer(Answer answer)
        {
            var result = await this.GetAnswer(answer.Id);

            _appDbContext.Entry(result!).CurrentValues.SetValues(answer);
            await _appDbContext.SaveChangesAsync();

            return result;
        }
    }
}

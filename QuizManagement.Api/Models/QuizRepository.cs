using Microsoft.EntityFrameworkCore;
using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;

namespace QuizManagement.Api.Models
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetQuizzes();
        Task<Quiz> GetQuiz(int id);
        Task<Quiz> AddQuiz(Quiz quiz);
        Task<Quiz> UpdateQuiz(Quiz quiz);
        Task<Quiz> DeleteQuiz(int id);
    }

    public class QuizRepository : IQuizRepository
    {
        private readonly AppDbContext _appDbContext;

        public QuizRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Quiz> AddQuiz(Quiz quiz)
        {
            var result = await _appDbContext.Quizzes.AddAsync(quiz);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Quiz> DeleteQuiz(int id)
        {
            var result = await this.GetQuiz(id);

            _appDbContext.Quizzes.Remove(result);
            await _appDbContext.SaveChangesAsync();

            return result;
        }

        public async Task<Quiz> GetQuiz(int id)
        {
            var result = await _appDbContext.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(quiz => quiz.Id == id);
            if (result == null) throw new KeyNotFoundException("Quiz not found");

            return result!;
        }

        public async Task<List<Quiz>> GetQuizzes()
        {
            return await _appDbContext.Quizzes.OrderBy(i => i.CreatedAt).ToListAsync();
        }

        public async Task<Quiz> UpdateQuiz(Quiz quiz)
        {
            var result = await _appDbContext.Quizzes.FirstOrDefaultAsync(q => q.Id == quiz.Id);
            if (result == null) throw new KeyNotFoundException("Quiz not found");

            result.Name = quiz.Name;
            _appDbContext.Update(result);
            await _appDbContext.SaveChangesAsync();

            return result;
        }
    }
}

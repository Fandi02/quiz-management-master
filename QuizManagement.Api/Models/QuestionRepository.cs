using Microsoft.EntityFrameworkCore;
using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;

namespace QuizManagement.Api.Models
{
    public interface IQuestionRepository
    {
        PagedResult<Question> GetQuestions(Pagination query);
        Task<Question> GetQuestion(int id);
        Task<Question> AddQuestion(Question question);
        Task<Question> UpdateQuestion(Question question);
        Task<Question> DeleteQuestion(int id);
    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _appDbContext;

        public QuestionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Question> AddQuestion(Question question)
        {
            var result = await _appDbContext.Questions.AddAsync(question);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Question> DeleteQuestion(int id)
        {
            var result = await this.GetQuestion(id);

            _appDbContext.Questions.Remove(result);
            await _appDbContext.SaveChangesAsync();

            return result;
        }

        public async Task<Question> GetQuestion(int id)
        {
            var result = await _appDbContext.Questions
              .Include(q => q.Answers)
              .FirstOrDefaultAsync(q => q.Id == id);
            if (result == null) throw new KeyNotFoundException("Question not found");

            return result!;
        }

        public PagedResult<Question> GetQuestions(Pagination query)
        {
            var builder = _appDbContext.Questions.Include(q => q.Answers);

            return builder
              .OrderBy(i => i.CreatedAt)
              .GetPaged(query.Page, query.Limit);
        }

        public async Task<Question> UpdateQuestion(Question question)
        {
            var result = await this.GetQuestion(question.Id);

            _appDbContext.Entry(result!).CurrentValues.SetValues(new Question
            {
                Id = question.Id,
                Text = question.Text,
                UpdatedAt = DateTime.UtcNow,
                QuizId = result.QuizId,
            });

            // Delete children
            foreach (var answer in result.Answers.ToList())
            {
                if (!question.Answers.Any(c => c.Id == answer.Id))
                {
                    _appDbContext.Answers.Remove(answer);
                }
            }

            // Update and Insert children
            foreach (var answer in question.Answers)
            {
                var existingChild = result.Answers
                    .Where(c => c.Id == answer.Id && c.Id != default(int))
                    .SingleOrDefault();

                if (existingChild != null)
                    // Update child
                    _appDbContext.Entry(existingChild).CurrentValues.SetValues(answer);
                else
                {
                    result.Answers.Add(answer);
                }
            }

            await _appDbContext.SaveChangesAsync();

            return result;
        }
    }
}

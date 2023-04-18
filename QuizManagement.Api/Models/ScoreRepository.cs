using Microsoft.EntityFrameworkCore;
using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;

namespace QuizManagement.Api.Models
{
    public interface IScoreRepository
    {
        PagedResult<Score> GetScores(Pagination query);
        Task<Score> GetScore(int id);
        Task<Score> AddScore(Score score);
        Task<Score> UpdateScore(Score score);
        Task<Score> DeleteScore(int id);
        Task<List<Leaderboard>> GetLeaderboard(int quizID);
    }

    public class ScoreRepository : IScoreRepository
    {
        private readonly AppDbContext _appDbContext;

        public ScoreRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Score> AddScore(Score score)
        {
            var result = await _appDbContext.Scores.AddAsync(score);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Score> DeleteScore(int id)
        {
            var result = await this.GetScore(id);

            _appDbContext.Scores.Remove(result);
            await _appDbContext.SaveChangesAsync();

            return result;
        }

        public async Task<Score> GetScore(int id)
        {
            var result = await _appDbContext.Scores.FirstOrDefaultAsync(score => score.Id == id);
            if (result == null) throw new KeyNotFoundException("Score not found");

            return result!;
        }

        public PagedResult<Score> GetScores(Pagination query)
        {
            var builder = _appDbContext.Scores;

            return builder
              .OrderBy(i => i.CreatedAt)
              .GetPaged(query.Page, query.Limit);
        }

        public async Task<Score> UpdateScore(Score score)
        {
            var result = await this.GetScore(score.Id);

            _appDbContext.Entry(result!).CurrentValues.SetValues(score);
            await _appDbContext.SaveChangesAsync();

            return result;
        }

        public async Task<List<Leaderboard>> GetLeaderboard(int quizID)
        {
            var db = _appDbContext;

            var results = await (
                from s in db.Scores

                join u in db.Users
                on s.UserId equals u.Id

                join a in db.Answers
                on s.AnswerId equals a.Id

                where s.QuizId == quizID && a.Value == true

                select new
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Score = a.Value
                } into result

                group result by result.UserId into g

                select new
                {
                    UserId = g.Key,
                    Name = g.Select(e => e.Name),
                    Score = g.Count() * 10
                }).OrderByDescending(s => s.Score).Take(10).ToListAsync();

            var leaderboards = new List<Leaderboard>();

            foreach (var value in results)
            {
                leaderboards.Add(new Leaderboard
                {
                    Name = value.Name.First(),
                    Score = value.Score,
                    UserId = value.UserId,
                });
            }

            return leaderboards;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using QuizManagement.Shared.Models;
using QuizManagement.Shared.Data;

namespace QuizManagement.Api.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Score> Scores => Set<Score>();
        // public DbSet<Leaderboard> Leaderboards => Set<Leaderboard>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            User user = new User
            {
                Id = 1,
                Name = "Dwa Meizadewa",
                Username = "infamous0192",
                Email = "infamous0192@gmail.com",
                Role = Role.Admin,
                Password = BCrypt.Net.BCrypt.HashPassword("asdqwe123"),
            };

            modelBuilder.Entity<User>().HasData(user);
        }
    }
}
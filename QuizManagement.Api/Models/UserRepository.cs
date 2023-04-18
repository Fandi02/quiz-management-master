using Microsoft.EntityFrameworkCore;
using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;
using QuizManagement.Api.Helpers;
using QuizManagement.Api.Authorization;

namespace QuizManagement.Api.Models
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
        Task<List<User>> GetByQuiz(int quizId);
        Task<User> GetUser(int id);
        Task<User> AddUser(User data);
        Task<User> UpdateUser(User data);
        Task<User> DeleteUser(int id);
        Task<Authenticated> Login(LoginDTO data);
        Task<Authenticated> Register(RegisterDTO data);
        Task<User> ChangePassword(int id, PasswordDTO data);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        private IJwtUtils _jwtUtils;

        public UserRepository(AppDbContext appDbContext, IJwtUtils jwtUtils)
        {
            _appDbContext = appDbContext;
            _jwtUtils = jwtUtils;
        }

        public async Task<User> AddUser(User user)
        {
            if (_appDbContext.Users.Where(u => u.Username == user.Username).Count() > 0)
            {
                throw new KeyNotFoundException("Username was used");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var result = await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User> DeleteUser(int id)
        {
            var result = await this.GetUser(id);

            _appDbContext.Users.Remove(result);
            await _appDbContext.SaveChangesAsync();

            return result;
        }

        public async Task<User> GetUser(int id)
        {
            var result = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (result == null) throw new KeyNotFoundException("User not found");

            return result!;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _appDbContext.Users.Where(u => u.Role == Role.Admin).ToListAsync();

            return users;
        }

        public async Task<List<User>> GetByQuiz(int quizId)
        {
            var users = await _appDbContext.Users.Where(u => u.QuizId == quizId).ToListAsync();

            return users;
        }

        public async Task<User> UpdateUser(User data)
        {
            var user = await this.GetUser(data.Id);

            user.Name = data.Name;
            user.Role = data.Role;
            user.Username = data.Username;

            if (data.Password != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword((string)data.Password);
            }

            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<Authenticated> Register(RegisterDTO data)
        {
            if (await this.isDuplicate(data.Username, data.QuizId))
            {
                throw new AppException("Username was used");
            }

            var user = new User
            {
                Name = data.Name,
                Username = data.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(data.Password),
                Email = data.Email,
                Role = Role.User,
                QuizId = data.QuizId,
            };

            var result = await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            var creds = new Authenticated
            {
                UserID = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role == Role.Admin ? "admin" : "user",
                Token = _jwtUtils.GenerateToken(user),
                QuizId = user.QuizId
            };

            return creds;
        }

        public async Task<Authenticated> Login(LoginDTO data)
        {
            var user = await _appDbContext.Users
                .Where(u => u.Username == data.Username && u.QuizId == data.QuizId)
                .FirstOrDefaultAsync();

            if (user == null) throw new KeyNotFoundException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(data.Password, user.Password))
            {
                throw new AppException("Password is incorrect");
            }

            var creds = new Authenticated
            {
                UserID = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role == Role.Admin ? "admin" : "user",
                Token = _jwtUtils.GenerateToken(user)
            };

            if (user.Role == Role.User)
            {
                creds.QuizId = user.QuizId;
            }

            return creds;
        }

        private async Task<bool> isDuplicate(string username, int quizId)
        {
            var count = await _appDbContext.Users
                .Where(u => u.Username == username && u.Role == Role.User && u.QuizId == quizId)
                .CountAsync();

            return count > 0;
        }

        public async Task<User> ChangePassword(int id, PasswordDTO data)
        {
            var user = await _appDbContext.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null) throw new KeyNotFoundException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(data.OldPassword, user.Password))
            {
                throw new AppException("Password is incorrect");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword((string)data.NewPassword);

            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }
    }
}

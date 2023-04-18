using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace QuizManagement.Client.Services
{
    public interface IUserService
    {
        Authenticated Creds { get; set; }
        Task Initialize();
        Task Login(LoginDTO model);
        Task Logout();
        Task<List<User>> GetUsers();
        Task<User> GetUser(int id);
        Task DeleteUser(int id);
        Task AddUser(UserDTO user);
        Task UpdateUser(User user);
    }

    public class UserService : IUserService
    {
        private IHttpService _httpService;
        private ILocalStorageService _localStorageService;
        private NavigationManager _navigationManager;
        private string _userKey = "creds";

        public Authenticated Creds { get; set; }

        public UserService(IHttpService httpService, ILocalStorageService localStorageService, NavigationManager navigationManager)
        {
            _httpService = httpService;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
        }

        public async Task Initialize()
        {
            Creds = await _localStorageService.GetItem<Authenticated>(_userKey);
        }

        public async Task Login(LoginDTO model)
        {
            Creds = await _httpService.Post<Authenticated>("/api/auth/login", model);
            await _localStorageService.SetItem(_userKey, Creds);
        }

        public async Task Logout()
        {
            Creds = null;
            await _localStorageService.RemoveItem(_userKey);
            _navigationManager.NavigateTo("/login");
        }

        public async Task<List<User>> GetUsers()
        {
            return await _httpService.Get<List<User>>("/api/user");
        }

        public async Task<User> GetUser(int id)
        {
            return await _httpService.Get<User>($"api/user/{id}");
        }

        public async Task DeleteUser(int id)
        {
            await _httpService.Delete($"api/user/{id}");
            // auto logout if the user deleted their own record
            if (id == Creds?.UserID)
                await Logout();
        }

        public async Task AddUser(UserDTO user)
        {
            await _httpService.Post($"api/user", user);
        }

        public async Task UpdateUser(User user)
        {
            await _httpService.Put($"api/user/{user.Id}", user);
            // update local storage if the user updated their own record
            // if (user.Id == Creds?.UserID)
            // {
            //     Creds.Name = user.Name;
            //     Creds.Username = user.Username;
            //     // Creds.Role = user.Role;
            //     await _localStorageService.SetItem(_userKey, Creds);
            // }
        }
    }
}
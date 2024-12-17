using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdvertisingAds.Core.Models;
using Newtonsoft.Json;

namespace Advertisung.Repository
{
    public class UserRepository
    {
        private readonly string _filePath;

        public UserRepository()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "users.json");
        }

        private List<User> GetAllUsers()
        {
            if (!File.Exists(_filePath))
            {
                return new List<User>();
            }

            var jsonData = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();
        }

        public async Task SaveUser(User user)
        {
            var users = GetAllUsers();
            user.Id = Guid.NewGuid().ToString();
            users.Add(user);
            var jsonData = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, jsonData);
        }

        public async Task<bool> SignUp(User user)
        {
            var users = GetAllUsers();
            if (users.Any(u => u.Email == user.Email))
            {
                return false; // User already exists
            }

            await SaveUser(user);
            return true; // User registered successfully
        }

        public async Task<User> SignIn(User user)
        {
            var currentUser = GetAllUsers().FirstOrDefault(u => u.password == user.password && u.Email == user.Email);
            return currentUser; // Returns the user if found, otherwise null
        }
    }
}

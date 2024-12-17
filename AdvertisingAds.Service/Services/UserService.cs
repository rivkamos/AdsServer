using AdvertisingAds.Core.Interfaces;
using AdvertisingAds.Core.Models;
using Advertisung.Repository;
using System.Threading.Tasks;

namespace AdvertisingAds.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Signup(User user)
        {
            return await _userRepository.SignUp(user);
        }

        public async Task<User> Signin(User user)
        {
            return await _userRepository.SignIn(user);
        }
    }
}

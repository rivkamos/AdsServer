using AdvertisingAds.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingAds.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> Signup(User user);
        Task<User> Signin(User user);
    }
}

using AdvertisingAds.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingAds.Core.Interfaces
{
    public interface IAdService
    {
        Task<List<Ad>> GetAllAdsAsync();
        Task<Ad> GetAdByIdAsync(int id);
        Task SaveAdAsync(List<Ad> ads);
        Task<bool> DeleteAdAsync(int id);
        Task<bool> UpdateAdAsync(Ad ad);
        Task<string> SaveImageAsync(IFormFile file);
    }
}

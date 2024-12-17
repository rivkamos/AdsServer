using AdvertisingAds.Core.Interfaces;
using AdvertisingAds.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdvertisingAds.Service.Services
{
    public class AdService : IAdService
    {
        private readonly string _filePath;
        public AdService() => _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ads.json");

        public async Task<List<Ad>> GetAllAdsAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Ad>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<Ad>>(json) ?? new List<Ad>();
        }

        public async Task SaveAdAsync(List<Ad> ads)
        {
            var json = JsonConvert.SerializeObject(ads, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<Ad> GetAdByIdAsync(int id)
        {
            var ads = await GetAllAdsAsync();
            return ads.FirstOrDefault(ad => ad.Id == id);
        }

        public async Task<bool> UpdateAdAsync(Ad updatedAd)
        {
            var ads = await GetAllAdsAsync();
            var existingAd = ads.FirstOrDefault(ad => ad.Id == updatedAd.Id);
            if (existingAd != null)
            {
                existingAd.Title = updatedAd.Title;
                existingAd.Description = updatedAd.Description;
                existingAd.Location = updatedAd.Location;
                await SaveAdAsync(ads);
                return true; // Indicate successful update
            }
            return false; // Indicate update failure
        }

        public async Task<bool> DeleteAdAsync(int id)
        {
            var ads = await GetAllAdsAsync();
            var adToRemove = ads.FirstOrDefault(ad => ad.Id == id);
            if (adToRemove != null)
            {
                ads.Remove(adToRemove);
                await SaveAdAsync(ads);
                return true; // Indicate successful deletion
            }
            return false; // Indicate deletion failure
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images"); // Adjust path as necessary
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"Images/{uniqueFileName}"; // Return the relative path to be stored in the database
        }
    }
}

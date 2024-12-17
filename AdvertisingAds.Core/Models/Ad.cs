using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingAds.Core.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreatorId { get; set; }
        public Location Location { get; set; }
        public AdType Type { get; set; } 
        public string Description { get; set; }
        public string ImageAdUrl { get; set; }
        public string ImageAdvertiserUrl { get; set; }
        public DateTime PostedDate { get; set; }
        public int Likes { get; set; }
        public IFormFile AdFile { get; set; }
        public IFormFile AdvertiserFile { get; set; }

        public Ad()
        {
            PostedDate = DateTime.UtcNow; 
            Likes = 0; 
        }
    }
}

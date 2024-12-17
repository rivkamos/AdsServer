using AdvertisingAds.Core.Interfaces;
using AdvertisingAds.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdvertisingAds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdService _adService;

        public AdController(IAdService adService)
        {
            _adService = adService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ad>>> Get()
        {
            var ads = await _adService.GetAllAdsAsync();
            return Ok(ads);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ad>> Get(int id)
        {
            var ad = await _adService.GetAdByIdAsync(id);
            if (ad == null)
            {
                return NotFound();
            }
            return Ok(ad);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromForm] Ad newAd)
        {
            newAd.CreatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (newAd.AdFile?.Length != 0)
            {
                var imagePath = await _adService.SaveImageAsync(newAd.AdFile);
                if (string.IsNullOrEmpty(imagePath))
                {
                    return StatusCode(500, "Could not save ad image.");
                }

                newAd.ImageAdUrl = imagePath;
            }

            if (newAd.AdvertiserFile?.Length != 0)
            {
                var imagePath = await _adService.SaveImageAsync(newAd.AdvertiserFile);
                if (string.IsNullOrEmpty(imagePath))
                {
                    return StatusCode(500, "Could not save advertiser image.");
                }

                newAd.ImageAdvertiserUrl = imagePath;
            }

            await _adService.SaveAdAsync(new List<Ad> { newAd });
            return CreatedAtAction(nameof(Get), new { id = newAd.Id }, newAd);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Ad updatedAd)
        {
            var existingAd = await _adService.GetAdByIdAsync(id);
            if (existingAd == null || existingAd.CreatorId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            {
                return Forbid(); // User is not allowed to update this ad
            }

            updatedAd.Id = id; // Ensure the ID is set for the update
            await _adService.UpdateAdAsync(updatedAd);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var existingAd = await _adService.GetAdByIdAsync(id);
            if (existingAd == null || existingAd.CreatorId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            {
                return Forbid(); // User is not allowed to delete this ad
            }

            await _adService.DeleteAdAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikeAd(int id)
        {
            var ad = await _adService.GetAdByIdAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            ad.Likes++; // Increment likes
            await _adService.UpdateAdAsync(ad); // Save updated ad back to storage

            return Ok(ad); // Return the updated ad
        }
    }
}

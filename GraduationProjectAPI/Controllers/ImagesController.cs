using GraduationProjectAPI.Dtos;
using GraduationProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public ImagesController(EducationPlatformContext context)
        {
            _context = context;
        }

        //Create New ImageForContent
        [HttpPost]
        public async Task<IActionResult> CreateImage([FromForm] CreateImageDto dto)
        {

            var IsValidContent = await _context.Contants.AnyAsync(c => c.Id == dto.ContantIdfk);
            if (!IsValidContent)
            {
                return BadRequest("Invalid Content ID");
            }

            if (dto.Path == null)
            {
                return BadRequest("Path Can't Empty");
            }


            ContantImage img = new ContantImage()
            {
                ContantIdfk = dto.ContantIdfk,
                Path = dto.Path,
            };
            await _context.AddAsync(img);
            _context.SaveChanges();
            return Ok(img);
        }

        //Update Specific Image By its Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(int id, [FromForm] CreateImageDto dto)
        {
            var image = await _context.ContantImages.FindAsync(id);
            if (image == null)
            {
                return NotFound("Invalid Image ID");
            }

            var IsValidContent = await _context.Contants.AnyAsync(c => c.Id == dto.ContantIdfk);
            if (!IsValidContent)
            {
                return BadRequest("Invalid Content ID");
            }

            if (dto.Path == null)
            {
                return BadRequest("Path Can't Empty");
            }

            if (dto == null)
            {
                return BadRequest("Can't add null Content");
            }
            else
            {

                image.ContantIdfk = dto.ContantIdfk;
                image.Path = dto.Path;


                _context.SaveChanges();
                return Ok(image);
            }
        }

        //Get All Images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var Images = await _context.ContantImages.Include(m => m.ContantIdfkNavigation)
                .Select(m => new ImagesDetailsDto
                {
                    Id = m.Id,
                    Path = m.Path,
                    ContantTitle = m.ContantIdfkNavigation.Title
                }).OrderByDescending(m => m.ContantTitle)
                .ToArrayAsync();
            return Ok(Images);
        }

        //Delete Image
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImageByID(int id)
        {
            var image = await _context.ContantImages.FindAsync(id);
            if (image == null)
            {
                return BadRequest("There is no Image with this ID");
            }
            else
            {
                _context.ContantImages.Remove(image);
                await _context.SaveChangesAsync();
                return Ok(image);
            }
        }




    }
}

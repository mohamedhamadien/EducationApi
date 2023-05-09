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
        private readonly EducationPlatform_GraduationProjectContext _context;

        public ImagesController(EducationPlatform_GraduationProjectContext context)
        {
            _context = context;
        }

        //Create New ImageForContent
        [HttpPost]
        public async Task<IActionResult> CreateImage([FromForm] CreateImageDto dto)
        {

            var IsValidContent = await _context.Contents.AnyAsync(c => c.Id == dto.ContantIdfk);
            if (!IsValidContent)
            {
                return BadRequest("Invalid Content ID");
            }

            if (dto.Path == null)
            {
                return BadRequest("Path Can't Empty");
            }


            ContentImage img = new ContentImage()
            {
                ContentId = dto.ContantIdfk,
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
            var image = await _context.ContentImages.FindAsync(id);
            if (image == null)
            {
                return NotFound("Invalid Image ID");
            }

            var IsValidContent = await _context.Contents.AnyAsync(c => c.Id == dto.ContantIdfk);
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

                image.ContentId = dto.ContantIdfk;
                image.Path = dto.Path;
                _context.SaveChanges();
                return Ok(image);
            }
        }

        //Get All Images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var Images = await _context.ContentImages.Include(m => m.Content)
                .Select(m => new ImagesDetailsDto
                {
                    Id = m.Id,
                    Path = m.Path,
                    ContantTitle = m.Content.Title
                }).OrderByDescending(m => m.ContantTitle)
                .ToArrayAsync();
            return Ok(Images);
        }

        //Delete Image
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImageByID(int id)
        {
            var image = await _context.ContentImages.FindAsync(id);
            if (image == null)
            {
                return BadRequest("There is no Image with this ID");
            }
            else
            {
                _context.ContentImages.Remove(image);
                await _context.SaveChangesAsync();
                return Ok(image);
            }
        }
    }
}
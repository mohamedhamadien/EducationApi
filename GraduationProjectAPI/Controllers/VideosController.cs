using GraduationProjectAPI.Dtos;
using GraduationProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly EducationPlatform_GraduationProjectContext _context;

        public VideosController(EducationPlatform_GraduationProjectContext context)
        {
            _context = context;
        }

        //Create New VideoForContent
        [HttpPost]
        public async Task<IActionResult> CreateVideo([FromForm] CreateImageDto dto)
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


            ContentVideo video = new ContentVideo()
            {
                ContentId = dto.ContantIdfk,
                Path = dto.Path,
            };
            await _context.AddAsync(video);
            _context.SaveChanges();
            return Ok(video);
        }

        //Update Specific video By its Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideo(int id, [FromForm] CreateVideoDto dto)
        {
            var video = await _context.ContentVideos.FindAsync(id);
            if (video == null)
            {
                return NotFound("Invalid Video ID");
            }

            var IsValidContent = await _context.Contents.AnyAsync(c => c.Id == dto.ContantIdfk);
            if (!IsValidContent)
            {
                return BadRequest("Invalid Content ID");
            }

            if (dto.Path == null)
            {
                return BadRequest("Path Of Video Can't Empty");
            }

            if (dto == null)
            {
                return BadRequest("Can't add null Content");
            }
            else
            {

                video.ContentId = dto.ContantIdfk;
                video.Path = dto.Path;


                _context.SaveChanges();
                return Ok(video);
            }
        }

        //Get All Videos
        [HttpGet]
        public async Task<IActionResult> GetAllVideos()
        {
            var Videos = await _context.ContentVideos.Include(m => m.Content)
                .Select(m => new VideoDetailsDto
                {
                    Id = m.Id,
                    Path = m.Path,
                    ContantTitle = m.Content.Title
                }).OrderByDescending(m => m.ContantTitle)
                .ToArrayAsync();
            return Ok(Videos);
        }

        //Delete Video By Its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoByID(int id)
        {
            var video = await _context.ContentVideos.FindAsync(id);
            if (video == null)
            {
                return BadRequest("There is no Video with this ID");
            }
            else
            {
                _context.ContentVideos.Remove(video);
                await _context.SaveChangesAsync();
                return Ok(video);
            }
        }
    }
}
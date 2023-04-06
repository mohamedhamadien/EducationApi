using GraduationProjectAPI.Dtos;
using GraduationProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFsController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public PDFsController(EducationPlatformContext context)
        {
            _context = context;
        }

        //Create New PDF For Content
        [HttpPost]
        public async Task<IActionResult> CreatePdf([FromForm] CreatePDFDto dto)
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


            ContantPdf file = new ContantPdf()
            {
                ContantIdfk = dto.ContantIdfk,
                Path = dto.Path,
            };
            await _context.AddAsync(file);
            _context.SaveChanges();
            return Ok(file);
        }

        //Update Specific PDF By its Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePDF(int id, [FromForm] CreatePDFDto dto)
        {
            var file = await _context.ContantPdfs.FindAsync(id);
            if (file == null)
            {
                return NotFound("Invalid PDF ID");
            }

            var IsValidContent = await _context.Contants.AnyAsync(c => c.Id == dto.ContantIdfk);
            if (!IsValidContent)
            {
                return BadRequest("Invalid Content ID");
            }

            if (dto.Path == null)
            {
                return BadRequest("Path Of PDF Can't be Empty");
            }

            if (dto == null)
            {
                return BadRequest("Can't add null Content");
            }
            else
            {

                file.ContantIdfk = dto.ContantIdfk;
                file.Path = dto.Path;


                _context.SaveChanges();
                return Ok(file);
            }
        }

        //Get All PDFs
        [HttpGet]
        public async Task<IActionResult> GetAllPDFs()
        {
            var files = await _context.ContantPdfs.Include(m => m.ContantIdfkNavigation)
                .Select(m => new PDFDetailsDto
                {
                    Id = m.Id,
                    Path = m.Path,
                    ContantTitle = m.ContantIdfkNavigation.Title
                }).OrderByDescending(m => m.ContantTitle)
                .ToArrayAsync();
            return Ok(files);
        }

        //Delete PDF By Its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePDFByID(int id)
        {
            var file = await _context.ContantPdfs.FindAsync(id);
            if (file == null)
            {
                return BadRequest("There is no pdf with this ID");
            }
            else
            {
                _context.ContantPdfs.Remove(file);
                await _context.SaveChangesAsync();
                return Ok(file);
            }
        }


    }

}


using GraduationProjectAPI.Dtos;
using GraduationProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public ContentsController(EducationPlatformContext context)
        {
            _context = context;
        }

        //Create New Content
        [HttpPost]
        public async Task<IActionResult> CreateContent([FromForm] CreateContentDto dto)
        {

            var IsValidClass = await _context.Classes.AnyAsync(c => c.ClassId == dto.ClassIdfk);
            if (!IsValidClass)
            {
                return BadRequest("Invalid Class ID");
            }

            Contant content = new Contant()
            {
                ClassIdfk = dto.ClassIdfk,
                Title = dto.Title,
                Date = dto.Date,
            };
            await _context.AddAsync(content);
            _context.SaveChanges();

            foreach (var item in dto.Images)
            {

                ContantImage contImage = new ContantImage()
                {
                    ContantIdfk = content.Id,
                    Path = item,
                };
                await _context.ContantImages.AddAsync(contImage);
                _context.SaveChanges();
            }

            foreach (var item in dto.Pdfs)
            {

                ContantPdf contentPdf = new ContantPdf()
                {
                    ContantIdfk = content.Id,
                    Path = item,
                };
                _context.ContantPdfs.Add(contentPdf);
                _context.SaveChanges();
            }

            foreach (var item in dto.Videos)
            {

                ContantVideo contentVideo = new ContantVideo()
                {
                    ContantIdfk = content.Id,
                    Path = item,
                };
                _context.ContantVideos.Add(contentVideo);
                _context.SaveChanges();
            }

            return Ok("Added Successfully");
        }


        //Get All Contents
        [HttpGet]
        public async Task<IActionResult> GetAllContents()
        {
            var contents = await _context.Contants.Include(m => m.ClassIdfkNavigation)
                .Select(m => new ContentDetailsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Date = m.Date,
                    ClassName = m.ClassIdfkNavigation.Title,
                    ClassIdfk = m.ClassIdfk,
                }).OrderByDescending(m => m.Date)
                .ToArrayAsync();
            return Ok(contents);
        }


        //Get All Contents And Its (Images, pdf, and videos)
        [HttpGet("All")]
        public async Task<IActionResult> GetAllContentsWithComponents()
        {
            var contents = await _context.Contants.Include(m => m.ClassIdfkNavigation).Include(m => m.ContantImages)
                .Select(m => new ContentDetailsWithComponentsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Date = m.Date,
                    ClassName = m.ClassIdfkNavigation.Title,
                    ClassIdfk = m.ClassIdfk,
                    Images = _context.ContantImages.Where(c => c.ContantIdfk == m.Id).Select(c => c.Path).ToArray(),
                    Pdfs = _context.ContantPdfs.Where(c => c.ContantIdfk == m.Id).Select(c => c.Path).ToArray(),
                    Videos = _context.ContantVideos.Where(c => c.ContantIdfk == m.Id).Select(c => c.Path).ToArray(),
                }).OrderByDescending(m => m.Date)
                .ToArrayAsync();
            return Ok(contents);
        }

        //Get Content And Its (Images, pdf, and videos)
        [HttpGet("AllDetails/{id}")]
        public async Task<IActionResult> GetContentWithComponents(int id)
        {
            //.Include(m => m.ClassIdfkNavigation)
            var content = await _context.Contants.Include(m => m.ContantImages)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (content == null)
            {
                return BadRequest("Invalid Content ID");
            }

            var dto = new ContentDetailsWithComponentsDto
            {
                Id = content.Id,
                Title = content.Title,
                Date = content.Date,
                //ClassName = content.ClassIdfkNavigation.Title,
                ClassIdfk = content.ClassIdfk,
                Images = _context.ContantImages.Where(c => c.ContantIdfk == content.Id).Select(c => c.Path).ToArray(),
                Pdfs = _context.ContantPdfs.Where(c => c.ContantIdfk == content.Id).Select(c => c.Path).ToArray(),
                Videos = _context.ContantVideos.Where(c => c.ContantIdfk == content.Id).Select(c => c.Path).ToArray(),
            };

            //.Select(m => new ContentDetailsWithComponentsDto
            //{
            //    Id = m.Id,
            //    Title = m.Title,
            //    Date = m.Date,
            //    ClassName = m.ClassIdfkNavigation.Title,
            //    ClassIdfk = m.ClassIdfk,
            //    Images = _context.ContantImages.Where(c => c.ContantIdfk == m.Id).Select(c => c.Path).ToArray(),
            //    Pdfs = _context.ContantPdfs.Where(c => c.ContantIdfk == m.Id).Select(c => c.Path).ToArray(),
            //    Videos = _context.ContantVideos.Where(c => c.ContantIdfk == m.Id).Select(c => c.Path).ToArray(),
            //});
            return Ok(content);
        }




        //Get Specific Content By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            var cont = await _context.Contants.Include(m => m.ClassIdfkNavigation).SingleOrDefaultAsync(m => m.Id == id);
            if (cont == null)
            {
                return BadRequest("Invalid Content ID");
            }

            var dto = new ContentDetailsDto
            {
                Id = cont.Id,
                Title = cont.Title,
                Date = cont.Date,
                ClassName = cont.ClassIdfkNavigation.Title,
                ClassIdfk = cont.ClassIdfk,
            };

            return Ok(dto);
        }


        //Get All Contents OF specific Class Using Class ID 
        [HttpGet("Class/{id}")]
        public async Task<IActionResult> GetContentsByClassId(int id)
        {
            var conts = await _context.Contants.Include(m => m.ClassIdfkNavigation).Where(m => m.ClassIdfk == id).ToListAsync();
            if (conts.Count == 0)
            {
                return BadRequest("No Contents For this Class");
            }

            var dtos = new List<ContentDetailsDto>();

            foreach (var cont in conts)
            {
                ContentDetailsDto dto = new ContentDetailsDto
                {
                    Id = cont.Id,
                    Title = cont.Title,
                    Date = cont.Date,
                    ClassName = cont.ClassIdfkNavigation.Title,
                    ClassIdfk = cont.ClassIdfk,
                };
                dtos.Add(dto);
            }


            return Ok(dtos.OrderBy(d => d.Date));
        }


        //Update Specific Content By its Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(int id, [FromForm] CreateContentDto dto)
        {
            var content = await _context.Contants.FindAsync(id);
            if (content == null)
            {
                return NotFound("Invalid Content ID");
            }


            var IsValidClass = await _context.Classes.AnyAsync(c => c.ClassId == dto.ClassIdfk);
            if (!IsValidClass)
            {
                return BadRequest("Invalid Class ID");
            }

            if (dto == null)
            {
                return BadRequest("Can't add null Content");
            }
            else
            {
                content.ClassIdfk = dto.ClassIdfk;
                content.Title = dto.Title;
                content.Date = dto.Date;

                _context.SaveChanges();
                return Ok(content);
            }
        }


        //Delete Specific Content By its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContentByID(int id)
        {
            var content = await _context.Contants.FindAsync(id);
            if (content == null)
            {
                return BadRequest("There is no Content with this ID");
            }
            else
            {
                _context.Contants.Remove(content);
                await _context.SaveChangesAsync();
                return Ok(content);
            }
        }

        //Get All Contents Due To Specific Month
        [HttpGet("Month/{number}")]
        public async Task<IActionResult> GetContentsByMonthId(int number)
        {
            var conts = await _context.Contants.Include(m => m.ClassIdfkNavigation).Where(m => m.Date.Month == number).ToListAsync();
            if (conts.Count == 0)
            {
                return BadRequest("No Contents For this Month");
            }

            var dtos = new List<ContentDetailsDto>();

            foreach (var cont in conts)
            {
                ContentDetailsDto dto = new ContentDetailsDto
                {
                    Id = cont.Id,
                    Title = cont.Title,
                    Date = cont.Date,
                    ClassName = cont.ClassIdfkNavigation.Title,
                    ClassIdfk = cont.ClassIdfk,
                };
                dtos.Add(dto);
            }


            return Ok(dtos.OrderBy(d => d.Date));
        }



    }
}

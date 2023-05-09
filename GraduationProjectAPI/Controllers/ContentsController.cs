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
        #region Load
        private readonly EducationPlatform_GraduationProjectContext _context;

        public ContentsController(EducationPlatform_GraduationProjectContext context)
        {
            _context = context;
        }
        #endregion

       
        #region Create New Content
        [HttpPost("CreateNewContent")]
        public async Task<IActionResult> CreateContent(CreateContentDto dto)
        {

            var IsValidClass = await _context.Classes.AnyAsync(c => c.ClassId == dto.ClassIdfk);
            if (!IsValidClass)
            {
                return BadRequest("Invalid Class ID");
            }

            Content content = new Content()
            {
                ClassId = dto.ClassIdfk,
                Title = dto.Title,
                Date = dto.Date,
            };
            await _context.AddAsync(content);
            _context.SaveChanges();

            if (dto.Images != null)
            {
                foreach (var item in dto.Images)
                {
                    ContentImage contImage = new ContentImage()
                    {
                        ContentId = content.Id,
                        Path = item,
                    };
                    await _context.ContentImages.AddAsync(contImage);
                    _context.SaveChanges();
                }
            }

            if (dto.Pdfs != null)
            {
                foreach (var item in dto.Pdfs)
                {

                    ContentPdf contentPdf = new ContentPdf()
                    {
                        ContentId = content.Id,
                        Path = item,
                    };
                    _context.ContentPdfs.Add(contentPdf);
                    _context.SaveChanges();
                }
            }

            if (dto.Videos != null)
            {
                foreach (var item in dto.Videos)
                {

                    ContentVideo contentVideo = new ContentVideo()
                    {
                        ContentId = content.Id,
                        Path = item,
                    };
                    _context.ContentVideos.Add(contentVideo);
                    _context.SaveChanges();
                }
            }

            return Ok("Added Successfully");
        }
        #endregion
        
        
        #region AllContentWithoutDetails
        [HttpGet("AllContentWithoutDetails")]
        public async Task<IActionResult> GetAllContents()
        {
            var contents = await _context.Contents.Include(m => m.Class)
                .Select(m => new ContentDetailsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Date = m.Date,
                    ClassName = m.Class.Title,
                    ClassIdfk = m.ClassId,
                }).OrderByDescending(m => m.Date)
                .ToArrayAsync();
            return Ok(contents);
        }
        #endregion

                
        #region AllContentWithDetails
        [HttpGet("AllContentWithDetails")]
        public async Task<IActionResult> GetAllContentsWithComponents()
        {
            var contents = await _context.Contents.Include(m => m.Class).Include(m => m.ContentImages)
                .Select(m => new ContentDetailsWithComponentsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Date = m.Date,
                    ClassName = m.Class.Title,
                    ClassIdfk = m.ClassId,
                    Images = _context.ContentImages.Where(c => c.ContentId == m.Id).Select(c => c.Path).ToArray(),
                    Pdfs = _context.ContentPdfs.Where(c => c.ContentId == m.Id).Select(c => c.Path).ToArray(),
                    Videos = _context.ContentVideos.Where(c => c.ContentId == m.Id).Select(c => c.Path).ToArray(),
                }).OrderByDescending(m => m.Date)
                .ToArrayAsync();
            return Ok(contents);
        }
        #endregion

        
        #region ContentByIDWithAllDetails
        [HttpGet("ContentByIDWithAllDetails/{id:int}")]
        public async Task<IActionResult> GetContentWithComponents(int id)
        {
            //.Include(m => m.ClassIdfkNavigation)
            var content = await _context.Contents.Include(m => m.ContentImages).Include(v => v.ContentVideos).Include(p => p.ContentPdfs).Include(f => f.Class).SingleOrDefaultAsync(c => c.Id == id);

            if (content == null)
            {
                return BadRequest("Invalid Content ID");
            }

            var dto = new ContentDetailsWithComponentsDto();
            dto.Id = content.Id;
            dto.Title = content.Title;
            dto.Date = content.Date;
            dto.ClassName = content.Class.Title;
            dto.ClassIdfk = content.ClassId;
            dto.Images = _context.ContentImages.Where(c => c.ContentId == id).Select(c => c.Path).ToArray();
            dto.Pdfs = _context.ContentPdfs.Where(c => c.ContentId == id).Select(c => c.Path).ToArray();
            dto.Videos = _context.ContentVideos.Where(c => c.ContentId == id).Select(c => c.Path).ToArray();

            return Ok(dto);
        }
        #endregion

        
        #region ContentByItsIDWithoutDetails
        [HttpGet("ContentByItsIDWithoutDetails/{id:int}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            var cont = await _context.Contents.Include(m => m.Class).SingleOrDefaultAsync(m => m.Id == id);
            if (cont == null)
            {
                return BadRequest("Invalid Content ID");
            }

            var dto = new ContentDetailsDto
            {
                Id = cont.Id,
                Title = cont.Title,
                Date = cont.Date,
                ClassName = cont.Class.Title,
                ClassIdfk = cont.ClassId,
            };

            return Ok(dto);
        }
        #endregion

       
        #region ContentByClassID
        [HttpGet("ContentByClassID/{id:int}")]
        public async Task<IActionResult> GetContentsByClassId(int id)
        {

            var conts = await _context.Contents.Include(m => m.Class).Where(m => m.ClassId == id).ToListAsync();
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
                    ClassName = cont.Class.Title,
                    ClassIdfk = cont.ClassId,
                };
                dtos.Add(dto);
            }
            return Ok(dtos.OrderBy(d => d.Date));
        }
        #endregion
        

        #region UpdateContentByit'sId
        [HttpPut("UpdateContent/{id:int}")]
        public async Task<IActionResult> UpdateContent(int id, CreateContentDto dto)
        {
            var content = await _context.Contents.Include(c => c.Class).Include(p => p.ContentPdfs).Include(x => x.ContentVideos).Include(a => a.ContentImages).FirstOrDefaultAsync(h => h.Id == id);
            if (content == null)
            {
                return NotFound("Invalid Content ID");
            }

            if (dto == null)
            {
                return BadRequest("Can't add null Content");
            }
            else
            {
                content.ClassId = content.ClassId;
                content.Title = dto.Title;
                content.Date = dto.Date;

                _context.SaveChanges();

                #region Update Imgs
                var imgs = await _context.ContentImages.Where(x => x.ContentId == id).ToListAsync();
                if (imgs != null)
                {
                    foreach (var item in imgs)
                    {
                        _context.ContentImages.Remove(item);
                        _context.SaveChanges();
                    }
                    if (dto.Images != null)
                    {
                        foreach (var item in dto.Images)
                        {
                            CreateImageDto contImage = new CreateImageDto()
                            {
                                ContantIdfk = id,
                                Path = item,
                            };

                            ContentImage newobj = new ContentImage()
                            {
                                ContentId = contImage.ContantIdfk,
                                Path = contImage.Path
                            };
                            await _context.ContentImages.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    if (dto.Images != null)
                    {
                        foreach (var item in dto.Images)
                        {
                            CreateImageDto contImage = new CreateImageDto()
                            {
                                ContantIdfk = id,
                                Path = item,
                            };
                            ContentImage newobj = new ContentImage()
                            {
                                ContentId = contImage.ContantIdfk,
                                Path = contImage.Path
                            };
                            await _context.ContentImages.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }
                }
                #endregion

                #region Update PDFs
                var pdfs = await _context.ContentPdfs.Where(x => x.ContentId == id).ToListAsync();
                if (pdfs != null)
                {
                    foreach (var item in pdfs)
                    {
                        _context.ContentPdfs.Remove(item);
                        _context.SaveChanges();
                    }
                    if (dto.Pdfs != null)
                    {
                        foreach (var item in dto.Pdfs)
                        {
                            CreatePDFDto contpdf = new CreatePDFDto()
                            {
                                ContantIdfk = id,
                                Path = item
                            };

                            ContentPdf newobj = new ContentPdf()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentPdfs.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    if (dto.Pdfs != null)
                    {
                        foreach (var item in dto.Pdfs)
                        {
                            CreatePDFDto contpdf = new CreatePDFDto()
                            {
                                ContantIdfk = id,
                                Path = item,
                            };

                            ContentPdf newobj = new ContentPdf()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentPdfs.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }
                }
                #endregion


                #region Update Video
                var video = await _context.ContentVideos.Where(x => x.ContentId == id).ToListAsync();
                if (video != null)
                {
                    foreach (var item in video)
                    {
                        _context.ContentVideos.Remove(item);
                        _context.SaveChanges();
                    }
                    if (dto.Videos != null)
                    {
                        foreach (var item in dto.Videos)
                        {
                            CreateVideoDto contpdf = new CreateVideoDto()
                            {
                                ContantIdfk = id,
                                Path = item,
                            };

                            ContentVideo newobj = new ContentVideo()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentVideos.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    if (dto.Videos != null)
                    {
                        foreach (var item in dto.Videos)
                        {
                            CreateVideoDto contpdf = new CreateVideoDto()
                            {
                                ContantIdfk = id,
                                Path = item,
                            };

                            ContentVideo newobj = new ContentVideo()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentVideos.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }
                }
                #endregion

                ContentDetailsWithComponentsDto updatedobg = new ContentDetailsWithComponentsDto()
                {
                    Id = content.Id,
                    Title = content.Title,
                    Date = content.Date,
                    ClassName = content.Class.Title,
                    ClassIdfk = content.ClassId,
                    Images = _context.ContentImages.Where(c => c.ContentId == id).Select(c => c.Path).ToArray(),
                    Pdfs = _context.ContentPdfs.Where(c => c.ContentId == id).Select(c => c.Path).ToArray(),
                    Videos = _context.ContentVideos.Where(c => c.ContentId == id).Select(c => c.Path).ToArray(),
                };
                return Ok(updatedobg);
            }
        }
        #endregion

                
        #region DeleteSpecificContentByit'sID
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteContentByID(int id)
        {
            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return BadRequest("There is no Content with this ID");
            }
            else
            {
                _context.Contents.Remove(content);
                await _context.SaveChangesAsync();
                return Ok(content);
            }
        }
        #endregion

        
        #region Delete Specific Content By its ID
        [HttpGet("Month/{number:int}")]
        public async Task<IActionResult> GetContentsByMonthId(int number)
        {
            var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == number).ToListAsync();
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
                    ClassName = cont.Class.Title,
                    ClassIdfk = cont.ClassId,
                };
                dtos.Add(dto);
            }

            return Ok(dtos.OrderBy(d => d.Date));
        } 
        #endregion


        #region ContentForStudentsBasedOnMonths


        [HttpGet("StudentMonth/{StId:int}")]
        public async Task<IActionResult> ContentForStudentsBasedOnMonths(int StId)
        {
            List<ContentDetailsWithComponentsDto> ContentList = new List<ContentDetailsWithComponentsDto>();
            var student = await _context.Students.FirstOrDefaultAsync(x => x.StId == StId);
            if (student == null)
            {
                return BadRequest("هذا الطالب غير مسجل ");
            }
            Month months = await _context.Months.Where(x => x.StId == StId).FirstOrDefaultAsync();
            if (months == null)
            {
                return BadRequest("هذا الطالب لم يحجز حتى الآن");
            }
            else
            {
                if (months.Jan == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 1).Where(x=>x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Feb == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 2).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Mar == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 3).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Apr == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 4).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.May == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 5).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Jun == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 6).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Jul == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 7).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Aug == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 8).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Sep == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 9).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Oct == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 10).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Nov == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 11).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                if (months.Dec == true)
                {
                    var conts = await _context.Contents.Include(m => m.Class).Where(m => m.Date.Month == 12).Where(x => x.ClassId == student.ClassId).ToListAsync();
                    if (conts.Count > 0)
                    {
                        foreach (var cont in conts)
                        {
                            ContentDetailsWithComponentsDto dto = new ContentDetailsWithComponentsDto
                            {
                                Id = cont.Id,
                                Title = cont.Title,
                                Date = cont.Date,
                                ClassName = cont.Class.Title,
                                ClassIdfk = cont.ClassId,
                                Images = _context.ContentImages.Select(c => c.Path).ToArray(),
                                Pdfs = _context.ContentPdfs.Select(c => c.Path).ToArray(),
                                Videos = _context.ContentVideos.Select(c => c.Path).ToArray(),
                            };
                            ContentList.Add(dto);
                        }
                    }
                }

                return Ok(ContentList);
            }
            
        }
        #endregion


        #region AddOrUpdate
        [HttpPost("AddOrUpdateContent")]
        public async Task<IActionResult> AddOrUpdateContent(AddOrUpdate dto)
        {
            
            var content = await _context.Contents.Include(c => c.Class).Include(p => p.ContentPdfs).Include(x => x.ContentVideos).Include(a => a.ContentImages).FirstOrDefaultAsync(h => h.Id == dto.Id);
            if (content == null)
            {
                return NotFound("Invalid Content ID");
            }

            if (dto == null)
            {
                return BadRequest("Can't add null Content");
            }
            else
            {
                content.ClassId = content.ClassId;
                content.Title = dto.Title;
                content.Date = dto.Date;

                _context.SaveChanges();

                #region Update Imgs
                var imgs = await _context.ContentImages.Where(x => x.ContentId == dto.Id).ToListAsync();
                if (imgs != null)
                {
                    foreach (var item in imgs)
                    {
                        _context.ContentImages.Remove(item);
                        _context.SaveChanges();
                    }
                    if (dto.Images != null)
                    {
                        foreach (var item in dto.Images)
                        {
                            CreateImageDto contImage = new CreateImageDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };

                            ContentImage newobj = new ContentImage()
                            {
                                ContentId = contImage.ContantIdfk,
                                Path = contImage.Path
                            };
                            await _context.ContentImages.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    if (dto.Images != null)
                    {
                        foreach (var item in dto.Images)
                        {
                            CreateImageDto contImage = new CreateImageDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };
                            ContentImage newobj = new ContentImage()
                            {
                                ContentId = contImage.ContantIdfk,
                                Path = contImage.Path
                            };
                            await _context.ContentImages.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }
                }
                #endregion

                #region Update PDFs
                var pdfs = await _context.ContentPdfs.Where(x => x.ContentId == dto.Id).ToListAsync();
                if (pdfs != null)
                {
                    foreach (var item in pdfs)
                    {
                        _context.ContentPdfs.Remove(item);
                        _context.SaveChanges();
                    }
                    if (dto.Pdfs != null)
                    {
                        foreach (var item in dto.Pdfs)
                        {
                            CreatePDFDto contpdf = new CreatePDFDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item
                            };

                            ContentPdf newobj = new ContentPdf()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentPdfs.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    if (dto.Pdfs != null)
                    {
                        foreach (var item in dto.Pdfs)
                        {
                            CreatePDFDto contpdf = new CreatePDFDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };

                            ContentPdf newobj = new ContentPdf()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentPdfs.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }
                }
                #endregion


                #region Update Video
                var video = await _context.ContentVideos.Where(x => x.ContentId == dto.Id).ToListAsync();
                if (video != null)
                {
                    foreach (var item in video)
                    {
                        _context.ContentVideos.Remove(item);
                        _context.SaveChanges();
                    }
                    if (dto.Videos != null)
                    {
                        foreach (var item in dto.Videos)
                        {
                            CreateVideoDto contpdf = new CreateVideoDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };

                            ContentVideo newobj = new ContentVideo()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentVideos.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    if (dto.Videos != null)
                    {
                        foreach (var item in dto.Videos)
                        {
                            CreateVideoDto contpdf = new CreateVideoDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };

                            ContentVideo newobj = new ContentVideo()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentVideos.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }
                }
                #endregion

                ContentDetailsWithComponentsDto updatedobg = new ContentDetailsWithComponentsDto()
                {
                    Id = content.Id,
                    Title = content.Title,
                    Date = content.Date,
                    ClassName = content.Class.Title,
                    ClassIdfk = content.ClassId,
                    Images = _context.ContentImages.Where(c => c.ContentId == dto.Id).Select(c => c.Path).ToArray(),
                    Pdfs = _context.ContentPdfs.Where(c => c.ContentId == dto.Id).Select(c => c.Path).ToArray(),
                    Videos = _context.ContentVideos.Where(c => c.ContentId == dto.Id).Select(c => c.Path).ToArray(),
                };
                return Ok(updatedobg);
            }
        }
        #endregion


        #region NewUpdateContentByit'sId
        [HttpPut("NewUpdateContent")]
        public async Task<IActionResult> UpdateContent2(AddOrUpdate dto)
        {
            var content = await _context.Contents.Include(c => c.Class).Include(p => p.ContentPdfs).Include(x => x.ContentVideos).Include(a => a.ContentImages).FirstOrDefaultAsync(h => h.Id == dto.Id);
            if (content == null)
            {
                return NotFound("Invalid Content ID");
            }

            if (dto == null)
            {
                return BadRequest("Can't add null Content");
            }
            else
            {
                content.ClassId = content.ClassId;
                content.Title = dto.Title;
                content.Date = dto.Date;

                _context.SaveChanges();

                #region Update Imgs
                var imgs = await _context.ContentImages.Where(x => x.ContentId == dto.Id).ToListAsync();
                if (imgs != null)
                {
                    foreach (var item in imgs)
                    {
                        _context.ContentImages.Remove(item);
                        _context.SaveChanges();
                    }
                    if (dto.Images != null)
                    {
                        foreach (var item in dto.Images)
                        {
                            CreateImageDto contImage = new CreateImageDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };

                            ContentImage newobj = new ContentImage()
                            {
                                ContentId = contImage.ContantIdfk,
                                Path = contImage.Path
                            };
                            await _context.ContentImages.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    if (dto.Images != null)
                    {
                        foreach (var item in dto.Images)
                        {
                            CreateImageDto contImage = new CreateImageDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };
                            ContentImage newobj = new ContentImage()
                            {
                                ContentId = contImage.ContantIdfk,
                                Path = contImage.Path
                            };
                            await _context.ContentImages.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }
                }
                #endregion

                #region Update PDFs
                var pdfs = await _context.ContentPdfs.Where(x => x.ContentId == dto.Id).ToListAsync();
                if (pdfs != null)
                {
                    foreach (var item in pdfs)
                    {
                        _context.ContentPdfs.Remove(item);
                        _context.SaveChanges();
                    }
                    if (dto.Pdfs != null)
                    {
                        foreach (var item in dto.Pdfs)
                        {
                            CreatePDFDto contpdf = new CreatePDFDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item
                            };

                            ContentPdf newobj = new ContentPdf()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentPdfs.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    if (dto.Pdfs != null)
                    {
                        foreach (var item in dto.Pdfs)
                        {
                            CreatePDFDto contpdf = new CreatePDFDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };

                            ContentPdf newobj = new ContentPdf()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentPdfs.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }
                }
                #endregion


                #region Update Video
                var video = await _context.ContentVideos.Where(x => x.ContentId == dto.Id).ToListAsync();
                if (video != null)
                {
                    foreach (var item in video)
                    {
                        _context.ContentVideos.Remove(item);
                        _context.SaveChanges();
                    }
                    if (dto.Videos != null)
                    {
                        foreach (var item in dto.Videos)
                        {
                            CreateVideoDto contpdf = new CreateVideoDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };

                            ContentVideo newobj = new ContentVideo()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentVideos.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    if (dto.Videos != null)
                    {
                        foreach (var item in dto.Videos)
                        {
                            CreateVideoDto contpdf = new CreateVideoDto()
                            {
                                ContantIdfk = dto.Id,
                                Path = item,
                            };

                            ContentVideo newobj = new ContentVideo()
                            {
                                ContentId = contpdf.ContantIdfk,
                                Path = contpdf.Path
                            };
                            await _context.ContentVideos.AddAsync(newobj);
                            _context.SaveChanges();
                        }
                    }
                }
                #endregion

                ContentDetailsWithComponentsDto updatedobg = new ContentDetailsWithComponentsDto()
                {
                    Id = content.Id,
                    Title = content.Title,
                    Date = content.Date,
                    ClassName = content.Class.Title,
                    ClassIdfk = content.ClassId,
                    Images = _context.ContentImages.Where(c => c.ContentId == dto.Id).Select(c => c.Path).ToArray(),
                    Pdfs = _context.ContentPdfs.Where(c => c.ContentId == dto.Id).Select(c => c.Path).ToArray(),
                    Videos = _context.ContentVideos.Where(c => c.ContentId == dto.Id).Select(c => c.Path).ToArray(),
                };
                return Ok(updatedobg);
            }
        }
        #endregion

    }
}
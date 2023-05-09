using GraduationProjectAPI.Dtos;
using GraduationProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        #region Load
        private readonly EducationPlatform_GraduationProjectContext _context;
        public ClassesController(EducationPlatform_GraduationProjectContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllClasses
        [HttpGet("GetAllClasses")]
        public async Task<IActionResult> GetAllClasses()
        {
            List<GetAllClassesDTO> classesdto = new List<GetAllClassesDTO>();
            var classes = await _context.Classes.Include(c=>c.Chat).ToListAsync();
            if (classes.Count() > 0)
            {
                foreach (var c in classes)
                {
                    GetAllClassesDTO obj = new GetAllClassesDTO();
                    obj.Class_ID = c.ClassId;
                    obj.Class_Name = c.Title;
                    obj.Chat_ID = c.Chat.ChatId;
                    classesdto.Add(obj);
                }
                return Ok(classesdto);
            }
            else
            {
                return BadRequest(" لا توجد صفوف دراسية");
            }

        }
        #endregion

        #region GetOneClass
        [HttpGet("GetOneClass/{id:int}")]
        public async Task<IActionResult> GetOneClasse(int id)
        {
            var cls = await _context.Classes.Include(c=>c.Chat).FirstOrDefaultAsync(c => c.ClassId == id);
            if (cls != null)
            {
                GetAllClassesDTO getcls = new GetAllClassesDTO
                {
                    Class_ID = cls.ClassId,
                    Class_Name = cls.Title,
                    Chat_ID = cls.Chat.ChatId
            };
                return Ok(getcls);
            }
            else
            {
                return BadRequest("لا يوجد صف دراسي بهذا الكود");
            }

        }
        #endregion

        #region UpdateClass
        [HttpPut("UpdateClass/{id:int}")]
        public async Task<IActionResult> UpdateClasse(int id, EditClassDTO editclass)
        {
            var cls = await _context.Classes.FirstOrDefaultAsync(c => c.ClassId == id);
            if (cls != null)
            {
                cls.Title = editclass.ClassTitle;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("لا يوجد صف دراسي بهذا الكود");
            }

        }
        #endregion

        #region AddClass
        [HttpPost("CreateClass")]
        public IActionResult CreateClasses(AddClassDTO newclass)
        {
            try
            {
                var cls = new Class();
                cls.Title = newclass.Class_Name;                
                _context.Classes.Add(cls);
                _context.SaveChanges();
                return Ok("Class added successfully");

            }
            catch
            {
                return BadRequest("erorr");
            }
        }
        #endregion

        #region DeleteClass
        [HttpDelete("DeleteClass/{id:int}")]
        public IActionResult DeleteClasses(int id)
        {
            Class? cls = _context.Classes.FirstOrDefault(e => e.ClassId == id);
            if (cls != null)
            {
                _context.Classes.Remove(cls);
                _context.SaveChanges();
                return Ok("Class deleted successfully");
            }
            else
            {
                return NotFound("Can't find class with this id");
            }
        }
        #endregion
    }
}
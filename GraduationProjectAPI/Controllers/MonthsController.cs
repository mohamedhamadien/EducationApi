using GraduationProjectAPI.Dtos;
using GraduationProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthsController : ControllerBase
    {
        #region Load
        private readonly EducationPlatform_GraduationProjectContext _context;
        public MonthsController(EducationPlatform_GraduationProjectContext context)
        {
            _context = context;
        }
        #endregion

        #region Addmonth
        [HttpPost("StudentMonths")]
        public IActionResult StudentMonths([FromForm] MonthsDTO newclass)
        {
            List<int> StudentsIDs = _context.Students.Select(x => x.StId).ToList();
            List<int> StudentsIDsInMonths = _context.Months.Select(x => x.StId).ToList();
            if (!StudentsIDs.Contains(newclass.StID))
            {
                return BadRequest("هذا الطالب غير موجود");
            }

            if (StudentsIDsInMonths.Contains(newclass.StID))
            {
                return BadRequest("هذا الطالب موجود بالفعل");
            }
            try
            {
                var newmonths = new Month();
                newmonths.Jan = newclass.Jan;
                newmonths.Feb = newclass.Feb;
                newmonths.Mar = newclass.Mar;
                newmonths.Apr = newclass.Apr;
                newmonths.May = newclass.May;
                newmonths.Jun = newclass.Jun;
                newmonths.Jul = newclass.Jul;
                newmonths.Aug = newclass.Aug;
                newmonths.Sep = newclass.Sep;
                newmonths.Oct = newclass.Oct;
                newmonths.Nov = newclass.Nov;
                newmonths.Dec = newclass.Dec;
                newmonths.StId = newclass.StID;
                _context.Months.Add(newmonths);
                _context.SaveChanges();
                return Ok("Added successfully");
            }
            catch
            {
                return BadRequest("erorr");
            }
        }
        #endregion

        #region GetStudentMonthsByStID
        [HttpGet("GetStudentMonths/{id:int}")]
        public async Task<IActionResult> GetStudentMonths(int id)
        {
            var newclass = await _context.Months.Include(x => x.St).FirstOrDefaultAsync(a => a.StId == id);
            if (newclass == null)
            {
                return NotFound("لا يوجد طالب بهذا الكود");
            }
            else
            {
                MonthsDTO newmonths = new MonthsDTO();
                newmonths.Jan = newclass.Jan;
                newmonths.Feb = newclass.Feb;
                newmonths.Mar = newclass.Mar;
                newmonths.Apr = newclass.Apr;
                newmonths.May = newclass.May;
                newmonths.Jun = newclass.Jun;
                newmonths.Jul = newclass.Jul;
                newmonths.Aug = newclass.Aug;
                newmonths.Sep = newclass.Sep;
                newmonths.Oct = newclass.Oct;
                newmonths.Nov = newclass.Nov;
                newmonths.Dec = newclass.Dec;
                newmonths.StID = newclass.StId;
                newmonths.StudentName = newclass.St.StName;
                return Ok(newmonths);
            }
        }
        #endregion

        #region updatestudentMonths
        [HttpPut("updatestudentMonths")]
        public IActionResult UpdateStudent(UpdateMonthDTO dto)
        {
            List<int> stIDS = _context.Months.Select(s => s.StId).ToList();

            if (!stIDS.Contains(dto.StID))
            {
                return BadRequest("لا يوجد طالب بهذا الكود");
            }

            var months = _context.Months.FirstOrDefault(x => x.StId == dto.StID);
            months.Jan = dto.Jan;
            months.Feb = dto.Feb;
            months.Mar = dto.Mar;
            months.Apr = dto.Apr;
            months.May = dto.May;
            months.Jun = dto.Jun;
            months.Jul = dto.Jul;
            months.Aug = dto.Aug;
            months.Sep = dto.Sep;
            months.Oct = dto.Oct;
            months.Nov = dto.Nov;
            months.Dec = dto.Dec;
            months.StId = dto.StID;
            _context.SaveChanges();
            return Ok("Updated successfully");

        }
        #endregion

        #region GetStudentMonthsByClassID
        [HttpGet("GetCLassStudentsMonths/{id:int}")]
        public async Task<IActionResult> GetCLassStudentsMonths(int id)
        {
            var IsValidClass = _context.Classes.Any(c => c.ClassId == id);
            if (!IsValidClass)
            {
                return BadRequest("لا يوجد صف دراسي بهذا الكود");
            }

            List<int> studentsIDsMonths = await _context.Months.Where(s => s.St.ClassId == id).Select(x => x.StId).ToListAsync();
            List<MonthsDTO> months = new List<MonthsDTO>();
            if (studentsIDsMonths.Count > 0)
            {
                foreach (var c in studentsIDsMonths)
                {
                    MonthsDTO obj2 = new MonthsDTO();
                    Month obj = _context.Months.Include(x => x.St).FirstOrDefault(x => x.StId == c);
                    obj2.Jan = obj.Jan;
                    obj2.Feb = obj.Feb;
                    obj2.Mar = obj.Mar;
                    obj2.Apr = obj.Apr;
                    obj2.May = obj.May;
                    obj2.Jun = obj.Jun;
                    obj2.Jul = obj.Jul;
                    obj2.Aug = obj.Aug;
                    obj2.Sep = obj.Sep;
                    obj2.Oct = obj.Oct;
                    obj2.Nov = obj.Nov;
                    obj2.Dec = obj.Dec;
                    obj2.StID = c;
                    obj2.StudentName = obj.St.StName;
                    months.Add(obj2);
                }
                return Ok(months);
            }
            else
            {
                return NotFound("لا يوجد طلاب في هذا الصف الدراسي");
            }
        }
        #endregion
    }
}
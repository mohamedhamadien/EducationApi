//using GraduationProjectAPI.Dtos;
//using GraduationProjectAPI.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace GraduationProjectAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StudentsDetailsController : ControllerBase
//    {
//        #region Load
//        private readonly EducationPlatform_GraduationProjectContext _context;
//        public StudentsDetailsController(EducationPlatform_GraduationProjectContext context)
//        {
//            _context = context;
//        }
//        #endregion

//        #region createstudent
//        [HttpPost]
//        public IActionResult CreateStudent([FromForm] CreateStudentDTO dto)
//        {
//            var IsValidClass = _context.Classes.Any(c => c.ClassId == dto.ClassIdfk);
//            if (!IsValidClass)
//            {
//                return BadRequest("Invalid Class ID");
//            }

//            List<string> allusernames = _context.AspNetUsers.Select(x => x.UserName).ToList();
//            if (allusernames.Contains(dto.UserNameFk))
//            {
//                return BadRequest("اسم المستخدم موجود بالفعل");
//            }

//            try
//            {
//                Models.AspNetUser login = new Models.AspNetUser
//                {
//                    UserName = dto.UserNameFk,
//                    Password = dto.Password,
//                    Role = "student",
//                };
//                _context.Logins.Add(login);
//                _context.SaveChanges();

//                Student student = new Student
//                {
//                    StName = dto.StName,
//                    Address = dto.Address,
//                    Available = dto.Available,
//                    ClassIdfk = dto.ClassIdfk,
//                    PaymentState = dto.PaymentState,
//                    Phone = dto.Phone,
//                    RegistedDate = dto.RegistedDate,
//                    UserNameFk = dto.UserNameFk,
//                };
//                _context.Students.Add(student);
//                _context.SaveChanges();
//                return Ok("Students added successfully");
//            }
//            catch
//            {
//                return BadRequest();
//            }

//        }
//        #endregion

//        #region updatestudent
//        [HttpPut("updatestudent/{id:int}")]
//        public IActionResult UpdateStudent(int id, [FromForm] UpdateStudent dto)
//        {
//            var student = _context.Students.Find(id);
//            if (student == null)
//            {
//                return BadRequest("لا يوجد طالب بهذا الكود");
//            }

//            var IsValidClass = _context.Classes.Any(c => c.ClassId == dto.ClassIdfk);
//            if (!IsValidClass)
//            {
//                return BadRequest("Invalid Class ID");
//            }
//            var login = _context.Logins.FirstOrDefault(x => x.UserName == student.UserNameFk);

//            login.Password = dto.Password;
//            _context.SaveChanges();

//            student.StName = dto.StName;
//            student.Address = dto.Address;
//            student.Available = dto.Available;
//            student.ClassIdfk = dto.ClassIdfk;
//            student.PaymentState = dto.PaymentState;
//            student.Phone = dto.Phone;
//            student.RegistedDate = dto.RegistedDate;
//            _context.SaveChanges();
//            return Ok("Students updated successfully");

//        }
//        #endregion

//        #region GetAllStudents
//        [HttpGet("GetAllStudents")]
//        public async Task<IActionResult> GetAllStudents()
//        {
//            List<GetAllStudentsDTO> studentsdto = new List<GetAllStudentsDTO>();
//            var stds = await _context.Students.Include(x => x.ClassIdfkNavigation).Include(s => s.UserNameFkNavigation).ToListAsync();
//            if (stds.Count() > 0)
//            {
//                foreach (var c in stds)
//                {
//                    GetAllStudentsDTO obj = new GetAllStudentsDTO();
//                    obj.St_ID = c.StId;
//                    obj.StName = c.StName;
//                    obj.Address = c.Address;
//                    obj.Available = c.Available;
//                    obj.Password = c.UserNameFkNavigation.Password;
//                    obj.Phone = c.Phone;
//                    obj.RegistedDate = c.RegistedDate;
//                    obj.UserNameFk = c.UserNameFk;
//                    obj.ClassTitle = c.ClassIdfkNavigation.Title;
//                    obj.PaymentState = c.PaymentState;
//                    studentsdto.Add(obj);
//                }
//                return Ok(studentsdto.OrderBy(s => s.StName));
//            }
//            else
//            {
//                return BadRequest(" لا يوجد طلاب");
//            }
//        }
//        #endregion

//        #region GetAllStudentsByClass_ID
//        [HttpGet("ClassStudents/{id:int}")]
//        public async Task<IActionResult> GetClassStudents(int id)
//        {
//            var IsValidClass = _context.Classes.Any(c => c.ClassId == id);
//            if (!IsValidClass)
//            {
//                return BadRequest("لا يوجد صف دراسي بهذا الكود");
//            }
//            List<GetAllStudentsDTO> studentsdto = new List<GetAllStudentsDTO>();
//            var stds = await _context.Students.Where(w => w.ClassIdfk == id).Include(x => x.ClassIdfkNavigation).Include(s => s.UserNameFkNavigation).ToListAsync();
//            if (stds.Count() > 0)
//            {
//                foreach (var c in stds)
//                {
//                    GetAllStudentsDTO obj = new GetAllStudentsDTO();
//                    obj.St_ID = c.StId;
//                    obj.StName = c.StName;
//                    obj.Address = c.Address;
//                    obj.Available = c.Available;
//                    obj.Password = c.UserNameFkNavigation.Password;
//                    obj.Phone = c.Phone;
//                    obj.RegistedDate = c.RegistedDate;
//                    obj.UserNameFk = c.UserNameFk;
//                    obj.ClassTitle = c.ClassIdfkNavigation.Title;
//                    obj.PaymentState = c.PaymentState;
//                    studentsdto.Add(obj);
//                }
//                return Ok(studentsdto.OrderBy(s => s.StName));
//            }
//            else
//            {
//                return BadRequest(" لا يوجد طلاب في هذا الصف الدراسي");
//            }
//        }
//        #endregion

//        #region GetOneStudentsByST_ID
//        [HttpGet("getstudent/{id:int}")]
//        public async Task<IActionResult> GetOneStudent(int id)
//        {
//            var stds = await _context.Students.Include(x => x.ClassIdfkNavigation).Include(s => s.UserNameFkNavigation).FirstOrDefaultAsync(a => a.StId == id);
//            if (stds == null)
//            {
//                return NotFound("لا يوجد طالب بهذا الكود");
//            }
//            else
//            {
//                GetAllStudentsDTO obj = new GetAllStudentsDTO();
//                obj.St_ID = stds.StId;
//                obj.StName = stds.StName;
//                obj.Address = stds.Address;
//                obj.Available = stds.Available;
//                obj.Password = stds.UserNameFkNavigation.Password;
//                obj.Phone = stds.Phone;
//                obj.RegistedDate = stds.RegistedDate;
//                obj.UserNameFk = stds.UserNameFk;
//                obj.ClassTitle = stds.ClassIdfkNavigation.Title;
//                obj.PaymentState = stds.PaymentState;
//                return Ok(obj);
//            }
//        }
//        #endregion

//        #region DeleteStudent
//        [HttpDelete("DeleteStudent/{id:int}")]
//        public IActionResult DeleteStudent(int id)
//        {
//            var std = _context.Students.Find(id);
//            if (std != null)
//            {
//                _context.Students.Remove(std);
//                _context.SaveChanges();
//                return Ok("تم حذف الطالب بنجاح");
//            }
//            else
//            {
//                return BadRequest("لا يوجد طالب بهذا الكود");
//            }
//        }
//        #endregion
//    }
//}

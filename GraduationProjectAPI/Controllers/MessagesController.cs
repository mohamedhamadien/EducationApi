//using GraduationProjectAPI.Dtos;
//using GraduationProjectAPI.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace GraduationProjectAPI.Controllers
//{
//    [Route("/[controller]")]
//    [ApiController]
//    public class MessagesController : ControllerBase
//    {
//        #region Load
//        private readonly GraduationProjectContext _context;
//        public MessagesController(GraduationProjectContext context)
//        {
//            _context = context;
//        }
//        #endregion

//        #region GetAllMessages
//        [HttpGet("GetAllMessages")]
//        public async Task<IActionResult> GetAllMessages()
//        {
//            List<GetAllMessages> msgdto = new List<GetAllMessages>();
//            var message = await _context.Messages.Include(x => x.StIdfkNavigationSt).ToListAsync();
//            if (message.Count() > 0)
//            {
//                foreach (var c in message)
//                {
//                    GetAllMessages obj = new GetAllMessages();
//                    obj.M_ID = c.Mid;
//                    obj.M_Body = c.Body;
//                    obj.M_Date = c.Date;
//                    obj.St_ID = c.StIdfkNavigationStId;
//                    obj.StName = c.StIdfkNavigationSt.StName;
//                    msgdto.Add(obj);
//                }
//                return Ok(msgdto);
//            }
//            else
//            {
//                return BadRequest(" لا توجد رسائل");
//            }
//        }
//        #endregion

//        #region GetMessagesForOneChat
//        [HttpGet("ChatMessages/{id:int}")]
//        public async Task<IActionResult> ChatMessages(int id)
//        {
//            var IsValidClass = _context.Classes.Any(c => c.ClassId == id);
//            if (!IsValidClass)
//            {
//                return BadRequest("لا يوجد صف دراسي بهذا الكود");
//            }
//            List<GetAllMessages> msgdto = new List<GetAllMessages>();
//            var message = await _context.Messages.Include(x => x.StIdfkNavigationSt).Where(m => m.ChatIdfkNavigationChatId == id).ToListAsync();
//            if (message.Count() > 0)
//            {
//                foreach (var c in message)
//                {
//                    GetAllMessages obj = new GetAllMessages();
//                    obj.M_ID = c.Mid;
//                    obj.M_Body = c.Body;
//                    obj.M_Date = c.Date;
//                    obj.St_ID = c.StIdfkNavigationStId;
//                    obj.StName = c.StIdfkNavigationSt.StName;
//                    msgdto.Add(obj);
//                }
//                return Ok(msgdto);
//            }
//            else
//            {
//                return BadRequest(" لا توجد رسائل في هذا الصف  ");
//            }
//        }
//        #endregion
//    }
//}

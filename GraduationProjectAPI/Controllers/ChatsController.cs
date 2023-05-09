//using GraduationProjectAPI.Dtos;
//using GraduationProjectAPI.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace GraduationProjectAPI.Controllers
//{
//    [Route("/[controller]")]
//    [ApiController]
//    public class ChatsController : ControllerBase
//    {
//        #region Load
//        private readonly GraduationProjectContext _context;
//        public ChatsController(GraduationProjectContext context)
//        {
//            _context = context;
//        }
//        #endregion

//        #region GetAllChats
//        [HttpGet("GetAllChats")]
//        public async Task<IActionResult> GetAllChats()
//        {
//            List<GetChatsDTO> chatsdto = new List<GetChatsDTO>();
//            var chats = await _context.Chats.ToListAsync();
//            if (chats.Count() > 0)
//            {
//                foreach (var c in chats)
//                {
//                    GetChatsDTO obj = new GetChatsDTO();
//                    obj.ID = c.ChatId;
//                    obj.Title = c.Title;
//                    chatsdto.Add(obj);
//                }
//                return Ok(chatsdto);
//            }
//            else
//            {
//                return BadRequest(" لا توجد محادثات");
//            }
//        }
//        #endregion

//        #region GetOneChat
//        [HttpGet("GetOneChat/{id:int}")]
//        public async Task<IActionResult> GetOneChat(int id)
//        {

//            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.ChatId == id);
//            if (chat != null)
//            {

//                List<GetAllMessages> msgdto = new List<GetAllMessages>();
//                var message = await _context.Messages.Include(x => x.StIdfkNavigationSt).Where(m => m.ChatIdfk == id).ToListAsync();
//                if (message.Count > 0)
//                {
//                    foreach (var c in message)
//                    {
//                        GetAllMessages obj = new GetAllMessages();
//                        obj.M_ID = c.Mid;
//                        obj.M_Body = c.Body;
//                        obj.M_Date = c.Date;
//                        obj.St_ID = c.StIdfk;
//                        obj.StName = c.StIdfkNavigationSt.StName;
//                        msgdto.Add(obj);
//                    }
//                    GetChatsDTO getchat = new GetChatsDTO
//                    {
//                        ID = chat.ChatId,
//                        Title = chat.Title,
//                        AllMessages = msgdto

//                    };
//                    return Ok(getchat);
//                }
//                else
//                {
//                    return BadRequest(" لا توجد رسائل في هذا الصف  ");
//                }

//            }
//            else
//            {
//                return BadRequest("لا توجد محادثة بهذا الكود");
//            }
//        }
//        #endregion

//        #region UpdateChat
//        [HttpPut("UpdateChat/{id:int}")]
//        public async Task<IActionResult> UpdateChat(int id, UpdateChat updatechat)
//        {
//            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.ChatId == id);
//            if (chat != null)
//            {
//                chat.Title = updatechat.Title;
//                _context.SaveChanges();
//                return Ok();
//            }
//            else
//            {
//                return BadRequest("لا توجد محادثة بهذا الكود");
//            }
//        }
//        #endregion
//    }
//}

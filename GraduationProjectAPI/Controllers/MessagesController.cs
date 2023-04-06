using GraduationProjectAPI.Dtos;
using GraduationProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        #region Load
        private readonly EducationPlatformContext _context;
        public MessagesController(EducationPlatformContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllMessages
        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetAllMessages()
        {
            List<GetAllMessages> msgdto = new List<GetAllMessages>();
            var message = await _context.Messages.ToListAsync();
            if (message != null)
            {
                foreach (var c in message)
                {
                    GetAllMessages obj = new GetAllMessages();
                    obj.M_ID = c.MId;
                    obj.M_Body = c.Body;
                    obj.M_Date = c.Date;
                    obj.Chat_ID = c.ChatIdfk;
                    obj.St_ID = c.StIdfk;
                    msgdto.Add(obj);
                }
                return Ok(msgdto);
            }
            else
            {
                return BadRequest(" لا توجد رسائل");
            }
        }
        #endregion

        #region GetMessagesForOneChat
        [HttpGet("ChatMessages/{id:int}")]
        public async Task<IActionResult> ChatMessages(int id)
        {
            List<GetAllMessages> msgdto = new List<GetAllMessages>();
            var message = await _context.Messages.Where(m => m.ChatIdfk == id).ToListAsync();
            if (message != null)
            {
                foreach (var c in message)
                {
                    GetAllMessages obj = new GetAllMessages();
                    obj.M_ID = c.MId;
                    obj.M_Body = c.Body;
                    obj.M_Date = c.Date;
                    obj.Chat_ID = c.ChatIdfk;
                    obj.St_ID = c.StIdfk;
                    msgdto.Add(obj);
                }
                return Ok(msgdto);
            }
            else
            {
                return BadRequest(" لا توجد رسائل في هذا الصف  ");
            }
        }
        #endregion
    }
}

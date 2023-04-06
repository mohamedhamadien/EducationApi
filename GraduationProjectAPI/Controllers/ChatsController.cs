using GraduationProjectAPI.Dtos;
using GraduationProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        #region Load
        private readonly EducationPlatformContext _context;
        public ChatsController(EducationPlatformContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllChats
        [HttpGet("GetAllChats")]
        public async Task<IActionResult> GetAllChats()
        {
            List<GetChatsDTO> chatsdto = new List<GetChatsDTO>();
            var chats = await _context.Chats.ToListAsync();
            if (chats != null)
            {
                foreach (var c in chats)
                {
                    GetChatsDTO obj = new GetChatsDTO();
                    obj.ID = c.ChatId;
                    obj.Title = c.Title;
                    chatsdto.Add(obj);
                }
                return Ok(chatsdto);
            }
            else
            {
                return BadRequest(" لا توجد محادثات");
            }
        }
        #endregion

        #region GetOneChat
        [HttpGet("GetOneChat/{id:int}")]
        public async Task<IActionResult> GetOneChat(int id)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.ChatId == id);
            if (chat != null)
            {
                GetChatsDTO getchat = new GetChatsDTO
                {
                    ID = chat.ChatId,
                    Title = chat.Title
                };
                return Ok(getchat);
            }
            else
            {
                return BadRequest("لا توجد محادثة بهذا الكود");
            }
        }
        #endregion

        #region UpdateChat
        [HttpPut("UpdateChat/{id:int}")]
        public async Task<IActionResult> UpdateChat(int id, UpdateChat updatechat)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.ChatId == id);
            if (chat != null)
            {
                chat.Title = updatechat.Title;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("لا توجد محادثة بهذا الكود");
            }
        }
        #endregion
    }
}

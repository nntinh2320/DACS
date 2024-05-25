using DACS.Data;
using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Admin.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository, ApplicationDbContext context)
        {
            _chatRepository = chatRepository;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var all_Chats = from s in _context.Chats
                            select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                string lowercaseSearchString = searchString.ToLower();
                all_Chats = all_Chats.Where(s =>
                    s.Name.ToLower().Contains(lowercaseSearchString));
            }

            return View(await all_Chats.ToListAsync());
        }

        [HttpPost]
        public JsonResult GetSearchChatValue(string search)
        {
            var chatResult = _context.Chats.Where(x => x.Name.Contains(search))
                                            .Select(x => new
                                            {
                                                label = x.Name,
                                                value = x.Name
                                            }).ToList();
            return Json(chatResult);
        }

        public async Task<IActionResult> Display(int id)
        {
            var chat = await _chatRepository.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return View(chat);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Chat chat)
        {
            bool check = await _context.Chats.AnyAsync(c => c.Name == chat.Name);
            if (check)
            {
                TempData["Message"] = "Đã có chất này rồi";
                return View(chat);
            }

            if (ModelState.IsValid)
            {
                await _chatRepository.AddChatAsync(chat);
                return RedirectToAction(nameof(Index));
            }

            return View(chat);
        }

        public async Task<IActionResult> Update(int id)
        {
            var chat = await _chatRepository.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _chatRepository.UpdateChatAsync(chat);
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var chat = await _chatRepository.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return View(chat);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chat = await _chatRepository.GetChatByIdAsync(id);
            if (chat != null)
            {
                await _chatRepository.DeleteChatAsync(id);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }

}

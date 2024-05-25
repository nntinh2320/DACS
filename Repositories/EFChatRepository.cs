using DACS.Data;
using DACS.Models;
using Microsoft.EntityFrameworkCore;

namespace DACS.Repositories
{
    public class EFChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public EFChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        // Tương tự như EFProductRepository, nhưng cho Category
        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            return await _context.Chats
                .ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int id)
        {
            return await _context.Chats
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateChatAsync(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using DACS.Data;
using DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DACS.Repositories
{
    public class EFCTCacChatRepository : ICTCacChatRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCTCacChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CTCacChat>> GetAllCTCacChats()
        {
            return await _context.CTCacChats.ToListAsync();
        }

        public async Task<CTCacChat> GetCTCacChatById(int id)
        {
            return await _context.CTCacChats.FindAsync(id);
        }
        
        public async Task AddCTCacChats(List<CTCacChat> cTCacChats)
        {
            await _context.CTCacChats.AddRangeAsync(cTCacChats);
        }
        public void UpdateCTCacChat(CTCacChat cTCacChat)
        {
            _context.CTCacChats.Update(cTCacChat);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

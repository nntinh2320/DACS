using DACS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DACS.Repositories
{
    public interface ICTCacChatRepository
    {
        Task<IEnumerable<CTCacChat>> GetAllCTCacChats();
        Task<CTCacChat> GetCTCacChatById(int id);
        Task AddCTCacChats(List<CTCacChat> cTCacChats);
        void UpdateCTCacChat(CTCacChat cTCacChat);
        Task SaveAsync();
    }
}

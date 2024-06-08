using DACS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DACS.Repositories

{
    using System.Collections.Generic;
    using DACS.Models;
    public interface IPhieuLayMauRepository
    {
        Task<IEnumerable<PhieuLayMau>> GetAllPhieuLayMaus();
        Task<PhieuLayMau> GetPhieuLayMauById(int id);
        Task<PhieuLayMau> GetPhieuLayMau(int id);
        void UpdatePhieuLayMau(PhieuLayMau phieuLayMau);
        Task AddPhieuLayMau(PhieuLayMau phieuLayMau);
        Task SaveAsync();
    }
}

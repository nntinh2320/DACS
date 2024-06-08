namespace DACS.Repositories

{
    using System.Collections.Generic;
    using DACS.Models;
    public interface IPhieuLayMauRepository
    {
        Task<IEnumerable<PhieuLayMau>> GetAllAsync();
        Task<PhieuLayMau> GetByIdAsync(int id);
        Task AddAsync(PhieuLayMau phieuLayMau);
        Task UpdateAsync(PhieuLayMau phieuLayMau);
        Task DeleteAsync(int id);
    }
}

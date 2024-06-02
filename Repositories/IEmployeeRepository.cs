using DACS.Models;

namespace DACS.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string userId);
        Task UpdateAsync(ApplicationUser employee);
        Task DeleteAsync(string userId);
    }
}

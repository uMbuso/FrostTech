using FridgeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Repositories
{
    public interface IRoleRepository
    {
        Task<bool> AddAsync(Role role);
        Task<bool> UpdateAsync(Role role);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByCodeAsync(string code);
        Task<bool> DeleteAsync(int id);
    }
}

using FridgeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Repositories
{
    public interface IUserRoleRepository
    {
        Task<bool> AddAsync(UserRole userRole);
        Task<bool> UpdateAsync(UserRole userRole);
        Task<bool> DeleteByUserIdAsync(int userId);
        Task<bool> DeleteByRoleIdAsync(int roleId);
        Task<UserRole?> GetByUserIdAsync(int userId);
    }
}

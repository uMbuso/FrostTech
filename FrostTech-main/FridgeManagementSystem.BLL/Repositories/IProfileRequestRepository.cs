using FridgeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Repositories
{
    public interface IProfileRequestRepository
    {
        Task<bool> AddAsync(ProfileRequest profile);
        Task<bool> UpdateAsync(ProfileRequest profile);
        Task<ProfileRequest?> GetByIdAsync(int id);
        Task<ProfileRequest?> GetByUserIdAsync(int userId);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByUserIdAsync(int userId);
    }
}

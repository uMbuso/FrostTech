using FridgeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Repositories
{
    public interface IFridgeAllocationRepository
    {
        Task<bool> AddAsync(FridgeAllocation allocation);
        Task<bool> UpdateAsync(FridgeAllocation allocation);
        Task<IEnumerable<FridgeAllocation>> GetAllAsync();
        Task<FridgeAllocation?> GetByIdAsync(int id);
        Task<FridgeAllocation?> GetByUserIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByUserIdAsync(int id);
    }
}

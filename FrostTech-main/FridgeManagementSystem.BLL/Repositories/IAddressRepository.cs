using FridgeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Repositories
{
    public interface IAddressRepository
    {
        Task<bool> AddAsync(Address address);
        Task<bool> UpdateAsync(Address address);
        Task<Address?> GetByIdAsync(int id);
        Task<Address?> GetByUserIdAsync(int userId);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByUserIdAsync(int userId);
    }
}

using FridgeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Repositories
{
    public interface IUserRepository
    {
        Task<bool> AddAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProfileRequest>> GetProfileRequestAsync();
        Task<ProfileRequest?> GetProfileRequestByIdAsync(int id);
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Employee>> GetEmployeeAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
    }
}

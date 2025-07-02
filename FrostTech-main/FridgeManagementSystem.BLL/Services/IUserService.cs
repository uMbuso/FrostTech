using FridgeManagementSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Services
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetUsers();
        Task<UserResponseDto> GetById(int id);
        Task<UserResponseDto> GetByEmail(string email);
        Task Create(UserCreateRequestDto newUser);
        Task Update(UserUpdateRequestDto userDto);
        Task Delete(int id);
        Task<UserResponseDto?> Login(UserLoginDto loginDto);
        Task Register(RequestAccountDto accountDto);

        Task<List<ProfileRequestDto>> GetProfileRequests();
        Task<ProfileRequestDto> GetProfileRequestById(int id);

        Task UpdateRequests(UpdateCustomerDto customerDto, string name);
        Task UpdateCustomer(UpdateCustomerDto customerDto);
        Task<CustomerDto> GetCustomeById(int id);
        Task<List<CustomerDto>> GetCustomers();
        Task<List<EmployeeDto>> GetEmployees();
        Task CreateEmployee(CreateEmployeeDto employeeDto);
        Task<EmployeeDto> GetEmployeeById(int id);
        Task UpdateEmployee(UpdateEmployeeDto employeeDto);

    }
}

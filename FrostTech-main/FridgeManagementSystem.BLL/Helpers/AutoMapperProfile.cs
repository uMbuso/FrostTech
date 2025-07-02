using AutoMapper;
using FridgeManagement.DAL.Models;
using FridgeManagementSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<UserCreateRequestDto, User>();
            CreateMap<User, UserResponseDto>();
            CreateMap<UserUpdateRequestDto, User>();
            CreateMap<UserResponseDto, UserUpdateRequestDto>();
            CreateMap<RoleCreateRequestDto, Role>();
            CreateMap<Role,  RoleResponseDto>();
            CreateMap<RoleResponseDto, RoleUpdateRequestDto>();
            CreateMap<RoleUpdateRequestDto, Role>();
            CreateMap<AddressCreateRequestDto, Address>();
            CreateMap<Customer,  CustomerDto>();
            CreateMap<User, CustomerDto>();
            CreateMap<UpdateCustomerDto, User>();
            CreateMap<CustomerDto, UpdateCustomerDto>();
            CreateMap<CreateEmployeeDto, User>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, UpdateEmployeeDto>();
            CreateMap<UpdateEmployeeDto, User>();
            CreateMap<ProfileRequest, ProfileRequestDto>();
            CreateMap<CreateProfileRequestDto, ProfileRequest>();
            CreateMap<ProfileRequest, CustomerDto>();
            CreateMap<ProfileRequestDto, UpdateCustomerDto>();
            CreateMap<FridgeAllocation, FridgeAllocationRequestDto>();
            CreateMap<CreateFridgeAllocationDto, FridgeAllocation>();
            CreateMap<CustomerDto, CreateFridgeAllocationDto>();
            CreateMap<UpdateFridgeAllocationDto, FridgeAllocation>();
            CreateMap<FridgeAllocationRequestDto, UpdateFridgeAllocationDto>();
        }
    }
}

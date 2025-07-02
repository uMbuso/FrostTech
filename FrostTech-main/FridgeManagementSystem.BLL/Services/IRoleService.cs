using FridgeManagementSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Services
{
    public interface IRoleService
    {
        Task<List<RoleResponseDto>> GetRoles();
        Task<RoleResponseDto> GetById(int id);
        Task<RoleResponseDto> GetByCode(string code);
        Task Create(RoleCreateRequestDto newRole);
        Task Update(RoleUpdateRequestDto roleDto);
        Task Delete(int id);
    }
}

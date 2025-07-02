using AutoMapper;
using FridgeManagement.DAL.Models;
using FridgeManagementSystem.BLL.DTOs;
using FridgeManagementSystem.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Services
{
    public class RoleService(IRoleRepository roleRepository, IMapper mapper) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Create(RoleCreateRequestDto newRole)
        {
            // Check role exists
            if (await _roleRepository.GetByCodeAsync(newRole.Code) != null)
            {
                throw new Exception("Role '" + newRole.Name + "' already exists");
            }

            var role = _mapper.Map<Role>(newRole);

            await _roleRepository.AddAsync(role);
        }

        public async Task Delete(int id)
        {
            await _roleRepository.DeleteAsync(id);
        }

        public async Task<RoleResponseDto> GetByCode(string code)
        {
            var role = await _roleRepository.GetByCodeAsync(code);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> GetById(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<List<RoleResponseDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllAsync();
            var result = _mapper.Map<List<RoleResponseDto>>(roles.ToList());
            return result;
        }

        public async Task Update(RoleUpdateRequestDto roleDto)
        {
            var role = await _roleRepository.GetByIdAsync(roleDto.Id);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            // Check if code has been changed
            var codeChanged = !string.IsNullOrEmpty(roleDto.Code) && role.Code != roleDto.Code;

            if (codeChanged && await _roleRepository.GetByCodeAsync(roleDto.Code) != null)
            {
                throw new Exception("Role '" + roleDto.Name + "' already exists");
            }

            _mapper.Map(roleDto, role);

            await _roleRepository.UpdateAsync(role);
        }
    }
}

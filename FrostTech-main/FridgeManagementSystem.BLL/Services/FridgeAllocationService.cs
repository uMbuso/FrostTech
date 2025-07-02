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
    public class FridgeAllocationService(IFridgeAllocationRepository fridgeAllocationRepository, IMapper mapper) : IFridgeAllocationService
    {
        private readonly IFridgeAllocationRepository _fridgeAllocationRepository = fridgeAllocationRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Create(CreateFridgeAllocationDto allocationDto)
        {
            var allocation = _mapper.Map<FridgeAllocation>(allocationDto);

            await _fridgeAllocationRepository.AddAsync(allocation);
        }

        public async Task<IEnumerable<FridgeAllocationRequestDto>> GetAll()
        {
            var allocations = await _fridgeAllocationRepository.GetAllAsync();

            var result = _mapper.Map<List<FridgeAllocationRequestDto>>(allocations.ToList());

            return result;
        }

        public async Task<FridgeAllocationRequestDto> GetById(int id)
        {
            var allocation = await _fridgeAllocationRepository.GetByIdAsync(id);

            return _mapper.Map<FridgeAllocationRequestDto>(allocation);
        }
        public async Task<FridgeAllocationRequestDto> GetByUserId(int userId)
        {
            var allocation = await _fridgeAllocationRepository.GetByUserIdAsync(userId);

            return _mapper.Map<FridgeAllocationRequestDto>(allocation);
        }


        public async Task Update(UpdateFridgeAllocationDto allocationDto)
        {
            var allocation = _mapper.Map<FridgeAllocation>(allocationDto);

            await _fridgeAllocationRepository.UpdateAsync(allocation);
        }
    }
}

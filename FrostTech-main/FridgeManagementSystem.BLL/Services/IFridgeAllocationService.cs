using FridgeManagementSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Services
{
    public interface IFridgeAllocationService
    {
        Task Create(CreateFridgeAllocationDto allocationDto);
        Task<IEnumerable<FridgeAllocationRequestDto>> GetAll();
        Task<FridgeAllocationRequestDto> GetById(int id);
        Task<FridgeAllocationRequestDto> GetByUserId(int userId);
        Task Update(UpdateFridgeAllocationDto allocationDto);
    }
}

using AutoMapper;
using FridgeManagementSystem.BLL.DTOs;
using FridgeManagementSystem.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FridgeManagementSystem.Controllers
{
    public class FridgeAllocationController(IFridgeAllocationService fridgeAllocationService, IUserService userService, IHttpContextAccessor contextAccessor, IMapper mapper) : Controller
    {
        private readonly IFridgeAllocationService _fridgeAllocationService = fridgeAllocationService;
        private readonly IUserService _userService = userService;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> Index()
        {
            var allocations = await _fridgeAllocationService.GetAll();

            return View(allocations);
        }

        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Create([FromRoute] int id)
        {
            var user = await _userService.GetCustomeById(id);

            var allocationDto = _mapper.Map<CreateFridgeAllocationDto>(user);
            allocationDto.UserId = user.Id;

            return View("Create", allocationDto);
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Create(CreateFridgeAllocationDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _fridgeAllocationService.Create(model);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var allocation = await _fridgeAllocationService.GetById(id);

            var user = await _userService.GetCustomeById(allocation.UserId);
            allocation.FirstName = user.FirstName;
            allocation.LastName = user.LastName;
            allocation.AddressLine1 = user.AddressLine1;
            allocation.AddressLine2 = user.AddressLine2;
            allocation.City = user.City;
            allocation.Province = user.Province;
            allocation.BusinessType = user.BusinessType;

            var name = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            allocation.ApprovedBy = name;
            allocation.IsApproved = true;

            var allocationDto = _mapper.Map<UpdateFridgeAllocationDto>(allocation);

            return View("Edit", allocationDto);
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Edit(UpdateFridgeAllocationDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _fridgeAllocationService.Update(model);

            return RedirectToAction("Index");
        }
    }
}

using AutoMapper;
using FridgeManagementSystem.BLL.DTOs;
using FridgeManagementSystem.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FridgeManagementSystem.Controllers
{
    public class RolesController(IRoleService roleService, IMapper mapper) : Controller
    {
        private IRoleService _roleService = roleService;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetRoles();
            return View(roles);
        }

        [Authorize(Roles = "Adminstrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Create(RoleCreateRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _roleService.Create(model);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleService.GetById(id);
            if (role == null)
            {
                throw new Exception();
            }

            var roleDto = _mapper.Map<RoleUpdateRequestDto>(role);

            return View("Edit", roleDto);
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Edit(RoleUpdateRequestDto model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _roleService.Update(model);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _roleService.Delete(id);

            return RedirectToAction("Index");

        }
    }
}

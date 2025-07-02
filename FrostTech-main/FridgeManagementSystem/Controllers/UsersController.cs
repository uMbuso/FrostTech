using AutoMapper;
using FridgeManagementSystem.BLL.DTOs;
using FridgeManagementSystem.BLL.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace FridgeManagementSystem.Controllers
{
    public class UsersController(IUserService userService, IHttpContextAccessor contextAccessor, IMapper mapper) : Controller
    {
        private IUserService _userService = userService;
        private IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IMapper _mapper = mapper;

        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsers();

            return View(users);
        }

        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [Authorize(Roles = "Adminstrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Create(UserCreateRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.Create(model);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                throw new Exception();
            }

            var userDto = _mapper.Map<UserUpdateRequestDto>(user);

            return View("Edit", userDto);
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Edit(UserUpdateRequestDto model)
        {
           
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.Update(model);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromBody] object data)
        {
            string? jsonString = data?.ToString();
            JObject? obj = jsonString != null ? JsonConvert.DeserializeObject<JObject>(jsonString) : new JObject(new { ActionView = "Index" });
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            string action = obj["ActionView"].ToString();
#           pragma warning restore CS8602 // Dereference of a possibly null reference.

            await _userService.Delete(id);

            return RedirectToAction(action);
        }

        [Authorize(Roles = "Adminstrator, Customer Liaison")]
        public async Task<IActionResult> ProfileRequests()
        {
            var requests = await _userService.GetProfileRequests();
            return View(requests);
        }

        [Authorize(Roles = "Adminstrator, Customer Liaison")]
        public async Task<IActionResult> UpdateRequests(int id)
        {
            var user = await _userService.GetProfileRequestById(id);
            if (user == null)
            {
                throw new Exception();
            }

            var userDto = _mapper.Map<UpdateCustomerDto>(user);

            return View("UpdateRequests", userDto);
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator, Customer Liaison")]
        public async Task<IActionResult> UpdateRequests(UpdateCustomerDto model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string name = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

            await _userService.UpdateRequests(model, name);

            return RedirectToAction("Customer");
        }

        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> Employee()
        {
            var employees = await _userService.GetEmployees();
            return View(employees);
        }

        [Authorize(Roles = "Adminstrator")]
        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.CreateEmployee(model);

            return RedirectToAction("Employee");
        }

        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var employee = await _userService.GetEmployeeById(id);
            var employeeDto = _mapper.Map<UpdateEmployeeDto>(employee);
            return View(employeeDto);
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> EditEmployee(UpdateEmployeeDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.UpdateEmployee(model);

            return RedirectToAction("Employee");
        }

        [Authorize(Roles = "Adminstrator, Customer Liaison")]
        public async Task<IActionResult> Customer()
        {
            var customers = await _userService.GetCustomers();
            return View(customers);
        }

        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> EditCustomer(int id)
        {
            var customer = await _userService.GetCustomeById(id);
            var customerDto = _mapper.Map<UpdateCustomerDto>(customer);
            return View(customerDto);
        }

        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> EditCustomer(UpdateCustomerDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.UpdateCustomer(model);

            return RedirectToAction("Customer");
        }
    }
}

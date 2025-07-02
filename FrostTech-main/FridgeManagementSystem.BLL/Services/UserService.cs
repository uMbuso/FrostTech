using AutoMapper;
using FridgeManagement.DAL.Models;
using FridgeManagementSystem.BLL.DTOs;
using FridgeManagementSystem.BLL.Repositories;


namespace FridgeManagementSystem.BLL.Services
{
    public class UserService(
        IUserRepository userRepository, 
        IUserRoleRepository userRoleRepository, 
        IRoleRepository roleRepository,  
        IAddressRepository addressRepository, 
        IProfileRequestRepository profileRequestRepository, 
        IMapper mapper) 
        : 
        IUserService
    {
        private IUserRepository _userRepository = userRepository;
        private IUserRoleRepository _userRoleRepository = userRoleRepository;
        private IRoleRepository _roleRepository = roleRepository;
        private IAddressRepository _addressRepository = addressRepository;
        private IProfileRequestRepository _profileRequestRepository = profileRequestRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Create(UserCreateRequestDto newUser)
        {
            // Check user exists
            if(await _userRepository.GetByEmailAsync(newUser.Email) != null)
            {
                throw new Exception("User with the email '" + newUser.Email + "' already exists");
            }

            var user = _mapper.Map<User>(newUser);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

            await _userRepository.AddAsync(user);
        }

        public async Task CreateEmployee(CreateEmployeeDto employeeDto)
        {
            // Check user exists
            if (await _userRepository.GetByEmailAsync(employeeDto.Email) != null)
            {
                throw new Exception("User with the email '" + employeeDto.Email + "' already exists");
            }

            var user = _mapper.Map<User>(employeeDto);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(employeeDto.Password);

            await _userRepository.AddAsync(user);

            var employee = await GetByEmail(employeeDto.Email);

            if (employee != null)
            {
                var addressDro = new AddressCreateRequestDto
                {
                    UserId = employee.Id,
                    AddressLine1 = employeeDto.AddressLine1,
                    AddressLine2 = employeeDto.AddressLine2,
                    City = employeeDto.City,
                    Province = employeeDto.Province
                };

                var address = _mapper.Map<Address>(addressDro);
                await _addressRepository.AddAsync(address);

                if(employeeDto.RoleId > 0)
                {
                    var userRole = new UserRole
                    {
                        UserId = employee.Id,
                        RoleId = employeeDto.RoleId
                    };

                    await _userRoleRepository.AddAsync(userRole);
                }
            }
        }

        public async Task UpdateEmployee(UpdateEmployeeDto employeeDto)
        {
            var employee = await _userRepository.GetByIdAsync(employeeDto.Id);

            // Check if email has been changed
            var emailChanged = !string.IsNullOrEmpty(employeeDto.Email) && employeeDto.Email != employee?.Email;

            if (emailChanged)
            {
                throw new Exception("User with the email '" + employeeDto.Email + "' already exists");
            }

            var user = _mapper.Map<User>(employeeDto);

            await _userRepository.UpdateAsync(user);

            var addressDro = new AddressCreateRequestDto
            {
                UserId = employeeDto.Id,
                AddressLine1 = employeeDto.AddressLine1,
                AddressLine2 = employeeDto.AddressLine2,
                City = employeeDto.City,
                Province = employeeDto.Province
            };

            var address = _mapper.Map<Address>(addressDro);
            address.Id = employeeDto.AddressId;
            await _addressRepository.UpdateAsync(address);

            // Check if role has been changed
            var userRole = await _userRoleRepository.GetByUserIdAsync(employeeDto.Id);
            var roleChanged = employeeDto.RoleId > 0 && employeeDto.RoleId != userRole?.RoleId;

            if (roleChanged)
            {
                await _userRoleRepository.DeleteByUserIdAsync(employeeDto.Id);
                await _userRoleRepository.AddAsync(new UserRole{UserId = employeeDto.Id, RoleId = employeeDto.RoleId});
            }
        }

        public async Task<List<UserResponseDto>> GetUsers()
        {
           var users = await _userRepository.GetAllAsync();
           var result = _mapper.Map<List<UserResponseDto>>(users.ToList());
            return result;
        }
        public async Task<List<ProfileRequestDto>> GetProfileRequests()
        {
            var users = await _userRepository.GetProfileRequestAsync();
            var result = _mapper.Map<List<ProfileRequestDto>>(users.ToList());
            return result;
        }

        public async Task<List<CustomerDto>> GetCustomers()
        {
            var users = await _userRepository.GetCustomersAsync();
            var result = _mapper.Map<List<CustomerDto>>(users.ToList());
            return result;
        }

        public async Task<List<EmployeeDto>> GetEmployees()
        {
            var users = await _userRepository.GetEmployeeAsync();
            var result = _mapper.Map<List<EmployeeDto>>(users.ToList());
            return result;
        }

        public async Task<UserResponseDto> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if(user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            var user = await _userRepository.GetEmployeeByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return _mapper.Map<EmployeeDto>(user);
        }

        public async Task<ProfileRequestDto> GetProfileRequestById(int id)
        {
            var user = await _userRepository.GetProfileRequestByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return _mapper.Map<ProfileRequestDto>(user);
        }

        public async Task<CustomerDto> GetCustomeById(int id)
        {
            var user = await _userRepository.GetCustomerByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return _mapper.Map<CustomerDto>(user);
        }

        public async Task<UserResponseDto> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task Update(UserUpdateRequestDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(userDto.Id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Check if email has been changed
            var emailChanged = !string.IsNullOrEmpty(userDto.Email) && user.Email != userDto.Email;

            if (emailChanged && await _userRepository.GetByEmailAsync(userDto.Email) != null)
            {
                throw new Exception("User with the email '" + userDto.Email + "' already exists");
            }

            _mapper.Map(userDto, user);

            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdateRequests(UpdateCustomerDto customerDto, string name)
        {
            var customer = await _userRepository.GetByIdAsync(customerDto.Id);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            // Check if email has been changed
            var emailChanged = !string.IsNullOrEmpty(customerDto.Email) && customer.Email != customerDto.Email;

            if (emailChanged && await _userRepository.GetByEmailAsync(customerDto.Email) != null)
            {
                throw new Exception("User with the email '" + customerDto.Email + "' already exists");
            }

            var user = _mapper.Map<User>(customerDto);
            user.IsProfileRequest = false;

            await _userRepository.UpdateAsync(user);

            
            var profileDto = await _profileRequestRepository.GetByUserIdAsync(user.Id);

            if(name != null)
            {
                if (profileDto != null)
                {
                    profileDto.ApprovedBy = name;
                    profileDto.IsApproved = true;

                    await _profileRequestRepository.UpdateAsync(profileDto);
                }
            }
        }

        public async Task UpdateCustomer(UpdateCustomerDto customerDto)
        {
            var customer = await _userRepository.GetByIdAsync(customerDto.Id);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            // Check if email has been changed
            var emailChanged = !string.IsNullOrEmpty(customerDto.Email) && customer.Email != customerDto.Email;

            if (emailChanged && await _userRepository.GetByEmailAsync(customerDto.Email) != null)
            {
                throw new Exception("User with the email '" + customerDto.Email + "' already exists");
            }

            var user = _mapper.Map<User>(customerDto);
            user.IsProfileRequest = false;

            await _userRepository.UpdateAsync(user);

            var addressDto = await _addressRepository.GetByUserIdAsync(customerDto.Id);

            if(addressDto != null)
            {
                addressDto.AddressLine1 = customerDto.AddressLine1;
                addressDto.AddressLine2 = customerDto.AddressLine2;
                addressDto.City = customerDto.City;
                addressDto.Province = customerDto.Province;
                var address = _mapper.Map<Address>(addressDto);
                await _addressRepository.UpdateAsync(address);
            }
        }

        public async Task Delete(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<UserResponseDto?> Login(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if(user != null)
            {
                var isMatch = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

                if (isMatch)
                {
                    var userDto = _mapper.Map<UserResponseDto>(user);
                    var userRole = await _userRoleRepository.GetByUserIdAsync(user.Id);
                    var role = userRole != null ? await _roleRepository.GetByIdAsync(userRole.RoleId) : null;
                    
                    if (role != null)
                    {
                        userDto.RoleCode = role.Code;
                        userDto.RoleName = role.Name;
                    }

                    return userDto;
                }
                
            }

            return null;
        }

        public async Task Register(RequestAccountDto accountDto)
        {
            var userDto = new UserCreateRequestDto
            {
                FirstName = accountDto.FirstName,
                LastName = accountDto.LastName,
                Email = accountDto.Email,
                PhoneNumber = accountDto.PhoneNumber,
                Password = accountDto.Password,
                ConfirmPassword = accountDto.ConfirmPassword,
                IsProfileRequest = true
            };

            await Create(userDto);

            var createdUser = await GetByEmail(accountDto.Email);

            if (createdUser != null)
            {
                var addressDto = new AddressCreateRequestDto
                {
                    UserId = createdUser.Id,
                    AddressLine1 = accountDto.AddressLine1,
                    AddressLine2 = accountDto.AddressLine2,
                    City = accountDto.City,
                    Province = accountDto.Province
                };

                var address = _mapper.Map<Address>(addressDto);
                await _addressRepository.AddAsync(address);

                var profileRequestDto = new CreateProfileRequestDto
                {
                   ApprovedBy = null,
                   IsApproved = false
                };

                var profile = _mapper.Map<ProfileRequest>(profileRequestDto);
                profile.Id = createdUser.Id;

                await _profileRequestRepository.AddAsync(profile);
            }
        }
    }
}

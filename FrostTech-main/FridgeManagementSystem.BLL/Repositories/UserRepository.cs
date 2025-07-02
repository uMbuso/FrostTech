using FridgeManagement.DAL;
using FridgeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.Repositories
{
    public class UserRepository(DbContext db, IUserRoleRepository userRoleRepository, IAddressRepository addressRepository, IProfileRequestRepository profileRequestRepository, IFridgeAllocationRepository allocationRepository) : IUserRepository
    {
        private readonly DbContext _db = db;
        private readonly IUserRoleRepository _userRoleRepository = userRoleRepository;
        private readonly IAddressRepository _addressRepository = addressRepository;
        private readonly IProfileRequestRepository _profileRequestRepository = profileRequestRepository;
        private readonly IFridgeAllocationRepository _allocationRepository = allocationRepository;

        public async Task<bool> AddAsync(User user)
        {
            try
            {
                string sql = @"
                    INSERT INTO Users (FirstName, LastName, Email, PhoneNumber, IdentificationNo, BusinessType, PasswordHash, IsProfileRequest)
                    VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @IdentificationNo, @BusinessType, @PasswordHash, @IsProfileRequest)"
                ;

                await _db.SaveData(sql, user, CommandType.Text);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                string sql = @"
                    UPDATE Users
                    SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber, IdentificationNo = @IdentificationNo, BusinessType = @BusinessType, IsProfileRequest = @IsProfileRequest
                    WHERE Id ='" + user.Id + "'"
                ;

                await _db.SaveData(sql, new {user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.IdentificationNo, user.BusinessType, user.IsProfileRequest}, CommandType.Text);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            string sql = @"SELECT * FROM Users";

            return await _db.GetData<User, dynamic>(sql, default!, CommandType.Text);
        }

        public async Task<IEnumerable<ProfileRequest>> GetProfileRequestAsync()
        {
            string sql = @"
                SELECT u.Id, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.BusinessType, a.Id AS AddressId, a.AddressLine1, a.AddressLine2, a.City, a.Province 
                FROM Users u
                JOIN Addresses a ON u.Id = a.UserId
                WHERE IsProfileRequest = 1
            ";

            return await _db.GetData<ProfileRequest, dynamic>(sql, default!, CommandType.Text);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            string sql = @"
                SELECT u.Id, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.BusinessType, a.Id AS AddressId, a.AddressLine1, a.AddressLine2, a.City, a.Province, pr.IsApproved 
                FROM Users u
                LEFT JOIN Addresses a ON u.Id = a.UserId
                LEFT JOIN ProfileRequests pr ON u.Id = pr.UserId
                WHERE (IsProfileRequest = 0 AND u.BusinessType IS NOT NULL) OR IsProfileRequest = 1
            ";

            return await _db.GetData<Customer, dynamic>(sql, default!, CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> GetEmployeeAsync()
        {
            string sql = @"
                SELECT u.Id, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.IdentificationNo, r.Id AS RoleId, r.Name AS Role, a.Id AS AddressId, a.AddressLine1, a.AddressLine2, a.City, a.Province 
                FROM Users u
                LEFT JOIN UserRoles ur ON u.Id = ur.UserId
                LEFT JOIN Roles r ON ur.RoleId = r.Id
                LEFT JOIN Addresses a ON u.Id = a.UserId
                WHERE IsProfileRequest = 0 AND u.IdentificationNo IS NOT NULL
            ";

            return await _db.GetData<Employee, dynamic>(sql, default!, CommandType.Text);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            string sql = @"
                SELECT u.Id, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.IdentificationNo, r.Id AS RoleId, r.Name AS Role, a.Id AS AddressId, a.AddressLine1, a.AddressLine2, a.City, a.Province 
                FROM Users u
                LEFT JOIN UserRoles ur ON u.Id = ur.UserId
                LEFT JOIN Roles r ON ur.RoleId = r.Id
                LEFT JOIN Addresses a ON u.Id = a.UserId
                WHERE u.Id = @Id
            ";

            var result = await _db.GetData<Employee, dynamic>(sql, new { Id = id }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<ProfileRequest?> GetProfileRequestByIdAsync(int id)
        {
            string sql = @"
                SELECT u.Id, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.BusinessType, a.Id AS AddressId, a.AddressLine1, a.AddressLine2, a.City, a.Province 
                FROM Users u
                LEFT JOIN Addresses a ON u.Id = a.UserId
                WHERE IsProfileRequest = 1 AND u.Id = @Id
            ";

            var result = await _db.GetData<ProfileRequest, dynamic>(sql, new { Id = id }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            string sql = @"
                SELECT u.Id, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.BusinessType, a.Id AS AddressId, a.AddressLine1, a.AddressLine2, a.City, a.Province 
                FROM Users u
                LEFT JOIN Addresses a ON u.Id = a.UserId
                WHERE u.Id = @Id
            ";

            var result = await _db.GetData<Customer, dynamic>(sql, new { Id = id }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            string sql = @"SELECT * FROM Users WHERE Id = @Id";

            var result = await _db.GetData<User, dynamic>(sql, new { Id = id }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            string sql = @"SELECT * FROM Users WHERE Email = @Email";

            var result = await _db.GetData<User, dynamic>(sql, new { Email = email }, CommandType.Text);

            return result.FirstOrDefault();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                string sql = @"DELETE FROM Users WHERE Id = @Id";

                bool isRolesDeleted = await _userRoleRepository.DeleteByUserIdAsync(id);
                bool isAddressDeleted = await _addressRepository.DeleteByUserIdAsync(id);
                bool isProfileDeleted = await _profileRequestRepository.DeleteByUserIdAsync(id);
                bool isFridgeAllocationDeleted = await _allocationRepository.DeleteByUserIdAsync(id);

                if(isRolesDeleted && isAddressDeleted && isProfileDeleted && isFridgeAllocationDeleted)
                {
                    var result = await _db.SaveData(sql, new { Id = id }, CommandType.Text);

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}

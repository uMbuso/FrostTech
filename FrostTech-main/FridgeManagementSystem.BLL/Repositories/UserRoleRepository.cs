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
    public class UserRoleRepository(DbContext db) : IUserRoleRepository
    {
        private readonly DbContext _db = db;

        public async Task<bool> AddAsync(UserRole userRole)
        {
            try
            {
                string sql = @"
                    INSERT INTO UserRoles (UserId, RoleId)
                    VALUES (@UserId, @RoleId)"
                ;

                await _db.SaveData(sql, userRole, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteByUserIdAsync(int userId)
        {
            try
            {
                string sql = @"DELETE FROM UserRoles WHERE UserId = @UserId";

                var result = await _db.SaveData(sql, new { UserId = userId }, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteByRoleIdAsync(int roleId)
        {
            try
            {
                string sql = @"DELETE FROM UserRoles WHERE RoleId = @RoleId";

                var result = await _db.SaveData(sql, new { RoleId = roleId }, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(UserRole userRole)
        {
            try
            {
                string sql = @"
                    UPDATE UserRoles
                    SET UserId = @UserId, RoleId = @RoleId
                    WHERE Id ='" + userRole.Id + "'"
                ;

                await _db.SaveData(sql, new { userRole.UserId, userRole.RoleId }, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UserRole?> GetByUserIdAsync(int userId)
        {
            string sql = @"SELECT * FROM UserRoles WHERE UserId = @UserId";

            var result = await _db.GetData<UserRole, dynamic>(sql, new { UserId = userId }, CommandType.Text);

            return result.FirstOrDefault();
        }
    }
}

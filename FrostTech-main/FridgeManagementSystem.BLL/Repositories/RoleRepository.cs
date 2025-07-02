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
    public class RoleRepository(DbContext db, IUserRoleRepository userRoleRepository) : IRoleRepository
    {
        private readonly DbContext _db = db;
        private readonly IUserRoleRepository _userRoleRepository = userRoleRepository;
        public async Task<bool> AddAsync(Role role)
        {
            try
            {
                string sql = @"
                    INSERT INTO Roles (Code, Name)
                    VALUES (@Code, @Name)"
                ;

                await _db.SaveData(sql, role, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Role role)
        {
            try
            {
                string sql = @"
                    UPDATE Roles
                    SET Code = @Code, Name = @Name
                    WHERE Id ='" + role.Id + "'"
                ;

                await _db.SaveData(sql, new { role.Code, role.Name}, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            string sql = @"SELECT * FROM Roles";

            return await _db.GetData<Role, dynamic>(sql, default!, CommandType.Text);
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            string sql = @"SELECT * FROM Roles WHERE Id = @Id";

            var result = await _db.GetData<Role, dynamic>(sql, new { Id = id }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<Role?> GetByCodeAsync(string code)
        {
            string sql = @"SELECT * FROM Roles WHERE Code = @Code";

            var result = await _db.GetData<Role, dynamic>(sql, new { Code = code }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                string sql = @"DELETE FROM Roles WHERE Id = @Id";

                bool isRolesDeleted = await _userRoleRepository.DeleteByUserIdAsync(id);

                if(isRolesDeleted)
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

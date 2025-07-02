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
    public class FridgeAllocationRepository(DbContext db) : IFridgeAllocationRepository
    {
        private readonly DbContext _db = db;

        public async Task<bool> AddAsync(FridgeAllocation allocation)
        {
            try
            {
                string sql = @"
                    INSERT INTO FridgeAllocations (UserId, AllocationDate, Action, FridgeType, MaintenanceDate, ApprovedBy, IsApproved)
                    VALUES (@UserId, @AllocationDate, @Action, @FridgeType, @MaintenanceDate, @ApprovedBy, @IsApproved)"
                ;

                await _db.SaveData(sql, new {allocation.UserId, allocation.AllocationDate, allocation.Action, allocation.FridgeType, allocation.MaintenanceDate, allocation.ApprovedBy, allocation.IsApproved }, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(FridgeAllocation allocation)
        {
            try
            {
                string sql = @"
                    UPDATE FridgeAllocations
                    SET UserId = @UserId, AllocationDate = @AllocationDate, Action = @Action, FridgeType = @FridgeType, MaintenanceDate = @MaintenanceDate, ApprovedBy = @ApprovedBy, IsApproved = @IsApproved
                    WHERE Id ='" + allocation.Id + "'"
                ;

                await _db.SaveData(sql, new { allocation.UserId, allocation.AllocationDate, allocation.Action, allocation.FridgeType, allocation.MaintenanceDate, allocation.ApprovedBy, allocation.IsApproved }, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<FridgeAllocation>> GetAllAsync()
        {
            string sql = @"
                SELECT fa.*, u.FirstName, u.LastName, u.BusinessType, a.Id AS AddressId, a.AddressLine1, a.AddressLine2, a.City, a.Province 
                FROM FridgeAllocations fa
                JOIN Users u ON fa.UserId = u.id
                LEFT JOIN Addresses a ON u.Id = a.UserId
            ";

            return await _db.GetData<FridgeAllocation, dynamic>(sql, default!, CommandType.Text);
        }

        public async Task<FridgeAllocation?> GetByIdAsync(int id)
        {
            string sql = @"SELECT * FROM FridgeAllocations WHERE Id = @Id";

            var result = await _db.GetData<FridgeAllocation, dynamic>(sql, new { Id = id }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<FridgeAllocation?> GetByUserIdAsync(int id)
        {
            string sql = @"SELECT * FROM FridgeAllocations WHERE UserId = @Id";

            var result = await _db.GetData<FridgeAllocation, dynamic>(sql, new { Id = id }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                string sql = @"DELETE FROM FridgeAllocations WHERE Id = @Id";


                var result = await _db.SaveData(sql, new { Id = id }, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> DeleteByUserIdAsync(int id)
        {
            try
            {
                string sql = @"DELETE FROM FridgeAllocations WHERE UserId = @Id";


                var result = await _db.SaveData(sql, new { Id = id }, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}

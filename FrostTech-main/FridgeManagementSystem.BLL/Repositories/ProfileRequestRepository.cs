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
    public class ProfileRequestRepository(DbContext db): IProfileRequestRepository
    {
        private readonly DbContext _db = db;

        public async Task<bool> AddAsync(ProfileRequest profile)
        {
            try
            {
                string sql = @"
                    INSERT INTO ProfileRequests (UserId, ApprovedBy, IsApproved)
                    VALUES (@UserId, @ApprovedBy, @IsApproved)"
                ;

                await _db.SaveData(sql, new { UserId = profile.Id, profile.ApprovedBy, profile.IsApproved}, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {

                string sql = @"DELETE FROM ProfileRequest WHERE Id = @Id";

                var result = await _db.SaveData(sql, new { Id = id }, CommandType.Text);

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

                string sql = @"DELETE FROM ProfileRequests WHERE UserId = @UserId";

                var result = await _db.SaveData(sql, new { UserId = userId }, CommandType.Text);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProfileRequest?> GetByIdAsync(int id)
        {
            string sql = @"SELECT * FROM ProfileRequests WHERE Id = @Id";

            var result = await _db.GetData<ProfileRequest, dynamic>(sql, new { Id = id }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<ProfileRequest?> GetByUserIdAsync(int userId)
        {
            string sql = @"SELECT Id AS ProfileId, UserId AS Id, ApprovedBy, IsApproved FROM ProfileRequests WHERE UserId = @UserId";

            var result = await _db.GetData<ProfileRequest, dynamic>(sql, new { UserId = userId }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateAsync(ProfileRequest profile)
        {
            try
            {
                string sql = @"
                    UPDATE ProfileRequests
                    SET UserId = @UserId, ApprovedBy = @ApprovedBy, IsApproved = @IsApproved
                    WHERE Id ='" + profile.ProfileId + "'"
                ;

                await _db.SaveData(sql, new {Id = profile.ProfileId, UserId = profile.Id, profile.ApprovedBy, profile.IsApproved }, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

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
    public class AddressRepository(DbContext db) : IAddressRepository
    {
        private readonly DbContext _db = db;

        public async Task<bool> AddAsync(Address address)
        {
            try
            {
                string sql = @"
                    INSERT INTO Addresses (UserId, AddressLine1, AddressLine2, City, Province)
                    VALUES (@UserId, @AddressLine1, @AddressLine2, @City, @Province)"
                ;

                await _db.SaveData(sql, address, CommandType.Text);

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

                string sql = @"DELETE FROM Addresses WHERE Id = @Id";

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

                string sql = @"DELETE FROM Addresses WHERE UserId = @UserId";

                var result = await _db.SaveData(sql, new { UserId = userId }, CommandType.Text);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            string sql = @"SELECT * FROM Addresses WHERE Id = @Id";

            var result = await _db.GetData<Address, dynamic>(sql, new { Id = id }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<Address?> GetByUserIdAsync(int userId)
        {
            string sql = @"SELECT * FROM Addresses WHERE UserId = @UserId";

            var result = await _db.GetData<Address, dynamic>(sql, new { UserId = userId }, CommandType.Text);

            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateAsync(Address address)
        {
            try
            {
                string sql = @"
                    UPDATE Addresses
                    SET UserId = @UserId, AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, City = @City, Province = @Province
                    WHERE Id ='" + address.Id + "'"
                ;

                await _db.SaveData(sql, address, CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

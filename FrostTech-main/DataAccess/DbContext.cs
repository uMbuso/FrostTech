using Dapper;
using FridgeManagement.DAL.Models;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace FridgeManagement.DAL
{
    public class DbContext(IOptions<DbSettings> dbSettings)
    {
        private DbSettings _dbSettings = dbSettings.Value;

        public IDbConnection CreateConnection()
        {
            var connectionString = $"Server={_dbSettings.Server}; Database={_dbSettings.Database}; User Id={_dbSettings.UserId}; Password={_dbSettings.Password};";
            return new SqlConnection(connectionString);
        }

        public async Task Init()
        {
            await _initDb();
            await _initTables();
            await _seedUserDb();
            await _seedRoleDb();
            await _seedUserRoleDb();
        }

        private async Task _initDb()
        {
            // Create database if it doesn't exist
            var connectionString = $"Server={_dbSettings.Server}; Database=master; User Id={_dbSettings.UserId}; Password={_dbSettings.Password};";
            using var connection = new SqlConnection(connectionString);
            var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{_dbSettings.Database}') CREATE DATABASE [{_dbSettings.Database}];";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initTables()
        {
            // Create tables if they don't exist
            using var connection = CreateConnection();
            await _initUsers();
            await _initRoles();
            await _initUserRoles();
            await _initAddresses();
            await _initProfileRequest();
            await _initFridgeAllocation();
            await _initFaults();
            await _initSuppliers();
            await _initPurchases();

            async Task _initUsers()
            {
                var sql = """
                    IF OBJECT_ID('Users', 'U') IS NULL
                    CREATE TABLE Users (
                        Id INT NOT NULL PRIMARY KEY IDENTITY,
                        FirstName NVARCHAR(50) NOT NULL,
                        LastName NVARCHAR(50) NOT NULL,
                        Email NVARCHAR(150) NOT NULL,
                        PhoneNumber NVARCHAR(150) NULL,
                        PasswordHash NVARCHAR(500) NOT NULL,
                        IdentificationNo NVARCHAR(15) NULL,
                        BusinessType NVARCHAR(100) NULL,
                        IsProfileRequest BIT NOT NULL
                    );

                    IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE OBJECT_ID = OBJECT_ID('DF_Users_IsProfileRequest') AND PARENT_OBJECT_ID = OBJECT_ID('Users'))
                    ALTER TABLE Users ADD CONSTRAINT DF_Users_IsProfileRequest DEFAULT 0 FOR IsProfileRequest;
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initRoles()
            {
                var sql = """
                    IF OBJECT_ID('Roles', 'U') IS NULL
                    CREATE TABLE Roles (
                        Id INT NOT NULL PRIMARY KEY IDENTITY,
                        Code NVARCHAR(50) NOT NULL,
                        Name NVARCHAR(100) NOT NULL,
                    );
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initUserRoles()
            {
                var sql = """
                    IF OBJECT_ID('UserRoles', 'U') IS NULL
                    CREATE TABLE UserRoles (
                        Id INT NOT NULL PRIMARY KEY IDENTITY,
                        UserId INT NOT NULL,
                        RoleId INT NOT NULL,
                    );

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE OBJECT_ID = OBJECT_ID('FK_UserRoles_Users_UserId') AND PARENT_OBJECT_ID = OBJECT_ID('UserRoles'))
                    ALTER TABLE UserRoles WITH CHECK ADD  CONSTRAINT FK_UserRoles_Users_UserId FOREIGN KEY(UserId) REFERENCES Users (Id);

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE OBJECT_ID = OBJECT_ID('FK_UserRoles_Roles_RoleId') AND PARENT_OBJECT_ID = OBJECT_ID('UserRoles'))
                    ALTER TABLE UserRoles  WITH CHECK ADD  CONSTRAINT FK_UserRoles_Roles_RoleId FOREIGN KEY(RoleId) REFERENCES Roles (Id);
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initAddresses()
            {
                var sql = """
                    IF OBJECT_ID('Addresses', 'U') IS NULL
                    CREATE TABLE Addresses (
                        Id INT NOT NULL PRIMARY KEY IDENTITY,
                        UserId INT NOT NULL,
                        AddressLine1 NVARCHAR(50) NOT NULL,
                        AddressLine2 NVARCHAR(50) NULL,
                        City NVARCHAR(50) NOT NULL,
                        Province NVARCHAR(50) NOT NULL,
                    );

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE OBJECT_ID = OBJECT_ID('FK_Addresses_User_UserId') AND PARENT_OBJECT_ID = OBJECT_ID('Addresses'))
                    ALTER TABLE Addresses WITH CHECK ADD  CONSTRAINT FK_Addresses_User_UserId FOREIGN KEY(UserId) REFERENCES Users (Id);
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initProfileRequest()
            {
                var sql = """
                    IF OBJECT_ID('ProfileRequests', 'U') IS NULL
                    CREATE TABLE ProfileRequests (
                        Id INT NOT NULL PRIMARY KEY IDENTITY,
                        UserId INT NOT NULL,
                        ApprovedBy NVARCHAR(100) NULL,
                        IsApproved BIT NOT NULL,
                    );

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE OBJECT_ID = OBJECT_ID('FK_ProfileRequests_User_UserId') AND PARENT_OBJECT_ID = OBJECT_ID('ProfileRequests'))
                    ALTER TABLE ProfileRequests WITH CHECK ADD  CONSTRAINT FK_ProfileRequests_User_UserId FOREIGN KEY(UserId) REFERENCES Users (Id);
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initFridgeAllocation()
            {
                var sql = """
                    IF OBJECT_ID('FridgeAllocations', 'U') IS NULL
                    CREATE TABLE FridgeAllocations (
                        Id INT NOT NULL PRIMARY KEY IDENTITY,
                        UserId INT NOT NULL,
                        AllocationDate DATETIME NOT NULL,
                        Action NVARCHAR(100) NULL,
                        FridgeType NVARCHAR(100) NULL,
                        MaintenanceDate DATETIME NULL,
                        ApprovedBy NVARCHAR(100) NULL,
                        IsApproved BIT NOT NULL,
                    );

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE OBJECT_ID = OBJECT_ID('FK_FridgeAllocations_User_UserId') AND PARENT_OBJECT_ID = OBJECT_ID('FridgeAllocations'))
                    ALTER TABLE FridgeAllocations WITH CHECK ADD  CONSTRAINT FK_FridgeAllocations_User_UserId FOREIGN KEY(UserId) REFERENCES Users (Id);
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initFaults()
            {
                var sql = """
                    IF OBJECT_ID('Faults', 'U') IS NULL
                    CREATE TABLE Faults (
                        Id INT NOT NULL PRIMARY KEY IDENTITY,
                        UserId INT NOT NULL,
                        FaultNumber NVARCHAR(10) NOT NULL,
                        RepairDate DATETIME NULL,
                        FridgeType NVARCHAR(100) NULL,
                        FaultDescription NVARCHAR(500) NULL,
                        RepairDescription NVARCHAR(500) NULL,
                        RepairStatus NVARCHAR(50) NULL,
                        IsScrap BIT NOT NULL
                    );

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE OBJECT_ID = OBJECT_ID('FK_Faults_User_UserId') AND PARENT_OBJECT_ID = OBJECT_ID('Faults'))
                    ALTER TABLE Faults WITH CHECK ADD  CONSTRAINT FK_Faults_User_UserId FOREIGN KEY(UserId) REFERENCES Users (Id);
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initSuppliers()
            {
                var sql = """
                    IF OBJECT_ID('Suppliers', 'U') IS NULL
                    CREATE TABLE Suppliers (
                        Id INT NOT NULL PRIMARY KEY IDENTITY,
                        Name NVARCHAR(100) NOT NULL,
                        ContactPerson NVARCHAR(100) NOT NULL,
                        AccountNumber NVARCHAR(100) NOT NULL,
                        Email NVARCHAR(100) NOT NULL,
                        PhoneNumber NVARCHAR(15) NOT NULL,
                        PhysicalAddress NVARCHAR(250) NOT NULL,
                    );
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initPurchases()
            {
                var sql = """
                    IF OBJECT_ID('Purchases', 'U') IS NULL
                    CREATE TABLE Purchases (
                        Id INT NOT NULL PRIMARY KEY IDENTITY,
                        Item NVARCHAR(100) NOT NULL,
                        Quantity INT NOT NULL,
                        SupplierId INT NOT NULL,
                        Status NVARCHAR(100) NOT NULL,
                    );

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE OBJECT_ID = OBJECT_ID('FK_Purchases_Supplier_SupplierId') AND PARENT_OBJECT_ID = OBJECT_ID('Purchases'))
                    ALTER TABLE Purchases WITH CHECK ADD  CONSTRAINT FK_Purchases_Supplier_SupplierId FOREIGN KEY(SupplierId) REFERENCES Suppliers (Id);
                """;
                await connection.ExecuteAsync(sql);
            }
        }

        public async Task<IEnumerable<T>> GetData<T, P>(string query, P parameters, CommandType commandType)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                return await dbConnection.QueryAsync<T>(query, parameters, commandType: commandType);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> SaveData<T>(string query, T parameters, CommandType commandType)
        {
            try
            {
                using IDbConnection connection = CreateConnection();
                await connection.ExecuteAsync(query, parameters, commandType: commandType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task _seedUserDb()
        {
            //Seed admin user
            var user = await GetData<User, dynamic>(@"SELECT * FROM Users WHERE FirstName = 'System' AND LastName = 'Administrator' AND Email = 'admin@fms.com'", new { }, CommandType.Text);

            if(user.FirstOrDefault() == null) {
                var sql = @"
                    INSERT INTO Users (FirstName, LastName, Email, PhoneNumber, PasswordHash)
                    VALUES ('System', 'Administrator', 'admin@fms.com', NULL, @PasswordHash)"
                ;

                var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin@2024");

                await SaveData(sql, new { PasswordHash = passwordHash }, CommandType.Text);
            }
        }

        private async Task _seedRoleDb()
        {
            //Seed admin role
            var role = await GetData<Role, dynamic>(@"SELECT * FROM Roles WHERE Code = 'admin'", new { }, CommandType.Text);

            if (role.FirstOrDefault() == null)
            {
                await SaveData(@"INSERT INTO Roles (Code,Name) VALUES ('admin','Adminstrator')", new { }, CommandType.Text);
            }
        }

        private async Task _seedUserRoleDb()
        {
            //Seed admin user role
            var user = await GetData<User, dynamic>(@"SELECT * FROM Users WHERE FirstName = 'System' AND LastName = 'Administrator' AND Email = 'admin@fms.com'", new { }, CommandType.Text);
            var role = await GetData<Role, dynamic>(@"SELECT * FROM Roles WHERE Code = 'admin'", new { }, CommandType.Text);

            var userRole = await GetData<UserRole, dynamic>(@"SELECT * FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId", new { UserId = user.FirstOrDefault()?.Id, RoleId = role.FirstOrDefault()?.Id }, CommandType.Text);

            if (user.FirstOrDefault() != null && role.FirstOrDefault() != null && userRole.FirstOrDefault() == null)
            {
                await SaveData(@"INSERT INTO UserRoles (UserId,RoleId) VALUES (@UserId,@RoleId)", new { UserId = user.FirstOrDefault()?.Id, RoleId = role.FirstOrDefault()?.Id }, CommandType.Text);
            }
        }

    }
}

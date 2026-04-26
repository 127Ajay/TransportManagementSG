using System.Data;
using Dapper;

namespace TransportManagementSG.Application.Database;

public class DBInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DBInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitalizeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        await CreateTables(connection);
        await CreateStoredProcedures(connection);
        
        //Seed Data
        await SeedRoles(connection);
    }

    private async Task CreateTables(IDbConnection connection)
    {
        var sql = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Role' AND xtype='U')
            BEGIN
                CREATE TABLE Role (
                    RoleId INT PRIMARY KEY IDENTITY,
                    RoleName NVARCHAR(100),
                    IsActive BIT
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='User' AND xtype='U')
            BEGIN
                CREATE TABLE [User] (
                    UserId INT PRIMARY KEY IDENTITY,
                    FirstName NVARCHAR(100),
                    LastName NVARCHAR(100),
                    Email NVARCHAR(150) UNIQUE,
                    PhoneNumber NVARCHAR(20),
                    RoleId INT,
                    IsActive BIT,
                    LoginPassword NVARCHAR(MAX),
                    LastLoginDate DATETIME,
                    CreatedBy NVARCHAR(100),
                    CreatedDate DATETIME,
                    ModifiedBy NVARCHAR(100),
                    ModifiedDate DATETIME,
                    FOREIGN KEY (RoleId) REFERENCES Role(RoleId)
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='VehicleType' AND xtype='U')
            BEGIN
                CREATE TABLE VehicleType (
                    VehicleTypeID INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(100),
                    WheelCount INT,
                    LengthFeet DECIMAL(10,2),
                    IsActive BIT,
                    CreatedBy NVARCHAR(100),
                    CreatedDate DATETIME,
                    ModifiedBy NVARCHAR(100),
                    ModifiedDate DATETIME
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='State' AND xtype='U')
            BEGIN
                CREATE TABLE State (
                    StateID INT PRIMARY KEY IDENTITY,
                    StateCode NVARCHAR(10),
                    StateName NVARCHAR(100),
                    IsActive BIT,
                    CreatedBy NVARCHAR(100),
                    CreatedDate DATETIME,
                    ModifiedBy NVARCHAR(100),
                    ModifiedDate DATETIME
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TransportStatus' AND xtype='U')
            BEGIN
                CREATE TABLE TransportStatus (
                    TransportStatusId INT PRIMARY KEY IDENTITY,
                    StatusName NVARCHAR(100),
                    IsActive BIT,
                    CreatedBy NVARCHAR(100),
                    CreatedDate DATETIME,
                    ModifiedBy NVARCHAR(100),
                    ModifiedDate DATETIME
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Vehicle' AND xtype='U')
            BEGIN
                CREATE TABLE Vehicle (
                    VehicleID INT PRIMARY KEY IDENTITY,
                    VehicleNumber NVARCHAR(50) UNIQUE,
                    StateID INT,
                    VehicleTypeID INT,
                    ModelNumber NVARCHAR(100),
                    RegistrationYear INT,
                    TransportStatusId INT,
                    IsActive BIT,
                    CreatedBy NVARCHAR(100),
                    CreatedDate DATETIME,
                    ModifiedBy NVARCHAR(100),
                    ModifiedDate DATETIME,
                    FOREIGN KEY (StateID) REFERENCES State(StateID),
                    FOREIGN KEY (VehicleTypeID) REFERENCES VehicleType(VehicleTypeID),
                    FOREIGN KEY (TransportStatusId) REFERENCES TransportStatus(TransportStatusId)
                );
            END
            ";

        await connection.ExecuteAsync(sql);
    }
    
    private async Task CreateStoredProcedures(IDbConnection connection)
    {
        // 🔹 Create User SP
        var sql = @"
            CREATE OR ALTER PROCEDURE usp_GetAllRoles
            AS
            BEGIN
                SET NOCOUNT ON;

                SELECT 
                    RoleId,
                    RoleName,
                    IsActive
                FROM Role
                WHERE IsActive = 1
                ORDER BY RoleName;
            END";
        
        await connection.ExecuteAsync(sql);

        var sql2 = @"
            CREATE OR ALTER PROCEDURE usp_GetRolesByName
                @RoleName NVARCHAR(100)
            AS
            BEGIN
                SET NOCOUNT ON;

                SELECT 
                    RoleId,
                    RoleName,
                    IsActive
                FROM Role
                WHERE 
                    IsActive = 1
                    AND RoleName LIKE '%' + @RoleName + '%'
                ORDER BY RoleName;
            END
        ";

        await connection.ExecuteAsync(sql2);
    }
    
    private async Task SeedRoles(IDbConnection connection)
    {
        var exists = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM Role");

        if (exists > 0) return;

        await connection.ExecuteAsync(@"
        INSERT INTO Role (RoleName, IsActive)
        VALUES
        ('Admin', 1),
        ('Manager', 1),
        ('Operator', 1)
    ");
    }
}
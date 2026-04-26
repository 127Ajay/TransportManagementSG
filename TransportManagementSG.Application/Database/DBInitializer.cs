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
                    PasswordHash VARBINARY(MAX),
                    PasswordSalt VARBINARY(MAX),
                    IsActive BIT,
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
                    VehicleTypeId INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(100),
                    WheelCount INT,
                    LengthFeet DECIMAL(10,2),
                    IsActive BIT
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='State' AND xtype='U')
            BEGIN
                CREATE TABLE State (
                    StateId INT PRIMARY KEY IDENTITY,
                    StateCode NVARCHAR(10),
                    StateName NVARCHAR(100),
                    IsActive BIT
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TransportStatus' AND xtype='U')
            BEGIN
                CREATE TABLE TransportStatus (
                    TransportStatusId INT PRIMARY KEY IDENTITY,
                    StatusName NVARCHAR(100),
                    IsActive BIT
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Vehicle' AND xtype='U')
            BEGIN
                CREATE TABLE Vehicle (
                    VehicleId INT PRIMARY KEY IDENTITY,
                    VehicleNumber NVARCHAR(50) UNIQUE,
                    StateId INT,
                    VehicleTypeId INT,
                    Model NVARCHAR(100),
                    RegistrationYear INT,
                    TransportStatusId INT,
                    IsActive BIT,
                    FOREIGN KEY (StateId) REFERENCES State(StateId),
                    FOREIGN KEY (VehicleTypeId) REFERENCES VehicleType(VehicleTypeId),
                    FOREIGN KEY (TransportStatusId) REFERENCES TransportStatus(TransportStatusId)
                );
            END
            ";

        await connection.ExecuteAsync(sql);
    }
    
}
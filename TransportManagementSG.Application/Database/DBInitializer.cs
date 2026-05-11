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

        var sql3 = @"
                CREATE OR ALTER PROCEDURE sp_ValidateUser
                    @Email NVARCHAR(100),
                    @Password NVARCHAR(100)
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        u.UserId,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.RoleID,
                        u.IsActive,
                        r.RoleName AS Role
                    FROM [User] u
                    INNER JOIN Role r ON u.RoleID = r.RoleId
                    WHERE u.Email = @Email
                      AND u.LoginPassword = @Password
                      AND u.IsActive = 1
                      AND r.IsActive = 1
                END

            ";
        await connection.ExecuteAsync(sql3);

        var sql4 = @"                    
        CREATE OR ALTER PROCEDURE sp_GetAllUsers
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        SELECT 
                            u.UserId,
                            u.FirstName,
                            u.LastName,
                            u.Email,
                            u.PhoneNumber,
                            r.RoleName AS Role
                        FROM [User] u
                        INNER JOIN Role r 
                            ON u.RoleID = r.RoleId
                        WHERE u.IsActive = 1
                          AND r.IsActive = 1;
                    END
        ";
        await connection.ExecuteAsync(sql4);

        var sql5 = @"
            CREATE OR ALTER PROCEDURE usp_CreateUser
            @FirstName     NVARCHAR(100),
            @LastName      NVARCHAR(100),
            @Email         NVARCHAR(200),
            @PhoneNumber   NVARCHAR(20),
            @RoleID        INT = NULL

            AS
            BEGIN
                SET NOCOUNT ON;

                INSERT INTO [User]
                (
                    FirstName,
                    LastName,
                    Email,
                    PhoneNumber,
                    RoleID,
                    IsActive,
                    CreatedDate
                )
                VALUES
                (
                    @FirstName,
                    @LastName,
                    @Email,
                    @PhoneNumber,
                    @RoleID,
                    1,
                    GETDATE()
                );

                -- Return inserted Id (optional but useful)
                SELECT SCOPE_IDENTITY() AS UserId;
            END
            ";

        await connection.ExecuteAsync(sql5);

        var sql6 = @"
            CREATE OR ALTER PROCEDURE usp_DeleteUser
                    @UserId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE [User]
                    SET IsActive = 0
                    WHERE UserId = @UserId;
                END
        ";

        await connection.ExecuteAsync(sql6);

        var sql7 = @"
        CREATE OR ALTER PROCEDURE usp_GetUserById
            @UserId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        UserId,
                        FirstName,
                        LastName,
                        Email,
                        PhoneNumber,
                        RoleID
                    FROM [User]
                    WHERE UserId = @UserId;
                END
            ";

        await connection.ExecuteAsync(sql7);

        var sql8 = @"CREATE OR ALTER PROCEDURE usp_UpdateUser
                @UserId INT,
                @FirstName NVARCHAR(100),
                @LastName NVARCHAR(100),
                @Email NVARCHAR(200),
                @PhoneNumber NVARCHAR(20),
                @RoleID INT
            AS
            BEGIN
                SET NOCOUNT ON;

                UPDATE [User]
                SET 
                    FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    RoleID = @RoleID
                WHERE UserId = @UserId;
            END";
        await connection.ExecuteAsync(sql8);

        var sql9 = @"CREATE OR ALTER PROCEDURE usp_GetRoleById
                    @RoleId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT RoleId,
                           RoleName,
                           IsActive
                    FROM Role
                    WHERE RoleId = @RoleId;
                END";
        await connection.ExecuteAsync(sql9);

        var sql10 = @"
                    CREATE OR ALTER PROCEDURE usp_CreateRole
                        @RoleName NVARCHAR(100),
                        @IsActive BIT
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        INSERT INTO Role (RoleName, IsActive)
                        VALUES (@RoleName, @IsActive);

                        SELECT CAST(SCOPE_IDENTITY() AS INT) AS RoleId;
                    END";
        await connection.ExecuteAsync(sql10);

        var sql11 = @"
                    CREATE OR ALTER PROCEDURE usp_UpdateRole
                        @RoleId INT,
                        @RoleName NVARCHAR(100),
                        @IsActive BIT
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        UPDATE Role
                        SET RoleName = @RoleName,
                            IsActive = @IsActive
                        WHERE RoleId = @RoleId;
                    END";
        await connection.ExecuteAsync(sql11);

        var sql12 = @"
                    CREATE OR ALTER PROCEDURE usp_DeleteRole
                            @RoleId INT
                        AS
                        BEGIN
                            SET NOCOUNT ON;

                            UPDATE Role
                            SET IsActive = 0
                            WHERE RoleId = @RoleId;
                        END";
        await connection.ExecuteAsync(sql12);
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
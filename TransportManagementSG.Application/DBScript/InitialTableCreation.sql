CREATE TABLE RoleMaster (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL
);

CREATE TABLE UserMaster (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Email NVARCHAR(255),
    PhoneNumber NVARCHAR(20),
    RoleID INT NOT NULL,
    IsActive BIT,
    LoginPassword NVARCHAR(255),
    LastLoginDateRoleName DATETIME,
    CreatedBy NVARCHAR(100),
    CreatedDate DATETIME,
    ModifiedBy NVARCHAR(100),
    ModifiedDate DATETIME,

    CONSTRAINT FK_User_Role FOREIGN KEY (RoleID)
        REFERENCES RoleMaster(RoleId)
);

CREATE TABLE StateMaster (
    StateID INT IDENTITY(1,1) PRIMARY KEY,
    StateName NVARCHAR(100),
    IsActive BIT,
    CreatedBy NVARCHAR(100),
    CreatedDate DATETIME,
    ModifiedBy NVARCHAR(100),
    ModifiedDate DATETIME
);

CREATE TABLE VehicleType (
    VehicleTypeID INT IDENTITY(1,1) PRIMARY KEY,
    VehicleTypeName NVARCHAR(100),
    Wheel INT,
    Feet INT,
    IsActive BIT,
    CreatedBy NVARCHAR(100),
    CreatedDate DATETIME,
    ModifiedBy NVARCHAR(100),
    ModifiedDate DATETIME
);

CREATE TABLE TransportStatus (
    TransportStatusID INT IDENTITY(1,1) PRIMARY KEY,
    TransportStatusName NVARCHAR(100),
    IsActive BIT,
    CreatedBy NVARCHAR(100),
    CreatedDate DATETIME,
    ModifiedBy NVARCHAR(100),
    ModifiedDate DATETIME
);

CREATE TABLE VehicleMaster (
    VehicleMasterID INT IDENTITY(1,1) PRIMARY KEY,
    StateID INT NOT NULL,
    VehicleNumber NVARCHAR(50),
    VehicleTypeID INT NOT NULL,
    Model INT,
    RegistrationYear INT,
    StatusID INT NOT NULL,
    IsActive BIT,

    CreatedBy NVARCHAR(100),
    CreatedDate DATETIME,
    ModifiedBy NVARCHAR(100),
    ModifiedDate DATETIME,

    CONSTRAINT FK_Vehicle_State FOREIGN KEY (StateID)
        REFERENCES StateMaster(StateID),

    CONSTRAINT FK_Vehicle_Type FOREIGN KEY (VehicleTypeID)
        REFERENCES VehicleType(VehicleTypeID),

    CONSTRAINT FK_Vehicle_Status FOREIGN KEY (StatusID)
        REFERENCES TransportStatus(TransportStatusID)
);
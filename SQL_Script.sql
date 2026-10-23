--DROP DATABASE FUNDAMICRO
--GO

CREATE DATABASE FUNDAMICRO
GO

USE FUNDAMICRO
GO
CREATE TABLE Customers(
Id					INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
RegisterDate		DATETIME NOT NULL,
FirtName			VARCHAR(250),
LastName            VARCHAR(250),
Address 			VARCHAR(500),
Phone				VARCHAR(100),
Status              CHAR(1) NOT NULL DEFAULT ('A'),

CONSTRAINT CK_Customers_Status
        CHECK (Status IN ('A', 'I'))
)
GO

CREATE TABLE Users(
Id					INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
RegisterDate		DATETIME NOT NULL,
FullName            VARCHAR(250),
UserName			VARCHAR(250),
PasswordHash        VARCHAR(MAX),
PasswordSalt        VARCHAR(MAX),
Status				CHAR(1) NOT NULL,
Email				VARCHAR(250)
)

CREATE TABLE LogCustomers
(
    Id					INT IDENTITY(1,1) PRIMARY KEY,
    IdCustomer			INT NOT NULL,
    IdUser				INT NOT NULL,
    FullUserName		VARCHAR(250) NOT NULL,
    ActionUser			VARCHAR(20) NOT NULL,
    DateRegister		DATETIME NOT NULL DEFAULT GETDATE(),   
	PreviousDetail		VARCHAR(MAX) NULL,
    NewDetail			VARCHAR(MAX) NULL,

	CONSTRAINT FK_Log_Customer
        FOREIGN KEY (IdCustomer)
        REFERENCES Customers(Id),

    CONSTRAINT FK_Log_Users
        FOREIGN KEY (IdUser)
        REFERENCES Users(Id),

    CONSTRAINT CK_Log_Action_User
        CHECK (ActionUser IN ('ADD', 'EDIT', 'DELETE'))
)
GO

CREATE OR ALTER PROCEDURE dbo.sp_ValidateUser
    @UserName VARCHAR(250)
AS
BEGIN

    SELECT
        Id,
        FullName,
        UserName,
        PasswordHash,
        PasswordSalt
    FROM dbo.Users
    WHERE UserName = @UserName
      AND Status = 'A';
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_CreateUser
    @FullName VARCHAR(250),
    @UserName VARCHAR(250),
    @PasswordHash VARCHAR(MAX),
    @PasswordSalt VARCHAR(MAX),
    @Email VARCHAR(250) = NULL
AS
BEGIN

    IF EXISTS (
        SELECT 1
        FROM dbo.Users
        WHERE UserName = @UserName
    )
    BEGIN
        RAISERROR(
            'El nombre de usuario ya está registrado.',
            16,
            1
        );
        RETURN;
    END;

    INSERT INTO dbo.Users
    (
        RegisterDate,
        FullName,
        UserName,
        PasswordHash,
        PasswordSalt,
        Status,
        Email
    )
    VALUES
    (
        GETDATE(),
        @FullName,
        @UserName,
        @PasswordHash,
        @PasswordSalt,
        'A',
        @Email
    );
END;
GO


USE FUNDAMICRO;
GO

CREATE OR ALTER PROCEDURE dbo.sp_RegisterCustomerAudit
    @IdCustomer INT,
    @IdUser INT,
    @FullUserName VARCHAR(250),
    @ActionUser VARCHAR(20),
    @PreviousDetail VARCHAR(MAX) = NULL,
    @NewDetail VARCHAR(MAX) = NULL
AS
BEGIN

    INSERT INTO dbo.LogCustomers
    (
        IdCustomer,
        IdUser,
        FullUserName,
        ActionUser,
        PreviousDetail,
        NewDetail
    )
    VALUES
    (
        @IdCustomer,
        @IdUser,
        @FullUserName,
        @ActionUser,
        @PreviousDetail,
        @NewDetail
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetCustomerAudit
AS
BEGIN

    SELECT
        L.Id,
        L.IdCustomer,
        L.FullUserName,
        L.ActionUser,
        L.DateRegister,
        L.PreviousDetail,
        L.NewDetail
    FROM dbo.LogCustomers AS L
    ORDER BY
        L.DateRegister DESC,
        L.Id DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetActiveCustomers
AS
BEGIN
    SELECT Id, RegisterDate, FirtName, LastName, Address, Phone
    FROM dbo.Customers
    WHERE Status = 'A'
    ORDER BY Id DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetCustomerById
    @Id INT
AS
BEGIN
    SELECT Id, RegisterDate, FirtName, LastName, Address, Phone
    FROM dbo.Customers
    WHERE Id = @Id AND Status = 'A';
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_AddCustomer
    @FirstName VARCHAR(250),
    @LastName VARCHAR(250),
    @Address VARCHAR(500) = NULL,
    @Phone VARCHAR(100) = NULL
AS
BEGIN
    INSERT INTO dbo.Customers
        (RegisterDate, FirtName, LastName, Address, Phone, Status)
    VALUES
        (GETDATE(), @FirstName, @LastName, @Address, @Phone, 'A');

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdCustomer;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetCustomerForUpdate
    @Id INT
AS
BEGIN
    SELECT FirtName, LastName, Address, Phone
    FROM dbo.Customers WITH (UPDLOCK)
    WHERE Id = @Id AND Status = 'A';
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_UpdateCustomer
    @Id INT,
    @FirstName VARCHAR(250),
    @LastName VARCHAR(250),
    @Address VARCHAR(500) = NULL,
    @Phone VARCHAR(100) = NULL
AS
BEGIN
    UPDATE dbo.Customers
    SET FirtName = @FirstName,
        LastName = @LastName,
        Address = @Address,
        Phone = @Phone
    WHERE Id = @Id AND Status = 'A';

    IF @@ROWCOUNT = 0
        THROW 50001, 'El cliente no existe o está inactivo.', 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DeactivateCustomer
    @Id INT
AS
BEGIN
    UPDATE dbo.Customers
    SET Status = 'I'
    WHERE Id = @Id AND Status = 'A';

    IF @@ROWCOUNT = 0
        THROW 50002, 'El cliente no existe o ya está inactivo.', 1;
END;
GO


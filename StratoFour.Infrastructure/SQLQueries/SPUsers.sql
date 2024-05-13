-- Stored Procedures User
CREATE PROCEDURE [dbo].[spUser_Delete]
    @Id INT
AS
BEGIN
    DELETE FROM dbo.Users WHERE UserId = @Id;
END;
GO

CREATE PROCEDURE [dbo].[spUser_Get]
    @Id INT
AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash FROM dbo.Users WHERE UserId = @Id;
END;
GO

CREATE PROCEDURE [dbo].[spUser_GetByEmail]
    @Id INT
AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash FROM dbo.Users WHERE Email = @Email;
END;
GO

CREATE PROCEDURE [dbo].[spUser_GetAll]
AS
BEGIN
    SELECT UserId, Username, Email FROM dbo.Users;
END;
GO

CREATE PROCEDURE [dbo].[spUser_Insert]
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(100)
AS
BEGIN
    INSERT INTO dbo.Users (Username, Email, PasswordHash)
    VALUES (@Username, @Email, @PasswordHash);
END;
GO

CREATE PROCEDURE [dbo].[spUser_Update]
    @Id INT,
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(100)
AS
BEGIN
    UPDATE dbo.Users
    SET Username = @Username, Email = @Email, PasswordHash = @PasswordHash
    WHERE UserId = @Id;
END;
GO

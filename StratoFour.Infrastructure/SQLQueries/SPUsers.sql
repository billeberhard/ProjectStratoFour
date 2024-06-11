-- Stored Procedures User
CREATE PROCEDURE [dbo].[spUser_Delete]
    @UserId INT
AS
BEGIN
    DELETE FROM dbo.Users WHERE UserId = @UserId;
END;
GO

CREATE PROCEDURE [dbo].[spUser_Get]
    @UserId INT
AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash, ConnectionId FROM dbo.Users WHERE UserId = @UserId;
END;
GO

CREATE PROCEDURE [dbo].[spUser_GetByEmail]
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash, ConnectionId  FROM dbo.Users WHERE Email = @Email;
END;
GO

CREATE PROCEDURE [dbo].[spUser_GetAll]
AS
BEGIN
    SELECT UserId, Username, Email, ConnectionId FROM dbo.Users;
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
    @UserId INT,
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(100), 
    @EmailVerification BIT,
    @ConnectionId NVARCHAR(100)
AS
BEGIN
    UPDATE dbo.Users
    SET Username = @Username, Email = @Email, PasswordHash = @PasswordHash, ConnectionId = @ConnectionId, EmailVerification = @EmailVerification
    WHERE UserId = @UserId;
END;
GO

-- Stored Procedures for Game Matching
CREATE PROCEDURE [dbo].[spGame_Insert]
    @Player1Id INT,
    @Player2Id INT,
    @RobotId INT,
    @StartTime DATETIME,
    @IsActive BIT
AS
BEGIN
    INSERT INTO dbo.Games (Player1Id, Player2Id, RobotId, StartTime, IsActive)
    VALUES (@Player1Id, @Player2Id, @RobotId, @StartTime, @IsActive);
    
    SELECT SCOPE_IDENTITY() AS SessionId;
END;
GO

CREATE PROCEDURE [dbo].[spGame_Get]
    @Id INT
AS
BEGIN
    SELECT SessionId, Player1Id, Player2Id, RobotId, StartTime, WinnerId, IsActive
    FROM dbo.Games
    WHERE SessionId = @Id;
END;
GO

CREATE PROCEDURE [dbo].[spGame_GetActive]
    @UserId INT
AS
BEGIN
    SELECT SessionId, Player1Id, Player2Id, RobotId, StartTime, WinnerId, IsActive
    FROM dbo.Games
    WHERE (Player1Id = @UserId OR Player2Id = @UserId) AND IsActive = 1;
END;
GO

CREATE PROCEDURE [dbo].[spGame_GetAll]
AS
BEGIN
    SELECT SessionId, Player1Id, Player2Id, RobotId, StartTime, WinnerId, IsActive
    FROM dbo.Games;
END;
GO

CREATE PROCEDURE [dbo].[spGame_Update]
    @SessionId INT,
    @WinnerId INT,
    @IsActive BIT
AS
BEGIN
    UPDATE dbo.Games
    SET WinnerId = @WinnerId, IsActive = @IsActive
    WHERE SessionId = @SessionId;
END;
GO

CREATE PROCEDURE [dbo].[spGame_Delete]
    @Id INT
AS
BEGIN
    DELETE FROM dbo.Games WHERE SessionId = @Id;
END;
GO

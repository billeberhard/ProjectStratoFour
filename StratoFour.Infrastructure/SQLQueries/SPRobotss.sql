-- Stored Procedures Robot
CREATE PROCEDURE [dbo].[spRobot_Delete]
    @Id INT
AS
BEGIN
    DELETE FROM dbo.Robots WHERE RobotId = @Id;
END;
GO

CREATE PROCEDURE [dbo].[spRobot_Get]
    @Id INT
AS
BEGIN
    SELECT RobotId, RobotName, RobotStatus FROM dbo.Robots WHERE RobotId = @Id;
END;
GO

CREATE PROCEDURE [dbo].[spRobot_GetAll]
AS
BEGIN
    SELECT RobotId, RobotName, RobotStatus FROM dbo.Robots;
END;
GO

CREATE PROCEDURE [dbo].[spRobot_Insert]
    @RobotName NVARCHAR(50),
    @RobotStatus NVARCHAR(50)
AS
BEGIN
    INSERT INTO dbo.Robots (RobotName, RobotStatus)
    VALUES (@RobotName, @RobotStatus);
END;
GO

CREATE PROCEDURE [dbo].[spRobot_Update]
    @Id INT,
    @RobotName NVARCHAR(50),
    @RobotStatus NVARCHAR(50)
AS
BEGIN
    UPDATE dbo.Robots
    SET RobotName = @RobotName, RobotStatus = @RobotStatus
    WHERE RobotId = @Id;
END;
GO

-- Stored Procedures Robot
CREATE PROCEDURE spRobot_GetReady
AS
BEGIN
    SELECT TOP 1 *
    FROM Robots
    WHERE RobotStatus = 'Ready';
END;
GO

CREATE PROCEDURE spRobot_GetAll
AS
BEGIN
    SELECT * FROM Robots;
END;
GO

CREATE PROCEDURE spRobot_GetById
    @RobotId INT
AS
BEGIN
    SELECT * FROM Robots WHERE RobotId = @RobotId;
END;
GO

CREATE PROCEDURE spRobot_Insert
    @RobotName NVARCHAR(50),
    @RobotStatus NVARCHAR(50)
AS
BEGIN
    INSERT INTO Robots (RobotName, RobotStatus)
    VALUES (@RobotName, @RobotStatus);
    
    SELECT SCOPE_IDENTITY() AS RobotId;
END;
GO

CREATE PROCEDURE spRobot_Update
    @RobotId INT,
    @RobotName NVARCHAR(50),
    @RobotStatus NVARCHAR(50)
AS
BEGIN
    UPDATE Robots
    SET RobotName = @RobotName, RobotStatus = @RobotStatus
    WHERE RobotId = @RobotId;
END;
GO

CREATE PROCEDURE spRobot_UpdateStatus
    @RobotId INT,
    @RobotStatus NVARCHAR(50)
AS
BEGIN
    UPDATE Robots
    SET RobotStatus = @RobotStatus
    WHERE RobotId = @RobotId;
END;
GO

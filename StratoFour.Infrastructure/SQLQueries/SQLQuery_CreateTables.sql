CREATE TABLE Users (
    UserId INT PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    RegistrationDate DATETIME NOT NULL
);

CREATE TABLE Robots (
    RobotId INT PRIMARY KEY,
    RobotName NVARCHAR(50) NOT NULL,
    RobotStatus NVARCHAR(50) NOT NULL CHECK (RobotStatus IN ('In Use', 'Ready', 'Inactive'))
);

CREATE TABLE Games (
    GameId INT PRIMARY KEY,
    Player1Id INT,
    Player2Id INT,
    RobotId INT NOT NULL,
    StartTime DATETIME NOT NULL,
    WinnerId INT,
    FOREIGN KEY (Player1Id) REFERENCES Users(UserId),
    FOREIGN KEY (Player2Id) REFERENCES Users(UserId),
    FOREIGN KEY (WinnerId) REFERENCES Users(UserId),
    FOREIGN KEY (RobotId) REFERENCES Robots(RobotId)
);

CREATE TABLE Moves (
    MoveId INT PRIMARY KEY,
    GameId INT,
    PlayerId INT,
    ColumnIndex INT NOT NULL,
    RowIndex INT NOT NULL,
    MoveTime DATETIME NOT NULL,
    FOREIGN KEY (GameId) REFERENCES Games(GameId),
    FOREIGN KEY (PlayerId) REFERENCES Users(UserId)
);




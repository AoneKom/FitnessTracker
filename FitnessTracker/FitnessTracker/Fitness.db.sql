Create database tracker;

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL
);

CREATE TABLE UserStats (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    Date DATETIME DEFAULT GETDATE(),
    ActivityLevel INT,       
    CaloriesBurned FLOAT,
    Weight FLOAT,
    Pulse INT,
    Pressure NVARCHAR(20)
);

CREATE TABLE Goals (
    UserId INT PRIMARY KEY FOREIGN KEY REFERENCES Users(Id),
    CaloriesTarget FLOAT,
    StepsTarget INT
);


ALTER TABLE Activities ADD Steps INT DEFAULT 0;

CREATE TABLE Activities (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    ActivityType NVARCHAR(50),
    Duration FLOAT,             
    CaloriesBurned FLOAT,
    Steps INT,
    Date DATETIME DEFAULT GETDATE()
);


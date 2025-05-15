-- Таблица пользователей
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE
);

-- Таблица целей
CREATE TABLE IF NOT EXISTS Goals (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER,
    Type TEXT NOT NULL,         
    TargetValue INTEGER NOT NULL,
    CurrentValue INTEGER DEFAULT 0,
    DateCreated TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Таблица активностей
CREATE TABLE IF NOT EXISTS Activities (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER,
    Type TEXT NOT NULL,         
    Duration INTEGER NOT NULL, 
    Calories INTEGER NOT NULL,
    Date TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Таблица состояния здоровья
CREATE TABLE IF NOT EXISTS HealthStatus (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER,
    Weight REAL,                -- В килограммах
    Pulse INTEGER,              -- ЧСС
    BloodPressure TEXT,         -- Например: "120/80"
    Date TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE INDEX IF NOT EXISTS idx_activities_user ON Activities(UserId);
CREATE INDEX IF NOT EXISTS idx_goals_user ON Goals(UserId);


FOREIGN KEY (...) REFERENCES Users(Id) ON DELETE CASCADE

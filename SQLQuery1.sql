-- Create database
CREATE DATABASE cybersecurity_chatbot;
GO

USE cybersecurity_chatbot;
GO

-- Tasks table
CREATE TABLE tasks (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NOT NULL,
    title NVARCHAR(200) NOT NULL,
    description NVARCHAR(MAX),
    reminder_date DATETIME,
    is_completed BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE()
);
GO

-- Activity log table
CREATE TABLE activity_log (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NOT NULL,
    action NVARCHAR(255) NOT NULL,
    details NVARCHAR(MAX),
    timestamp DATETIME DEFAULT GETDATE()
);
GO

-- Quiz scores table
CREATE TABLE quiz_scores (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NOT NULL,
    score INT,
    total INT,
    date_taken DATETIME DEFAULT GETDATE()
);
GO
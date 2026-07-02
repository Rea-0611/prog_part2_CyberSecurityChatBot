USE cybersecurity_chatbot;
GO

-- Drop tables if they exist (optional - be careful!)
IF OBJECT_ID('tasks', 'U') IS NOT NULL DROP TABLE tasks;
IF OBJECT_ID('activity_log', 'U') IS NOT NULL DROP TABLE activity_log;
IF OBJECT_ID('quiz_scores', 'U') IS NOT NULL DROP TABLE quiz_scores;
GO

-- Create tasks table
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

-- Create activity log table
CREATE TABLE activity_log (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NOT NULL,
    action NVARCHAR(255) NOT NULL,
    details NVARCHAR(MAX),
    timestamp DATETIME DEFAULT GETDATE()
);
GO

-- Create quiz scores table
CREATE TABLE quiz_scores (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NOT NULL,
    score INT,
    total INT,
    date_taken DATETIME DEFAULT GETDATE()
);
GO

-- Insert test data
INSERT INTO tasks (username, title, description) 
VALUES ('TestUser', 'Test Task', 'This is a test task');
GO

INSERT INTO activity_log (username, action, details) 
VALUES ('TestUser', 'Database Test', 'Database created successfully');
GO

-- Verify
SELECT * FROM tasks;
SELECT * FROM activity_log;
SELECT * FROM quiz_scores;
GO
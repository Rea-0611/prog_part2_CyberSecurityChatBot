# prog_part2_CyberSecurityChatBot and POE Part 3

🛡️ Cybersecurity Awareness Chatbot – Part 2

A modern **WPF desktop application** that educates users about cybersecurity threats through an interactive chat interface. This is **Part 2** of a three‑part portfolio project for the IIE, building on the console application from Part 1.

## 📌 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Project Structure](#project-structure)
- [Requirements](#requirements)
- [How to Run](#how-to-run)
- [How It Works](#how-it-works)
- [Class Breakdown](#class-breakdown)
- [Sentiment Detection](#sentiment-detection)
- [Example Conversation](#example-conversation)
- [Continuous Integration](#continuous-integration)
- [Video Presentation](#video-presentation)
- [Requirements Traceability](#requirements-traceability)
- [Author](#author)

---

## Overview

This application transforms the command-line chatbot from Part 1 into a **Graphical User Interface (GUI)** using **Windows Presentation Foundation (WPF)**. The chatbot recognises cybersecurity keywords, provides random responses from topic-specific lists, detects user sentiment, remembers user preferences, and maintains natural conversation flow.

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| 🎤 **Voice Greeting** | Plays `greet.wav` on application launch |
| 🖼️ **Professional UI** | Three-page navigation with modern design (gradients, rounded corners, chat bubbles) |
| 💬 **Chat Interface** | Message bubbles with user/bot differentiation, auto-scrolling |
| 🔑 **Keyword Recognition** | Detects 15+ cybersecurity topics (passwords, phishing, scams, privacy, 2FA, malware, VPN, etc.) |
| 🎲 **Random Responses** | Multiple answers per topic, randomly selected for variety |
| 🔁 **Conversation Flow** | Handles follow-ups like "another tip" or "tell me more" |
| 🧠 **Memory & Recall** | Saves username to `users.txt`, stores interests to `user_interests.txt` |
| 😊 **Sentiment Detection** | Detects worried, frustrated, curious, confused, happy, sad, grateful – responds empathetically |
| ✅ **Input Validation** | Handles empty inputs and unrecognised queries gracefully |
| 📁 **File Storage** | Persistent storage of usernames and user interests |

---

## 📁 Project Structure

```

prog_part2_CyberSecurityChatBot/
│
├── MainWindow.xaml              # Main window with navigation Frame
├── MainWindow.xaml.cs           # Event handlers (only calls to managers)
│
├── GreetingManager.cs           # Voice greeting playback
├── UsernameManager.cs           # Username validation, file storage, interests
├── ResponseManager.cs           # Keyword dictionary, random responses, sentiment detection
├── UIManager.cs                 # Chat display, navigation, message processing
│
├── greet.wav                    # Voice greeting file
├── image.jpg                    # Logo image
│
├── users.txt                    # (Auto-created) Stores usernames
├── user_interests.txt           # (Auto-created) Stores user interests
│
├── App.xaml                     # Application definition
├── App.xaml.cs                  # Application code-behind
│
└── .github/workflows/
└── dotnet.yml               # CI workflow

```
<img width="257" height="196" alt="image" src="https://github.com/user-attachments/assets/8ad07111-d0b8-43e9-9f7d-405e74cd5580" />


---

## 📋 Requirements

- **Windows OS** (for `System.Media.SoundPlayer` – voice greeting)
- **.NET Framework 4.7.2** or **.NET Core/.NET 5+** (WPF support)
- **Visual Studio 2019/2022** (recommended)

---

## 🚀 How to Run

### Option 1: Using Visual Studio

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/prog_part2_CyberSecurityChatBot.git
```

2. Open the solution – Double-click prog_part2_CyberSecurityChatBot.csproj
3. Add your media files (if not already present):
   · Place greet.wav in the project root
   · Place image.jpg in the project root
   · Set both files' Copy to Output Directory = Copy if newer
4. Build and run – Press F5

Option 2: Using Command Line

```bash
dotnet build
dotnet run
```

⚠️ Voice greeting only works on Windows. Other features work on any OS.

---

⚙️ How It Works

Navigation Flow

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│   Start Page    │────▶│  Username Page  │────▶│   Chat Page     │
│ (Welcome + logo)│     │ (Enter your name)│     │ (Conversation)  │
└─────────────────┘     └─────────────────┘     └─────────────────┘
        ▲                                              │
        │                                              │
        └────────────── (Back button) ─────────────────┘
```

Response Generation (Dictionary Method)

The ResponseManager uses a Dictionary<string, List<string>> where each keyword maps to a list of possible answers:

```csharp
topicResponses = new Dictionary<string, List<string>>
{
    ["password"] = new List<string>
    {
        "🔐 Create strong passwords with 12+ characters...",
        "🔑 Never reuse passwords across different accounts...",
        "🛡️ Enable Two-Factor Authentication (2FA)..."
    },
    ["phish"] = new List<string> { ... },
    // ... 15+ topics
};
```

When a user asks a question:

1. The input is split into words
2. Each word is checked against dictionary keys
3. When a match is found, a random response is selected from the corresponding list
4. The response is displayed in a chat bubble

---

📚 Class Breakdown

Class Responsibility Key Methods
MainWindow Container with Frame, event handlers welcome(), submit_name(), send()
GreetingManager Plays voice greeting PlayVoiceGreeting()
UsernameManager Validates names, saves to file, stores interests ProcessUsername(), SaveUserInterest(), GetSavedInterests()
ResponseManager Keyword matching, random responses, sentiment detection GenerateResponse(), DetectSentiment(), GetSentimentResponse()
UIManager Chat display, navigation, message processing AddUserMessage(), AddBotMessage(), ProcessUserQuestion()

---

😊 Sentiment Detection

The chatbot detects 7 emotions and responds empathetically:

Sentiment Keywords Detected Example Response
worried worried, scared, nervous, anxious, afraid "😟 It's completely understandable to feel worried about cybersecurity..."
frustrated frustrated, annoyed, angry, mad, upset "😤 I understand this can be frustrating. Let's break it down..."
curious curious, interested, wondering, want to learn "🤔 That's a great question! I'm glad you're curious..."
confused confused, don't understand, not clear, unclear "🤨 No worries - cybersecurity can be confusing at first..."
happy happy, great, good, awesome, excellent "😊 I'm glad to hear that! Staying positive helps..."
sad sad, upset, depressed, unhappy, terrible "😔 I'm sorry you're feeling this way..."
grateful thanks, thank you, appreciate, helpful "🙏 You're very welcome! I'm here to help..."

If a negative sentiment is detected alongside a cybersecurity question, the bot responds empathetically before providing the security tip.

---

💬 Example Conversation

```
Start Page → Click "Start Chatbot"
Username Page → Enter "Thabo"

🤖 CyberBot: Hey Thabo! Welcome to AI Cybersecurity Assistant! 🎉

🤖 CyberBot: I'm here to help you stay safe online. What would you like to learn about?

👤 Thabo: I'm worried about phishing emails

🤖 CyberBot: 😟 It's completely understandable to feel worried about cybersecurity. 
   Let me share some practical tips to help you feel more secure.

🤖 CyberBot: 🎣 Phishing emails often have urgent language, spelling mistakes, 
   or suspicious links. Always check the sender!

👤 Thabo: Tell me about passwords

🤖 CyberBot: 🔐 Create strong passwords with 12+ characters, mixing uppercase, 
   lowercase, numbers, and symbols!

👤 Thabo: another tip

🤖 CyberBot: 🔑 Never reuse passwords across different accounts. Use a password 
   manager like Bitwarden or LastPass!

👤 Thabo: I'm interested in privacy

🤖 CyberBot: Great! I'll remember your interests! 📝

👤 Thabo: bye

🤖 CyberBot: 👋 Stay safe online! Remember: cybersecurity is everyone's responsibility.
```

---

🔁 Continuous Integration (GitHub Actions)

The repository includes a CI workflow that builds the project on every push.

Workflow file: .github/workflows/dotnet.yml

```yaml
name: .NET Build WPF

on: [push]

jobs:
  build:
    runs-on: windows-latest
    steps:
    - uses: actions/checkout@v4
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 8.0.x
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --configuration Release
```

🎥 Video Presentation

▶️ Watch the explanation and demo on YouTube

The video (8–10 minutes) covers:

· Overview of Part 2 requirements
· Code structure and class responsibilities
· Navigation flow (3-page design)
· Keyword recognition and random response system
· Sentiment detection and empathetic responses
· Memory and recall (username and interests)
· Live demonstration of all features

⚠️ The video uses my own voice (no AI voice) and is unlisted as required.

Youtube link to video: https://youtu.be/NLM3fG6D0z0

---

✅ Requirements Traceability (Part 2)

Requirement Implementation
GUI using WPF MainWindow.xaml with Frame navigation, gradients, chat bubbles
Keyword recognition ResponseManager.topicResponses dictionary with 15+ topics
Random responses List<string> per topic + Random.Next()
Conversation flow Tracks lastTopic, handles "another tip", "tell me more"
Memory and recall UsernameManager saves to users.txt and user_interests.txt
Sentiment detection DetectSentiment() + empathetic responses from sentimentResponses
Error handling Empty input check, fallback messages, no crashes
Code optimisation Dictionary lookups, separate classes, OOP principles
GitHub ≥6 commits See commit history
CI workflow .github/workflows/dotnet.yml
Video presentation YouTube link above

---

There was use of AI, namely Deepseek, for some parts of the code and all credit for generated parts goes therefore to named AI tool.

```


# 🛡️ Cybersecurity Awareness Chatbot – Part 3 (POE)

A complete **WPF desktop application** that educates users about cybersecurity through an interactive chat interface, task management, quiz games, and activity logging. This is the final part of a three‑part portfolio project for the IIE, building on Parts 1 and 2.

---

## 📌 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Project Structure](#project-structure)
- [Requirements](#requirements)
- [How to Run](#how-to-run)
- [Database Setup](#database-setup)
- [Features Explained](#features-explained)
  - [Chat Interface](#chat-interface)
  - [Task Assistant with Reminders](#task-assistant-with-reminders)
  - [Cybersecurity Quiz](#cybersecurity-quiz)
  - [Activity Log](#activity-log)
- [How It Works](#how-it-works)
- [Class Breakdown](#class-breakdown)
- [Example Conversation](#example-conversation)
- [Video Presentation](#video-presentation)
- [Continuous Integration](#continuous-integration)
- [Requirements Traceability](#requirements-traceability)
- [Author](#author)

---

## Overview

This application is the final submission for the Cybersecurity Awareness Chatbot project. It transforms the command-line chatbot from Part 1 into a fully-featured **WPF desktop application** with:

- 💬 **Intelligent Chat** – keyword-based responses covering 30+ cybersecurity topics
- 📋 **Task Assistant** – manage cybersecurity tasks with reminders stored in SQL Server
- 🎮 **Interactive Quiz** – 35 cybersecurity questions with instant feedback and scoring
- 📊 **Activity Log** – track all user actions and bot interactions

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| 🎤 **Voice Greeting** | Plays `greet.wav` on application launch |
| 🖼️ **Professional UI** | Three-page navigation with modern dark theme |
| 💬 **Chat Interface** | Message bubbles with user/bot differentiation, auto-scrolling |
| 🔑 **Keyword Recognition** | Detects 30+ cybersecurity topics (passwords, phishing, scams, privacy, 2FA, malware, VPN, deepfakes, AI, cloud security, zero-day attacks, etc.) |
| 🎲 **Random Responses** | Multiple answers per topic, randomly selected for variety |
| 🔁 **Conversation Flow** | Handles follow-ups like "another tip" or "tell me more" |
| 📋 **Task Assistant** | Add, view, complete, and delete cybersecurity tasks |
| ⏰ **Reminders** | Set reminders for tasks with date/time |
| 🗄️ **Database Integration** | Stores tasks, activity logs, and quiz scores in SQL Server |
| 🎮 **Cybersecurity Quiz** | 35 questions covering 12+ categories with instant feedback |
| 📊 **Activity Log** | Tracks all significant actions with timestamps |
| 🧠 **NLP Simulation** | Natural language processing with keyword detection |
| 💾 **Memory & Recall** | Remembers username and user interests |
| ✅ **Input Validation** | Handles empty inputs and unrecognised queries gracefully |

---

## 📁 Project Structure

```

prog_part2_CyberSecurityChatBot/
│
├── MainWindow.xaml              # Main window UI
├── MainWindow.xaml.cs           # Event handlers
│
├── DatabaseHelper.cs            # SQL Server operations
├── ActivityLogManager.cs        # Activity logging
├── TaskManager.cs               # Task management
├── QuizManager.cs               # Quiz logic
├── QuizQuestion.cs              # Quiz question model
├── GreetingManager.cs           # Voice greeting
├── UsernameManager.cs           # Username + interests
├── ResponseManager.cs           # 30+ topics with random responses
│
├── greet.wav                    # Voice greeting file
├── image.jpg                    # Logo image
│
├── users.txt                    # (Auto-created) Stores usernames
├── user_interests.txt           # (Auto-created) Stores user interests
│
├── App.xaml                     # Application definition
├── App.xaml.cs                  # Application code-behind
│
└── .github/workflows/
└── dotnet.yml               # CI workflow

```

---

## 📋 Requirements

- **Windows OS** (for `System.Media.SoundPlayer` – voice greeting)
- **.NET Framework 4.7.2** or **.NET 6/8** (WPF support)
- **Visual Studio 2019/2022** (recommended)
- **SQL Server LocalDB** or **SQL Server Express** (for database features)

---

## 🚀 How to Run

### Option 1: Using Visual Studio

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/prog_part2_CyberSecurityChatBot.git
```

2. Open the solution – Double-click prog_part2_CyberSecurityChatBot.csproj
3. Install NuGet packages:
   · Tools → NuGet Package Manager → Package Manager Console
   · Run: Install-Package System.Data.SqlClient
4. Add your media files (if not already present):
   · Place greet.wav in the project root
   · Place image.jpg in the project root
   · Set both files' Copy to Output Directory = Copy if newer
5. Set up the database (see Database Setup below)
6. Build and run – Press F5

Option 2: Using Command Line

```bash
dotnet build
dotnet run
```

⚠️ Voice greeting only works on Windows. Other features work on any OS.

---

🗄️ Database Setup

Step 1: Install SQL Server LocalDB

LocalDB comes with Visual Studio. If you don't have it:

1. Download SSMS from: https://aka.ms/ssms
2. During installation, select LocalDB

Step 2: Create the Database

In Visual Studio:

1. View → SQL Server Object Explorer
2. Expand (localdb)\MSSQLLocalDB
3. Right-click Databases → Add New Database
4. Name it: cybersecurity_chatbot

Step 3: Create Tables

Run this script in a new query window:

```sql
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
```

Step 4: Update Connection String

In DatabaseHelper.cs, update the connection string if needed:

```csharp
// For LocalDB (default):
connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=cybersecurity_chatbot;Integrated Security=True;";

// For SQL Server Express:
// connectionString = @"Server=localhost\SQLEXPRESS;Database=cybersecurity_chatbot;Integrated Security=True;";

// For full SQL Server:
// connectionString = "Server=localhost;Database=cybersecurity_chatbot;Integrated Security=True;";
```

---

Features Explained

💬 Chat Interface

The main chat interface allows users to:

· Ask cybersecurity questions in natural language
· Receive informative responses with cybersecurity tips
· Use commands like help, add task, show tasks, start quiz, show log

Topics Covered (30+):

· Passwords, Phishing, Scams, Privacy, Safe Browsing, 2FA, Malware, VPN, Backups, Updates, Encryption, Social Engineering, Identity Theft, Public Wi-Fi, Deepfakes, AI, Cloud Security, Zero-Day Attacks, and more.

---

📋 Task Assistant with Reminders

The task assistant helps users manage cybersecurity-related tasks:

Action Command Description
Add Task add task [title] Adds a new cybersecurity task
Add with Reminder remind me [title] Adds task with date/time reminder
View Tasks show tasks Displays all pending tasks
Complete Click ✅ Marks task as completed
Delete Click 🗑️ Removes task from the list

Database Storage: All tasks are stored in SQL Server with:

· Title and description
· Reminder date/time
· Completion status
· Creation timestamp

---

🎮 Cybersecurity Quiz

The quiz tests users' knowledge with 35 questions across 12+ categories:

Category Questions Topics
Passwords 4 Strength, managers, reuse, best practices
Phishing 4 Email scams, vishing, reporting
Social Engineering 2 Manipulation, password sharing
2FA 3 Authenticator apps, backup codes
Malware/Ransomware 4 Signs, prevention, backups
Safe Browsing 3 HTTPS, encryption, security
Privacy 3 App permissions, VPNs, public Wi-Fi
Backups/Updates 3 3-2-1 rule, update schedules
Cloud/AI 2 Cloud security, deepfakes
Identity Theft 2 Protection, monitoring
Encryption/Zero-Day 2 Data protection, vulnerabilities
General Security 3 Common threats, breach response

Features:

· Questions shuffled randomly each time
· Immediate feedback with explanations
· Score tracking and final feedback
· Quiz history stored in database

---

📊 Activity Log

The activity log automatically tracks all significant actions:

Action Description
Task Added Task title and reminder date
Task Completed Task marked as done
Task Deleted Task removed
Quiz Answer Question number and correct/incorrect
Quiz Complete Final score
User Input User's chat messages
Bot Response Bot's replies
Help Viewed User requested help

Viewing Log:

· Type show log in chat
· Click the "Activity Log" tab
· Click "Refresh" to update

---

How It Works

Response Generation

The ResponseManager uses a Dictionary<string, List<string>> where each keyword maps to multiple possible answers:

```csharp
topicResponses = new Dictionary<string, List<string>>
{
    ["password"] = new List<string>
    {
        "🔐 Create strong passwords with 12+ characters...",
        "🔑 Never reuse passwords across different accounts...",
        "🛡️ Enable Two-Factor Authentication (2FA)..."
    },
    // ... 30+ topics
};
```

When a user asks a question:

1. Input is split into words
2. Each word is checked against dictionary keys
3. When a match is found, a random response is selected
4. The response is displayed in a chat bubble

Database Operations

The DatabaseHelper class handles all SQL Server operations:

· AddTask() – Insert new task
· GetTasks() – Retrieve user tasks
· CompleteTask() – Mark task as done
· DeleteTask() – Remove task
· AddActivityLog() – Log actions
· GetRecentActivity() – Retrieve recent logs
· SaveQuizScore() – Store quiz results

Quiz Logic

The QuizManager manages the quiz flow:

1. 35 questions shuffled randomly
2. One question displayed at a time
3. User selects answer (A, B, C, or D)
4. Immediate feedback with explanation
5. Score updates in real-time
6. Final feedback based on percentage

---

📚 Class Breakdown

Class Responsibility Key Methods
MainWindow Container with tabs, event handlers ProcessQuestion(), StartQuiz(), LoadTasks()
DatabaseHelper SQL Server operations AddTask(), GetTasks(), AddActivityLog(), SaveQuizScore()
ActivityLogManager Activity logging AddLog(), GetRecentLogs(), GetLogSummary()
TaskManager Task management AddTask(), CompleteTask(), DeleteTask()
QuizManager Quiz logic ResetQuiz(), SubmitAnswer(), GetFinalFeedback()
QuizQuestion Question model Properties: Id, Category, Question, Options, CorrectAnswerIndex, Explanation
ResponseManager Chat responses GenerateResponse()
UsernameManager Username + interests ProcessUsername(), CheckIfUserExists()
GreetingManager Voice greeting PlayVoiceGreeting()

---

💬 Example Conversation

```
Please enter your name: Thabo

Welcome, Thabo! 🎉
💡 Try these commands:
• 'add task [title]' - Add a new task
• 'show tasks' - View your tasks
• 'start quiz' - Take a cybersecurity quiz
• 'show log' - View recent activity
• 'help' - Show all commands

You: Tell me about phishing
Bot: 🎣 Phishing emails often have urgent language, spelling mistakes, or suspicious links. 
     Always check the sender!

You: How do I create a strong password?
Bot: 🔐 Create strong passwords with 12+ characters, mixing uppercase, lowercase, numbers, and symbols!

You: add task Enable two-factor authentication on all accounts
Bot: ✅ Task added: 'Enable two-factor authentication on all accounts'

You: show tasks
Bot: 📋 Your Tasks:
     ⬜ Enable two-factor authentication on all accounts

You: start quiz
Bot: 🎮 Starting Cybersecurity Quiz! Test your knowledge.

You: [Selects answer]
Bot: ✅ Correct! Two-Factor Authentication adds an extra layer of security...
```

---

🎥 Video Presentation

YouTube video link: https://youtu.be/NSk-_Kef9YE

The video (8–10 minutes) covers:

· Overview of all three parts
· Code structure and class responsibilities
· Database integration with SQL Server
· Task assistant with reminders
· Quiz with 35 questions and feedback
· Activity log tracking
· Live demonstration of all features

⚠️ The video uses my own voice (no AI voice) and is unlisted as required.

---

🔁 Continuous Integration (GitHub Actions)

The repository includes a CI workflow that builds the project on every push.

Workflow file: .github/workflows/dotnet.yml

```yaml
name: .NET Build WPF

on: [push]

jobs:
  build:
    runs-on: windows-latest
    steps:
    - uses: actions/checkout@v4
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 8.0.x
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --configuration Release
```

✅ Build status badge:
https://github.com/Rea_0611/prog_part2_CyberSecurityChatBot/actions/workflows/dotnet.yml/badge.svg

---

✅ Requirements Traceability (Part 3)

Requirement Implementation
Task Assistant with Reminders TaskManager.cs + DatabaseHelper.cs + Tasks Tab
Database Integration (SQL Server) DatabaseHelper.cs with SQL Server operations
Add Task with Details TaskManager.AddTask() with title, description, reminder
View and Manage Tasks TaskListView with Complete/Delete buttons
Cybersecurity Mini-Game (Quiz) QuizManager.cs + Quiz Tab
More than 10 Questions 35 questions across 12+ categories
Mix Question Types Multiple-choice and true/false
Immediate Feedback Explanation shown after each answer
Track User Score Score tracking with percentage and final feedback
NLP Simulation Keyword detection with NLPManager and TaskManager
Activity Log ActivityLogManager.cs + Activity Log Tab
Track Key Actions Tasks, reminders, quiz, chat interactions
Display Last 5-10 Actions Logs displayed in Activity Log tab
GitHub ≥6 Commits See commit history
3 Releases Tagged GitHub releases
CI Workflow .github/workflows/dotnet.yml
Video Presentation YouTube link above

---

👨‍💻 Author

Pontsho Reshoketswe Sedumedi
DISD0601 YEAR 2 GROUP 1 PROG6221 
GitHub Profile

---

📄 License

This project is for educational purposes as part of a coursework portfolio.

---

Built with C#, WPF, SQL Server, and ❤️ for cybersecurity awareness.

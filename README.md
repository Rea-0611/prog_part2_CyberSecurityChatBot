# prog_part2_CyberSecurityChatBot

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


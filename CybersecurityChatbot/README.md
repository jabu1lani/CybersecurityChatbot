# 🔐 Cybersecurity Awareness Chatbot

## 📋 Project Overview
A WPF-based cybersecurity awareness chatbot that helps users learn about online safety through interactive conversations, quizzes, and task management.

## ✨ Features

### Part 1 & 2 Features
- **Keyword Recognition** - Responds to cybersecurity topics (passwords, phishing, privacy, etc.)
- **Sentiment Detection** - Identifies user emotions (worried, curious, frustrated, happy)
- **Memory Storage** - Remembers user name and favourite topics
- **Follow-up Questions** - Handles "tell me more" requests
- **Voice Greeting** - Plays a welcome audio message

### Part 3 Features
- **📝 Task Management System** - Add, view, complete, and delete cybersecurity tasks
- **🗄️ MySQL Database Integration** - Persistent task storage
- **🎮 Cybersecurity Quiz** - 10+ questions with immediate feedback
- **📋 Activity Log** - Tracks all user interactions and bot actions
- **⏰ Reminder System** - Set reminders for tasks
- **🔍 NLP Simulation** - Keyword detection for natural language processing

## 🚀 Installation Guide

### Prerequisites
- Windows 10/11
- Visual Studio 2022 or later
- MySQL Server 8.0 or later
- .NET 6.0 or later

### Step 1: Install MySQL
1. Download MySQL Installer from: https://dev.mysql.com/downloads/installer/
2. Choose "Developer Default" setup type
3. Set root password to: `MySecurePassword123` (or your preferred password)
4. Complete installation

### Step 2: Create Database
Open MySQL Command Line or MySQL Workbench and run:

```sql
CREATE DATABASE IF NOT EXISTS cybersecurity_bot;
USE cybersecurity_bot;

CREATE TABLE IF NOT EXISTS tasks (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    reminder_date DATETIME,
    is_completed BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

Main Features
1. Chatting with the Bot
Type any cybersecurity topic to learn about it
The bot understands: password, phishing, privacy, scam, malware, safe browsing
Follow up with tell me more for additional information
The bot detects your sentiment and responds appropriately

2. Task Management
Add a task: Type add task [task name]
Add description: Follow the prompts to add details
Set reminder: Choose yes and specify when (e.g., "tomorrow", "in 3 days")
View tasks: Type show tasks or list tasks
Complete task: Type complete task [number]
Delete task: Type delete task [number]

3. Quiz
Start quiz: Type quiz, game, or test my knowledge
Answer questions: Type A, B, C, D or the full answer
Get feedback: Immediate feedback with explanations
View score: Final score displayed at the end

4. Activity Log
View log: Type show activity log or what have you done for me
Shows recent actions: Last 5-10 actions with timestamps

5. Additional Commands
Help: Type help or what can I ask
Check-in: Type how are you
Exit: Type exit, quit, or bye

How to Use
Getting Started
Launch the application - The chatbot window will open
Enter your name - The bot will greet you and remember your name
Start chatting - Type your questions or commands in the input box
Press Enter or click the "Send" button to submit

Main Features
1. Chatting with the Bot
Type any cybersecurity topic to learn about i
The bot understands: password, phishing, privacy, scam, malware, safe browsing
Follow up with tell me more for additional information
The bot detects your sentiment and responds appropriately

2. Task Management
Add a task: Type add task [task name]
Add description: Follow the prompts to add details
Set reminder: Choose yes and specify when (e.g., "tomorrow", "in 3 days")
View tasks: Type show tasks or list tasks
Complete task: Type complete task [number]
Delete task: Type delete task [number]

3. Quiz
Start quiz: Type quiz, game, or test my knowledge
Answer questions: Type A, B, C, D or the full answer
Get feedback: Immediate feedback with explanations
View score: Final score displayed at the end

4. Activity Log
View log: Type show activity log or what have you done for me
Shows recent actions: Last 5-10 actions with timestamps

5. Additional Commands
Help: Type help or what can I ask
Check-in: Type how are you
Exit: Type exit, quit, or bye






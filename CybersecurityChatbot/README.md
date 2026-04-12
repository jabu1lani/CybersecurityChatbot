# Cybersecurity Awareness Chatbot - Part 1

## Project Overview
This command-line chatbot educates South African citizens about cybersecurity threats including phishing, malware, and social engineering. The bot provides interactive conversations about online safety practices.

## Features Implemented

### Core Features (Required for Part 1)
- 🎤 **Voice Greeting**: Plays a recorded WAV file when the application launches
- 🎨 **ASCII Art Display**: Shows a custom cybersecurity-themed logo
- 💬 **Personalized Conversations**: Uses the user's name throughout the session
- 🔐 **Cybersecurity Topics**: Covers password safety, phishing, and safe browsing
- ✅ **Input Validation**: Handles empty inputs and unrecognized queries gracefully
- 🎨 **Enhanced Console UI**: Uses colored text, borders, and visual formatting
- ⌨️ **Typing Effect**: Simulates natural conversation with character-by-character display

### Technical Implementation
- **Object-Oriented Design**: Separated concerns into three main classes
  - `Program.cs`: Application entry point
  - `Chatbot.cs`: Main conversation orchestration
  - `UIManager.cs`: All visual and audio presentation
  - `ResponseHandler.cs`: Input processing and response generation
- **GitHub Integration**: Minimum 6 commits with meaningful messages
- **CI/CD Pipeline**: GitHub Actions workflow for automated builds

## System Requirements
- **Operating System**: Windows (required for System.Media audio playback)
- **.NET Version**: .NET 6.0 or higher
- **Development Environment**: Visual Studio 2022 or VS Code with C# extensions
- **Audio**: Speakers or headphones for voice greeting


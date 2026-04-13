using System;
using System.Media; // Required for playing audio files
using System.IO; // Required for file path operations
using System.Threading; // Required for typing effect delay

namespace CybersecurityChatbot
{
    // Manages all user interface elements including:
    // - Audio playback (voice greeting)
    // - ASCII art display
    // - Colored console output
    // - Typing effects
    // - Visual formatting (borders, dividers)
    public class UIManager
    {
        // Path to the voice greeting audio file
        private readonly string _audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "audio", "greeting.wav");

        // Plays the voice greeting audio file when the application starts
        // Uses System.Media.SoundPlayer for WAV file playback
        // Includes error handling in case the audio file is missing or corrupted
        public void PlayVoiceGreeting()
        {
            try
            {
                if (File.Exists(_audioPath))
                {
                    using (var soundPlayer = new SoundPlayer(_audioPath))
                    {
                        soundPlayer.PlaySync();
                    }
                }
                else
                {
                    // Fallback message if audio file is missing
                    Console.WriteLine("[System]: Audio file not found. Continuing with text-only mode.");
                }
            }
            catch (Exception ex)
            {
                // Catch any audio playback errors (corrupt file, unsupported format, etc.)
                Console.WriteLine($"[System]: Could not play audio: {ex.Message}");
            }
        }


        // Displays the ASCII art logo for the Cybersecurity Awareness Bot
        // The art creates a visual identity for the chatbot
        public void DisplayAsciiArt()
        {
            string asciiArt = @"
    ╔══════════════════════════════════════════════════════════════════╗
    ║                                                                  ║
    ║      ██████╗██╗   ██╗██████╗ ███████╗██████╗                     ║
    ║     ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗                    ║
    ║     ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝                    ║
    ║     ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗                    ║
    ║     ╚██████╗   ██║   ██████╔╝███████╗██║  ██║                    ║
    ║      ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝                    ║
    ║                                                                  ║
    ║                                                                  ║
    ║                                                                  ║
    ║              ██████╗  ██████╗ ████████╗                          ║
    ║              ██╔══██╗██╔═══██╗╚══██╔══╝                          ║
    ║              ██████╔╝██║   ██║   ██║                             ║
    ║              ██╔══██╗██║   ██║   ██║                             ║
    ║              ██████╔╝╚██████╔╝   ██║                             ║
    ║              ╚═════╝  ╚═════╝    ╚═╝                             ║
    ║                                                                  ║
    ║           CYBERSECURITY AWARENESS ASSISTANT                      ║
    ║                                                                  ║
    ╚══════════════════════════════════════════════════════════════════╝
            ";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(asciiArt);
            Console.ResetColor();
        }

        // Displays the main welcome message with decorative formatting
        // Introduces the bot's purpose
        public void DisplayWelcomeMessage()
        {
            DisplayDivider();
            DisplayColoredText("WELCOME TO THE CYBERSECURITY AWARENESS BOT", ConsoleColor.Yellow);
            Console.WriteLine();
            DisplayColoredText("Your personal assistant for online safety education", ConsoleColor.White);
            Console.WriteLine();
            DisplayDivider();
            
            
        }

        // Shows a personalized greeting using the user's name
        // Also displays the list of topics the user can ask about
        public void DisplayPersonalisedGreeting(string userName)
        {
            DisplayDivider();
            DisplayColoredText($"Hello, {userName}! 👋", ConsoleColor.Green);
            Console.WriteLine();
            DisplayColoredText("I'm here to help you learn about staying safe online.", ConsoleColor.White);
            Console.WriteLine();
            DisplayColoredText("You can ask me about:", ConsoleColor.Yellow);
            Console.WriteLine();
            DisplayColoredText("• Password safety", ConsoleColor.Magenta);
            DisplayColoredText("  • Phishing scams", ConsoleColor.Magenta);
            DisplayColoredText("  • Safe browsing habits", ConsoleColor.Magenta);
            DisplayColoredText("  • General cybersecurity tips", ConsoleColor.Magenta);
            Console.WriteLine();
            
        }

        // Draws a visual separator line using box-drawing characters
        // Improves readability
        public void DisplayDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('═', 70));
            Console.ResetColor();
        }

        // Displays text in a specified color without changing the rest of the console
        public void DisplayColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        // Displays an error message when the user enters empty input
        public void DisplayEmptyInputResponse()
        {
            DisplayColoredText("\n[Bot]: ", ConsoleColor.Cyan);
            DisplayColoredText("I didn't quite understand that. Could you rephrase?\n", ConsoleColor.Yellow);
        }

        // Shows a farewell message when the user exits the application
        public void DisplayExitMessage(string userName)
        {
            DisplayDivider();
            DisplayColoredText($"\n[Bot]: ", ConsoleColor.Cyan);
            DisplayColoredText($"Stay safe online, {userName}! Remember: Think before you click! 🔒\n", ConsoleColor.Green);
            DisplayDivider();
        }

        // Simulates a typing effect by displaying characters one by one with delays
        // Creates a more natural, conversational feel
        public async void DisplayTypingEffect(string message)
        {
            DisplayColoredText("\n[Bot]: ", ConsoleColor.Cyan);

            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(15); // Typing effect delay
            }
            Console.WriteLine("\n");
        }
    }
}
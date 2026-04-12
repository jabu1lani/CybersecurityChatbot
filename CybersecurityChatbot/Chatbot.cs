using System;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    // Main chatbot class that orchestrates the entire conversation flow
    // This class manages user interaction, coordinates UI elements, and handles responses
    public class Chatbot
    {
        // Private fields to store UI manager and response handler instances
        private readonly UIManager _uiManager; // handles visial and audio
        private readonly ResponseHandler _responseHandler;// processes input and generates responses 
        private string _userName; // stores the username


        // Constructor - Initializes the chatbot with required dependencies
        public Chatbot()
        {
            _uiManager = new UIManager(); // Initialises the ui manager
            _responseHandler = new ResponseHandler(); // Initialises the response handler
        }


        // Main method that starts the chatbot experience
        public async Task StartAsync()
        {
            // Play voice greeting
            _uiManager.PlayVoiceGreeting();

            // Display ASCII art logo
            _uiManager.DisplayAsciiArt();

            // Display welcome message
            _uiManager.DisplayWelcomeMessage();

            // Get user's name
            _userName = GetUserName();

            // Personalised greeting
            _uiManager.DisplayPersonalisedGreeting(_userName);

            // Start conversation loop
            await StartConversationAsync();
        }

        // Prompts the user for their name with validation
        private string GetUserName()
        {
            // Display prompt asking for user's name
            _uiManager.DisplayColoredText("\n[Bot]: ", ConsoleColor.Cyan);
            _uiManager.DisplayColoredText("What's your name? ", ConsoleColor.White);

            string name = Console.ReadLine()?.Trim();

            // Input validation loop - continue asking until valid input is provided
            while (string.IsNullOrWhiteSpace(name))
            {
                _uiManager.DisplayColoredText("[Bot]: ", ConsoleColor.Cyan);
                _uiManager.DisplayColoredText("I didn't catch that. Please tell me your name: ", ConsoleColor.Yellow);
                name = Console.ReadLine()?.Trim();
            }

            // Return the validated name
            return name;
        }

        // Main conversation loop that continuously processes user input
        private async Task StartConversationAsync()
        {
            bool running = true;

            // Continue looping while the user hasn't chosen to exit
            while (running)
            {
                _uiManager.DisplayDivider();
                _uiManager.DisplayColoredText($"[{_userName}]: ", ConsoleColor.Green);
                string userInput = Console.ReadLine()?.Trim().ToLower();

                // Validate that user didn't just press Enter with empty input
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    _uiManager.DisplayEmptyInputResponse();
                    continue;
                }

                // Check if user wants to exit the conversation
                if (_responseHandler.IsExitCommand(userInput))
                {
                    _uiManager.DisplayExitMessage(_userName);
                    running = false;
                    continue;
                }

                // Generate appropriate response based on user input
                string response = _responseHandler.GetResponse(userInput, _userName);
                _uiManager.DisplayTypingEffect(response);

                // Small delay to prevent console from scrolling too quickly
                await Task.Delay(500);
            }
        }
    }
}
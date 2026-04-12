using System;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    // This class handles the application startup and initialization

    class Program
    {
        // The main method that starts the chatbot application Using async Task Main

        static async Task Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";

            // Create an instance of the Chatbot class which contains all bot logic
            var chatbot = new Chatbot();

            // Start the chatbot and wait for completion
            // The user will interact with the bot until they choose to exit
            await chatbot.StartAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
    // Handles all response generation logic for the chatbot
    public class ResponseHandler
    {
        // Dictionary mapping keywords to response-generating functions
        // The key is the keyword/phrase to match
        // The value is a function that takes a user name and returns a response string
        // StringComparer.OrdinalIgnoreCase makes matching case-insensitive
        private readonly Dictionary<string, Func<string, string>> _responsePatterns;

        // Constructor - Initializes the response pattern 
        public ResponseHandler()
        {
            _responsePatterns = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "how are you", (name) => GetHowAreYouResponse() },
                { "purpose", (name) => GetPurposeResponse() },
                { "what can i ask", (name) => GetWhatCanAskResponse() },
                { "password", (name) => GetPasswordSafetyResponse() },
                { "phishing", (name) => GetPhishingResponse() },
                { "safe browsing", (name) => GetSafeBrowsingResponse() },
                { "tips", (name) => GetGeneralTipsResponse() },
                { "hello", (name) => GetGreetingResponse(name) },
                { "hi", (name) => GetGreetingResponse(name) },
                { "help", (name) => GetHelpResponse() }
            };
        }

        // Main method to get a response based on user input
        // Searches for matching keywords and returns appropriate response
        public string GetResponse(string userInput, string userName)
        {
            string lowerInput = userInput.ToLower();

            foreach (var pattern in _responsePatterns)
            {
                if (lowerInput.Contains(pattern.Key))
                {
                    return pattern.Value(userName);
                }
            }

            return GetDefaultResponse();
        }


        // Checks if the user wants to exit the conversation
        // Supports multiple exit command variations
        public bool IsExitCommand(string input)
        {
            string lowerInput = input.ToLower();
            string[] exitCommands = { "exit", "quit", "bye", "goodbye" };

            foreach (var cmd in exitCommands)
            {
                if (lowerInput.Contains(cmd))
                    return true;
            }
            return false;
        }


        // Returns a random response to "How are you?" questions
        // Creates variety
        private string GetHowAreYouResponse()
        {
            string[] responses = {
                "I'm functioning perfectly, thank you for asking! Ready to help you stay secure online.",
                "All systems operational! I'm here and eager to share cybersecurity knowledge with you.",
                "Doing great! Nothing makes me happier than helping people protect themselves online."
            };
            return responses[new Random().Next(responses.Length)];
        }

        // Explains the chatbot's purpose to the user
        // Includes context about South African cybersecurity threats
        private string GetPurposeResponse()
        {
            return "My purpose is to educate South African citizens about cybersecurity threats like phishing, malware, and social engineering. I'm here to help you stay safe online!";
        }

        // Lists all the topics the user can ask about
        private string GetWhatCanAskResponse()
        {
            return "You can ask me about:\n• Password safety tips\n• How to spot phishing emails\n• Safe browsing practices\n• General cybersecurity advice\nJust type your question and I'll help!";
        }

        // Provides comprehensive password safety tips
        private string GetPasswordSafetyResponse()
        {
            return "🔐 PASSWORD SAFETY TIPS:\n" +
                   "1. Use long passwords (at least 12 characters)\n" +
                   "2. Combine uppercase, lowercase, numbers, and symbols\n" +
                   "3. Never reuse passwords across different sites\n" +
                   "4. Use a password manager like Bitwarden or LastPass\n" +
                   "5. Enable two-factor authentication (2FA) whenever possible";
        }

        // Educates users about identifying phishing attempts
        private string GetPhishingResponse()
        {
            return "🎣 PHISHING WARNING SIGNS:\n" +
                   "• Urgent or threatening language\n" +
                   "• Suspicious sender email addresses\n" +
                   "• Spelling and grammar mistakes\n" +
                   "• Requests for personal information\n" +
                   "• Links that don't match the displayed text\n\n" +
                   "NEVER click suspicious links or download attachments from unknown senders!";
        }

        // Provides safe web browsing practices
        private string GetSafeBrowsingResponse()
        {
            return "🌐 SAFE BROWSING HABITS:\n" +
                   "• Look for 'https://' and padlock icon in address bar\n" +
                   "• Don't download files from untrusted websites\n" +
                   "• Keep your browser and extensions updated\n" +
                   "• Use ad-blockers and anti-tracking tools\n" +
                   "• Clear your browsing data regularly";
        }

        // Provides general cybersecurity best practices
        private string GetGeneralTipsResponse()
        {
            return "💡 GENERAL CYBERSECURITY TIPS:\n" +
                   "• Keep all software updated\n" +
                   "• Use antivirus and firewall protection\n" +
                   "• Back up important data regularly\n" +
                   "• Be careful what you share on social media\n" +
                   "• Lock your devices when not in use";
        }

        // Returns a personalized greeting response
        private string GetGreetingResponse(string userName)
        {
            string[] greetings = {
                $"Hi {userName}! Ready to learn about cybersecurity?",
                $"Hello {userName}! What would you like to know about online safety?",
                $"Hey {userName}! Ask me about passwords, phishing, or safe browsing!"
            };
            return greetings[new Random().Next(greetings.Length)];
        }

        // Displays help information showing all available topics
        private string GetHelpResponse()
        {
            return "📚 AVAILABLE TOPICS:\n" +
                   "• Password safety\n" +
                   "• Phishing scams\n" +
                   "• Safe browsing\n" +
                   "• General tips\n\n" +
                   "Just type any of these keywords, or ask:\n" +
                   "- 'How are you?'\n" +
                   "- 'What's your purpose?'\n" +
                   "- 'What can I ask about?'\n\n" +
                   "Type 'exit' to quit the chatbot.";
        }

        // Returns a default response when no keywords match
        private string GetDefaultResponse()
        {
            string[] defaultResponses = {
                "I'm not sure about that. Could you ask about passwords, phishing, or safe browsing?",
                "Hmm, I don't have information on that topic. Try asking about cybersecurity tips!",
                "I didn't quite understand. Would you like me to show you what I can help with? (Type 'help')"
            };
            return defaultResponses[new Random().Next(defaultResponses.Length)];
        }
    }
}
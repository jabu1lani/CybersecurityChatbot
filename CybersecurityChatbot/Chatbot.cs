using System;
using System.Collections.Generic;
using System.IO;
using System.Media;

namespace CybersecurityChatbot
{
    public class ChatBot
    {
        private KeywordResponder _keywordResponder = null!;
        private SentimentDetector _sentimentDetector = null!;
        private MemoryStore _memoryStore = null!;
        private bool _awaitingName;
        private string? _lastTopic;
        private Random _random = null!;
        private string? _audioPath;

        public ChatBot()
        {
            Initialize();
        }

        private void Initialize()
        {
            _keywordResponder = new KeywordResponder();
            _sentimentDetector = new SentimentDetector();
            _memoryStore = new MemoryStore();
            _awaitingName = true;
            _random = new Random();
            _audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
        }

        public void PlayVoiceGreeting()
        {
            try
            {
                if (!string.IsNullOrEmpty(_audioPath) && File.Exists(_audioPath))
                {
                    using (var soundPlayer = new SoundPlayer(_audioPath))
                    {
                        soundPlayer.Play();
                    }
                }
            }
            catch (Exception)
            {
                // Silently fail if audio can't play
            }
        }

        public string GetGreeting()
        {
            return "🔐 Welcome to the Cybersecurity Awareness Bot! 🔐\n\n" +
                   "I'm your personal assistant for online safety education.\n" +
                   "What's your name?";
        }

        public string ProcessInput(string userInput)
        {
            string lowerInput = userInput.ToLower().Trim();

            // Step 1: Handle name capture if awaiting name
            if (_awaitingName)
            {
                if (!string.IsNullOrWhiteSpace(userInput))
                {
                    _memoryStore!.UserName = userInput.Trim();
                    _awaitingName = false;
                    return $"Nice to meet you, {_memoryStore.UserName}! 👋\n\n" +
                           "I'm here to help you learn about staying safe online.\n\n" +
                           "You can ask me about:\n" +
                           "• Password safety 🔐\n" +
                           "• Phishing scams 🎣\n" +
                           "• Privacy protection 🛡️\n" +
                           "• Malware prevention 🦠\n" +
                           "• Safe browsing habits 🌐\n\n" +
                           "Type 'help' to see all available commands!";
                }
            }

            // Step 2: Check for exit command
            if (IsExitCommand(lowerInput))
            {
                return GetExitMessage();
            }

            // Step 3: Check for special phrases
            if (lowerInput.Contains("how are you"))
            {
                return GetHowAreYouResponse();
            }

            if (lowerInput.Contains("help") || lowerInput.Contains("what can i ask"))
            {
                return GetHelpResponse();
            }

            // Step 4: Handle follow-up questions
            if (IsFollowUp(lowerInput) && !string.IsNullOrEmpty(_lastTopic))
            {
                return HandleFollowUp();
            }

            // Step 5: Detect sentiment
            Sentiment sentiment = _sentimentDetector!.Detect(userInput);
            string sentimentResponse = _sentimentDetector.GetSentimentResponse(sentiment);

            // Step 6: Check for keyword matches
            string? keywordResponse = _keywordResponder!.GetResponse(userInput);

            if (keywordResponse != null)
            {
                // Store the topic for follow-up questions
                string? matchedKeyword = _keywordResponder.GetMatchedKeyword(userInput);
                if (!string.IsNullOrEmpty(matchedKeyword))
                {
                    _lastTopic = matchedKeyword;
                }

                // Check if user is sharing their favourite topic
                if (lowerInput.Contains("interested in") || lowerInput.Contains("like"))
                {
                    string? topic = ExtractTopic(userInput);
                    if (!string.IsNullOrEmpty(topic))
                    {
                        _memoryStore!.FavouriteTopic = topic;
                        return sentimentResponse + keywordResponse +
                               $"\n\n💡 Great! I'll remember that you're interested in {topic}. " +
                               "I'll share more relevant tips with you!";
                    }
                }

                // Personalize response if user has a favourite topic
                if (!string.IsNullOrEmpty(_memoryStore!.FavouriteTopic) &&
                    keywordResponse.Contains(_memoryStore.FavouriteTopic))
                {
                    return sentimentResponse + keywordResponse +
                           $"\n\n✨ As someone interested in {_memoryStore.FavouriteTopic}, " +
                           "this tip is especially relevant for you!";
                }

                return sentimentResponse + keywordResponse;
            }

            // Step 7: Fallback response
            return GetFallbackResponse();
        }

        private bool IsExitCommand(string input)
        {
            string[] exitCommands = { "exit", "quit", "bye", "goodbye" };
            foreach (var cmd in exitCommands)
            {
                if (input.Contains(cmd))
                    return true;
            }
            return false;
        }

        private string GetExitMessage()
        {
            string name = string.IsNullOrEmpty(_memoryStore?.UserName) ? "friend" : _memoryStore.UserName;
            return $"Stay safe online, {name}! Remember: Think before you click! 🔒\n\n" +
                   "Feel free to come back anytime for more cybersecurity tips!";
        }

        private string GetHowAreYouResponse()
        {
            string[] responses = {
                "I'm functioning perfectly, thank you for asking! Ready to help you stay secure online.",
                "All systems operational! I'm here and eager to share cybersecurity knowledge with you.",
                "Doing great! Nothing makes me happier than helping people protect themselves online."
            };
            return responses[_random!.Next(responses.Length)];
        }

        private string GetHelpResponse()
        {
            return "📚 AVAILABLE COMMANDS AND TOPICS:\n\n" +
                   "🔐 Cybersecurity Topics:\n" +
                   "   • 'password' - Password safety tips\n" +
                   "   • 'phishing' - How to spot phishing scams\n" +
                   "   • 'privacy' - Protecting your personal information\n" +
                   "   • 'scam' - Recognizing common online scams\n" +
                   "   • 'malware' - Preventing malware infections\n" +
                   "   • 'safe browsing' - Safe web browsing practices\n\n" +
                   "💬 Conversation:\n" +
                   "   • 'tell me more' - Get more information about current topic\n" +
                   "   • 'explain more' - Deeper explanation\n" +
                   "   • 'how are you' - Check in with me\n" +
                   "   • 'help' - Show this menu\n" +
                   "   • 'exit/quit/bye' - End the conversation\n\n" +
                   "💡 Tip: I remember your name and favourite topics!";
        }

        private bool IsFollowUp(string input)
        {
            string[] followUpPhrases = {
                "tell me more", "explain more", "more details",
                "elaborate", "go on", "continue", "what else",
                "another tip", "give me another"
            };

            foreach (var phrase in followUpPhrases)
            {
                if (input.Contains(phrase))
                    return true;
            }
            return false;
        }

        private string HandleFollowUp()
        {
            if (string.IsNullOrEmpty(_lastTopic))
                return GetFallbackResponse();

            string? followUpResponse = _keywordResponder!.GetFollowUpResponse(_lastTopic);

            if (followUpResponse != null)
            {
                return $"📌 Continuing with {_lastTopic.ToUpper()}:\n\n{followUpResponse}";
            }

            string? response = _keywordResponder.GetResponse(_lastTopic);
            return (response ?? GetFallbackResponse()) +
                   "\n\nWould you like me to explain more about this topic?";
        }

        private string? ExtractTopic(string input)
        {
            string lowerInput = input.ToLower();
            string[] topics = { "password", "phishing", "privacy", "scam", "malware", "browsing" };

            foreach (var topic in topics)
            {
                if (lowerInput.Contains(topic))
                    return topic;
            }
            return null;
        }

        private string GetFallbackResponse()
        {
            string[] fallbacks = {
                "I'm not sure I understand. Can you try rephrasing? Try asking about passwords, phishing, or privacy!",
                "Hmm, I didn't quite catch that. Would you like to ask about cybersecurity topics like passwords or phishing?",
                "I'm not familiar with that. Type 'help' to see what I can help you with regarding online safety!"
            };

            return fallbacks[_random!.Next(fallbacks.Length)];
        }

        public string? GetUserName()
        {
            return _memoryStore?.UserName;
        }
    }
}
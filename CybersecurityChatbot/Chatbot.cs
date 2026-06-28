using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Linq;
using System.Text.RegularExpressions;

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
        private DatabaseHelper _dbHelper = null!;
        private ActivityLogger _activityLogger = null!;
        private QuizManager _quizManager = null!;

        // NLP state
        private bool _awaitingTaskTitle = false;
        private bool _awaitingTaskDescription = false;
        private bool _awaitingReminderDate = false;
        private string? _pendingTaskTitle;
        private string? _pendingTaskDescription;

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
            _dbHelper = new DatabaseHelper();
            _activityLogger = new ActivityLogger();
            _quizManager = new QuizManager();

            _activityLogger.LogAction("Chatbot initialized");
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
                    _activityLogger.LogAction($"User identified as: {_memoryStore.UserName}");
                    return $"Nice to meet you, {_memoryStore.UserName}! 👋\n\n" +
                           "I'm here to help you learn about staying safe online.\n\n" +
                           "You can ask me about:\n" +
                           "• Password safety 🔐\n" +
                           "• Phishing scams 🎣\n" +
                           "• Privacy protection 🛡️\n" +
                           "• Malware prevention 🦠\n" +
                           "• Safe browsing habits 🌐\n\n" +
                           "Type 'help' to see all available commands!\n\n" +
                           "🎮 Type 'quiz' to test your cybersecurity knowledge!\n" +
                           "📝 Type 'task' to manage your cybersecurity tasks!";
                }
            }

            // Check for quiz commands
            if (lowerInput.Contains("quiz") || lowerInput.Contains("game") ||
                lowerInput.Contains("test my knowledge") || lowerInput.Contains("play"))
            {
                return StartQuiz();
            }

            // Check if currently in quiz
            if (_quizManager.IsQuizActive())
            {
                return ProcessQuizAnswer(userInput);
            }

            // Check for activity log request
            if (lowerInput.Contains("show activity") || lowerInput.Contains("what have you done") ||
                lowerInput.Contains("activity log") || lowerInput.Contains("log"))
            {
                return _activityLogger.GetFormattedLog();
            }

            // Check for task management
            if (lowerInput.Contains("add task") || lowerInput.Contains("new task") ||
                lowerInput.Contains("create task") || lowerInput.Contains("task add"))
            {
                return HandleTaskAddition(userInput);
            }

            if (lowerInput.Contains("show tasks") || lowerInput.Contains("list tasks") ||
                lowerInput.Contains("view tasks") || lowerInput.Contains("my tasks"))
            {
                return ListTasks();
            }

            if (lowerInput.Contains("complete task") || lowerInput.Contains("mark task"))
            {
                return CompleteTask(userInput);
            }

            if (lowerInput.Contains("delete task") || lowerInput.Contains("remove task"))
            {
                return DeleteTask(userInput);
            }

            // Check for reminder addition
            if (lowerInput.Contains("remind me") || lowerInput.Contains("set reminder") ||
                lowerInput.Contains("remember to"))
            {
                return HandleReminder(userInput);
            }

            // Check for exit command
            if (IsExitCommand(lowerInput))
            {
                return GetExitMessage();
            }

            // Check for special phrases
            if (lowerInput.Contains("how are you"))
            {
                return GetHowAreYouResponse();
            }

            if (lowerInput.Contains("help") || lowerInput.Contains("what can i ask"))
            {
                return GetHelpResponse();
            }

            // Handle follow-up questions
            if (IsFollowUp(lowerInput) && !string.IsNullOrEmpty(_lastTopic))
            {
                return HandleFollowUp();
            }

            // Detect sentiment
            Sentiment sentiment = _sentimentDetector!.Detect(userInput);
            string sentimentResponse = _sentimentDetector.GetSentimentResponse(sentiment);

            // Check for keyword matches
            string? keywordResponse = _keywordResponder!.GetResponse(userInput);

            if (keywordResponse != null)
            {
                string? matchedKeyword = _keywordResponder.GetMatchedKeyword(userInput);
                if (!string.IsNullOrEmpty(matchedKeyword))
                {
                    _lastTopic = matchedKeyword;
                    _activityLogger.LogAction($"Discussed topic: {matchedKeyword}");
                }

                if (!string.IsNullOrEmpty(_memoryStore!.FavouriteTopic) &&
                    keywordResponse.Contains(_memoryStore.FavouriteTopic))
                {
                    return sentimentResponse + keywordResponse +
                           $"\n\n✨ As someone interested in {_memoryStore.FavouriteTopic}, " +
                           "this tip is especially relevant for you!";
                }

                return sentimentResponse + keywordResponse;
            }

            // Check if we're in a multi-step interaction
            if (_awaitingTaskTitle || _awaitingTaskDescription || _awaitingReminderDate)
            {
                return HandleTaskFlow(userInput);
            }

            // Fallback response
            return GetFallbackResponse();
        }

        private string StartQuiz()
        {
            if (_quizManager.IsQuizActive())
            {
                return "You're already in a quiz! Answer the current question below:";
            }

            _quizManager.StartQuiz();
            _activityLogger.LogAction("Started cybersecurity quiz");
            return "🎮 Let's test your cybersecurity knowledge!\n\n" +
                   "I'll ask you 10 questions. Are you ready?\n" +
                   _quizManager.GetCurrentQuestion();
        }

        private string ProcessQuizAnswer(string userInput)
        {
            bool correct = _quizManager.AnswerQuestion(userInput);
            string feedback = correct ? "✅ Correct!" : $"❌ Incorrect. {_quizManager.GetLastAnswerFeedback()}";

            if (_quizManager.IsQuizComplete())
            {
                int score = _quizManager.GetScore();
                string scoreMessage = GetScoreMessage(score);
                _activityLogger.LogAction($"Completed quiz with score: {score}/10");
                return $"{feedback}\n\n{scoreMessage}\n\n" +
                       "🎮 Quiz complete! Type 'quiz' to play again or ask me about cybersecurity topics!";
            }

            return $"{feedback}\n\n{_quizManager.GetCurrentQuestion()}";
        }

        private string GetScoreMessage(int score)
        {
            if (score >= 8) return "🏆 Excellent! You're a cybersecurity pro!";
            if (score >= 6) return "👏 Good job! Keep learning to become a pro!";
            if (score >= 4) return "📚 Not bad! Review some topics to improve your score!";
            return "💪 Keep learning! Every expert was once a beginner!";
        }

        private string HandleTaskAddition(string userInput)
        {
            // Extract task title
            string taskTitle = ExtractTaskTitle(userInput);
            if (!string.IsNullOrEmpty(taskTitle))
            {
                _pendingTaskTitle = taskTitle;
                _pendingTaskDescription = "";
                _awaitingTaskDescription = true;
                return $"Task '{taskTitle}' added! Would you like to add a description? (Type 'skip' to skip)";
            }

            _awaitingTaskTitle = true;
            return "What would you like to call this task? (e.g., 'Enable two-factor authentication')";
        }

        private string HandleTaskFlow(string userInput)
        {
            if (_awaitingTaskTitle)
            {
                _pendingTaskTitle = userInput;
                _awaitingTaskTitle = false;
                _awaitingTaskDescription = true;
                return $"Task '{_pendingTaskTitle}' added! Would you like to add a description? (Type 'skip' to skip)";
            }

            if (_awaitingTaskDescription)
            {
                if (userInput.ToLower() != "skip")
                {
                    _pendingTaskDescription = userInput;
                }
                _awaitingTaskDescription = false;
                _awaitingReminderDate = true;
                return $"Description saved! Would you like to set a reminder for '{_pendingTaskTitle}'? (Type 'yes' or 'no')";
            }

            if (_awaitingReminderDate)
            {
                if (userInput.ToLower() == "yes" || userInput.ToLower() == "y")
                {
                    return "When would you like to be reminded? (e.g., 'tomorrow', 'in 3 days', or a specific date like '2026-07-01')";
                }
                else
                {
                    // Save task without reminder
                    SaveTask(_pendingTaskTitle, _pendingTaskDescription, null);
                    _awaitingReminderDate = false;
                    return $"Task '{_pendingTaskTitle}' saved successfully! 📝\n\n" +
                           "You can view your tasks by typing 'show tasks'.";
                }
            }

            // Parse reminder date
            DateTime? reminderDate = ParseReminderDate(userInput);
            if (reminderDate.HasValue)
            {
                SaveTask(_pendingTaskTitle, _pendingTaskDescription, reminderDate.Value);
                _awaitingReminderDate = false;
                return $"✅ Task '{_pendingTaskTitle}' saved with reminder set for {reminderDate.Value:yyyy-MM-dd HH:mm}!\n\n" +
                       "📝 You can view your tasks by typing 'show tasks'.";
            }

            return "I couldn't understand the date. Please specify a date like 'tomorrow', 'in 3 days', or '2026-07-01'.";
        }

        private void SaveTask(string title, string description, DateTime? reminderDate)
        {
            int taskId = _dbHelper.AddTask(title, description, reminderDate);
            string actionLog = $"Task added: '{title}'";
            if (reminderDate.HasValue)
            {
                actionLog += $" (Reminder set for {reminderDate.Value:yyyy-MM-dd HH:mm})";
            }
            _activityLogger.LogAction(actionLog);
        }

        private string ListTasks()
        {
            var tasks = _dbHelper.GetTasks();
            if (tasks.Count == 0)
            {
                return "📝 You don't have any tasks yet. Type 'add task' to create one!";
            }

            string result = "📝 YOUR CYBERSECURITY TASKS:\n\n";
            int counter = 1;
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "✅ COMPLETED" : "⏳ PENDING";
                result += $"{counter}. {task.Title} - {status}\n";
                if (!string.IsNullOrEmpty(task.Description))
                {
                    result += $"   📝 {task.Description}\n";
                }
                if (task.ReminderDate.HasValue)
                {
                    result += $"   ⏰ Reminder: {task.ReminderDate.Value:yyyy-MM-dd HH:mm}\n";
                }
                result += "\n";
                counter++;
            }
            return result + "\nType 'complete task [number]' or 'delete task [number]' to manage tasks.";
        }

        private string CompleteTask(string userInput)
        {
            var match = Regex.Match(userInput, @"\d+");
            if (match.Success)
            {
                int taskIndex = int.Parse(match.Value) - 1;
                var tasks = _dbHelper.GetTasks();
                if (taskIndex >= 0 && taskIndex < tasks.Count)
                {
                    var task = tasks[taskIndex];
                    _dbHelper.MarkTaskCompleted(task.Id);
                    _activityLogger.LogAction($"Task completed: '{task.Title}'");
                    return $"✅ Task '{task.Title}' marked as completed!";
                }
            }
            return "Please specify the task number. Example: 'complete task 1'";
        }

        private string DeleteTask(string userInput)
        {
            var match = Regex.Match(userInput, @"\d+");
            if (match.Success)
            {
                int taskIndex = int.Parse(match.Value) - 1;
                var tasks = _dbHelper.GetTasks();
                if (taskIndex >= 0 && taskIndex < tasks.Count)
                {
                    var task = tasks[taskIndex];
                    _dbHelper.DeleteTask(task.Id);
                    _activityLogger.LogAction($"Task deleted: '{task.Title}'");
                    return $"🗑️ Task '{task.Title}' deleted successfully!";
                }
            }
            return "Please specify the task number. Example: 'delete task 1'";
        }

        private string HandleReminder(string userInput)
        {
            string reminderText = ExtractReminderText(userInput);
            if (!string.IsNullOrEmpty(reminderText))
            {
                // Extract date
                string dateText = "";
                DateTime? reminderDate = null;

                if (userInput.Contains("tomorrow"))
                {
                    reminderDate = DateTime.Now.AddDays(1);
                    dateText = "tomorrow";
                }
                else if (userInput.Contains("in "))
                {
                    var match = Regex.Match(userInput, @"in (\d+) days?");
                    if (match.Success)
                    {
                        int days = int.Parse(match.Groups[1].Value);
                        reminderDate = DateTime.Now.AddDays(days);
                        dateText = $"in {days} days";
                    }
                }
                else
                {
                    // Try to parse a date
                    var dateMatch = Regex.Match(userInput, @"(\d{4}-\d{2}-\d{2})");
                    if (dateMatch.Success)
                    {
                        if (DateTime.TryParse(dateMatch.Groups[1].Value, out DateTime parsedDate))
                        {
                            reminderDate = parsedDate;
                            dateText = parsedDate.ToString("yyyy-MM-dd");
                        }
                    }
                }

                if (reminderDate.HasValue)
                {
                    _dbHelper.AddTask($"Reminder: {reminderText}", $"Reminder set for {reminderText}", reminderDate);
                    _activityLogger.LogAction($"Reminder set: '{reminderText}' for {reminderDate.Value:yyyy-MM-dd HH:mm}");
                    return $"⏰ Reminder set for '{reminderText}' on {dateText}! I'll remind you then.";
                }
                else
                {
                    _dbHelper.AddTask($"Reminder: {reminderText}", $"Reminder set for {reminderText}", DateTime.Now.AddDays(1));
                    _activityLogger.LogAction($"Reminder set: '{reminderText}' (default: tomorrow)");
                    return $"⏰ Reminder set for '{reminderText}' for tomorrow!";
                }
            }

            return "What would you like me to remind you about? (e.g., 'remind me to update my password tomorrow')";
        }

        private string ExtractTaskTitle(string input)
        {
            var patterns = new string[] {
                @"add task\s+(?:called\s+)?['""]?(.+?)['""]?(?:\s+with|\s+to|\s+for|$)",
                @"new task\s+(?:called\s+)?['""]?(.+?)['""]?(?:\s+with|\s+to|\s+for|$)",
                @"task\s+(?:called\s+)?['""]?(.+?)['""]?(?:\s+with|\s+to|\s+for|$)",
                @"create task\s+(?:called\s+)?['""]?(.+?)['""]?(?:\s+with|\s+to|\s+for|$)"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Groups[1].Value.Trim();
                }
            }

            // If no clear title, use the whole input after "add task" etc.
            string[] prefixes = { "add task ", "new task ", "task ", "create task " };
            foreach (var prefix in prefixes)
            {
                if (input.ToLower().StartsWith(prefix))
                {
                    return input.Substring(prefix.Length).Trim();
                }
            }

            return null;
        }

        private string ExtractReminderText(string input)
        {
            // Remove common reminder phrases
            string[] prefixes = { "remind me to ", "set reminder to ", "remember to " };
            foreach (var prefix in prefixes)
            {
                if (input.ToLower().Contains(prefix))
                {
                    string text = input.Substring(input.ToLower().IndexOf(prefix) + prefix.Length);
                    // Remove date references
                    text = Regex.Replace(text, @"\s*(in \d+ days?|tomorrow|on \d{4}-\d{2}-\d{2}).*$", "");
                    return text.Trim();
                }
            }
            return input;
        }

        private DateTime? ParseReminderDate(string input)
        {
            input = input.ToLower().Trim();

            // Tomorrow
            if (input == "tomorrow" || input.Contains("tomorrow"))
            {
                return DateTime.Now.AddDays(1);
            }

            // In X days
            var match = Regex.Match(input, @"in (\d+) days?");
            if (match.Success)
            {
                int days = int.Parse(match.Groups[1].Value);
                return DateTime.Now.AddDays(days);
            }

            // In X weeks
            match = Regex.Match(input, @"in (\d+) weeks?");
            if (match.Success)
            {
                int weeks = int.Parse(match.Groups[1].Value);
                return DateTime.Now.AddDays(weeks * 7);
            }

            // Specific date (YYYY-MM-DD)
            match = Regex.Match(input, @"(\d{4}-\d{2}-\d{2})");
            if (match.Success)
            {
                if (DateTime.TryParse(match.Groups[1].Value, out DateTime date))
                {
                    return date;
                }
            }

            // Next week
            if (input.Contains("next week"))
            {
                return DateTime.Now.AddDays(7);
            }

            return null;
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
            _activityLogger.LogAction($"User {name} exited the chat");
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
                   "🎮 Games & Activities:\n" +
                   "   • 'quiz' - Test your cybersecurity knowledge\n" +
                   "   • 'show activity log' - View recent actions\n\n" +
                   "📝 Task Management:\n" +
                   "   • 'add task' - Create a new cybersecurity task\n" +
                   "   • 'show tasks' - View all your tasks\n" +
                   "   • 'complete task [number]' - Mark task as done\n" +
                   "   • 'delete task [number]' - Remove a task\n" +
                   "   • 'remind me to...' - Set a reminder\n\n" +
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
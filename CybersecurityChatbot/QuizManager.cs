using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions = new List<QuizQuestion>();
        private int _currentQuestionIndex = -1;
        private int _score = 0;
        private bool _isActive = false;
        private string? _lastAnswerFeedback;

        public QuizManager()
        {
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> {
                        "A) Reply with your password",
                        "B) Delete the email",
                        "C) Report the email as phishing",
                        "D) Ignore it"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams and protects others from falling victim."
                },
                new QuizQuestion
                {
                    Question = "Which of the following is a strong password?",
                    Options = new List<string> {
                        "A) password123",
                        "B) Correct-Horse-Battery-Staple",
                        "C) 123456",
                        "D) yourname"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "A strong password uses a combination of words, numbers, and symbols. 'Correct-Horse-Battery-Staple' is a good example of a passphrase."
                },
                new QuizQuestion
                {
                    Question = "What does 'https://' indicate in a website URL?",
                    Options = new List<string> {
                        "A) The website is safe",
                        "B) The connection is encrypted",
                        "C) The website is from a trusted source",
                        "D) It's a government website"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "HTTPS indicates that the connection between your browser and the website is encrypted, protecting your data."
                },
                new QuizQuestion
                {
                    Question = "True or False: Using the same password for multiple accounts is safe.",
                    Options = new List<string> {
                        "A) True",
                        "B) False"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Using the same password for multiple accounts is dangerous. If one account is breached, all your accounts become vulnerable."
                },
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    Options = new List<string> {
                        "A) Using two different passwords",
                        "B) Adding a second layer of security to your login",
                        "C) Sharing your password with two people",
                        "D) Changing your password twice a year"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "2FA adds an extra layer of security by requiring two forms of identification before granting access."
                },
                new QuizQuestion
                {
                    Question = "What should you do before downloading software from the internet?",
                    Options = new List<string> {
                        "A) Download it immediately",
                        "B) Check if it's from a reputable source",
                        "C) Share it with friends first",
                        "D) Disable your antivirus"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Always verify that software comes from a reputable source to avoid malware and security risks."
                },
                new QuizQuestion
                {
                    Question = "True or False: Public Wi-Fi networks are always safe to use without protection.",
                    Options = new List<string> {
                        "A) True",
                        "B) False"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Public Wi-Fi networks are often unsecured. Always use a VPN when accessing sensitive information on public networks."
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> {
                        "A) A type of fishing game",
                        "B) A cyber attack that steals personal information through fake communications",
                        "C) A programming language",
                        "D) A type of computer virus"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Phishing is a cyber attack where criminals impersonate legitimate organizations to steal sensitive information."
                },
                new QuizQuestion
                {
                    Question = "How often should you update your passwords?",
                    Options = new List<string> {
                        "A) Never",
                        "B) Every 5 years",
                        "C) Regularly, especially if you suspect a breach",
                        "D) Only when you forget them"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Regular password updates and immediate changes after potential breaches help maintain account security."
                },
                new QuizQuestion
                {
                    Question = "What is the best way to protect against malware?",
                    Options = new List<string> {
                        "A) Install antivirus software and keep it updated",
                        "B) Ignore all security updates",
                        "C) Click on all pop-up ads",
                        "D) Download from unknown sources"
                    },
                    CorrectAnswerIndex = 0,
                    Explanation = "Antivirus software, regular updates, and safe browsing habits are your best protection against malware."
                }
            };
        }

        public void StartQuiz()
        {
            _currentQuestionIndex = 0;
            _score = 0;
            _isActive = true;
            _lastAnswerFeedback = null;

            // Shuffle questions
            var random = new Random();
            _questions = _questions.OrderBy(x => random.Next()).ToList();
        }

        public bool AnswerQuestion(string userInput)
        {
            if (_currentQuestionIndex >= _questions.Count)
                return false;

            var question = _questions[_currentQuestionIndex];
            int answerIndex = ParseAnswer(userInput);

            bool correct = answerIndex == question.CorrectAnswerIndex;
            if (correct)
            {
                _score++;
                _lastAnswerFeedback = question.Explanation;
            }
            else
            {
                _lastAnswerFeedback = $"The correct answer was: {question.Options[question.CorrectAnswerIndex]}. {question.Explanation}";
            }

            _currentQuestionIndex++;
            return correct;
        }

        private int ParseAnswer(string input)
        {
            input = input.ToLower().Trim();

            // Try to parse number
            if (int.TryParse(input, out int number))
            {
                if (number >= 1 && number <= 4)
                    return number - 1;
            }

            // Try to parse letter
            char? letter = input.Length > 0 ? input[0] : '?';
            if (letter >= 'a' && letter <= 'd')
                return letter.Value - 'a';
            if (letter >= 'A' && letter <= 'D')
                return letter.Value - 'A';

            // Try to parse true/false
            if (input == "a" || input == "true")
                return 0;
            if (input == "b" || input == "false")
                return 1;

            return -1;
        }

        public string GetCurrentQuestion()
        {
            if (_currentQuestionIndex >= _questions.Count)
                return "Quiz complete!";

            var question = _questions[_currentQuestionIndex];
            string result = $"Question {_currentQuestionIndex + 1} of {_questions.Count}:\n\n";
            result += question.Question + "\n\n";
            foreach (var option in question.Options)
            {
                result += option + "\n";
            }
            return result;
        }

        public string GetLastAnswerFeedback()
        {
            return _lastAnswerFeedback ?? "No feedback available.";
        }

        public bool IsQuizActive()
        {
            return _isActive && _currentQuestionIndex < _questions.Count;
        }

        public bool IsQuizComplete()
        {
            return _isActive && _currentQuestionIndex >= _questions.Count;
        }

        public int GetScore()
        {
            return _score;
        }

        public int GetTotalQuestions()
        {
            return _questions.Count;
        }
    }

    public class QuizQuestion
    {
        public string? Question { get; set; }
        public List<string>? Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string? Explanation { get; set; }
    }
}
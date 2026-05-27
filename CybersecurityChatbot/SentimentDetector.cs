using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Happy
    }

    public class SentimentDetector
    {
        private Dictionary<Sentiment, List<string>> _triggerWords = null!;

        public SentimentDetector()
        {
            InitializeTriggerWords();
        }

        private void InitializeTriggerWords()
        {
            _triggerWords = new Dictionary<Sentiment, List<string>>
            {
                [Sentiment.Worried] = new List<string>
                {
                    "worried", "scared", "afraid", "anxious", "nervous",
                    "unsafe", "concerned", "fear", "panicked", "stress"
                },
                [Sentiment.Curious] = new List<string>
                {
                    "curious", "wondering", "interested", "want to know",
                    "how does", "explain", "learn", "understand", "tell me"
                },
                [Sentiment.Frustrated] = new List<string>
                {
                    "frustrated", "annoyed", "confused", "don't understand",
                    "angry", "upset", "difficult", "hard", "complicated"
                },
                [Sentiment.Happy] = new List<string>
                {
                    "great", "thanks", "helpful", "awesome", "love it",
                    "perfect", "excellent", "wonderful", "appreciate"
                }
            };
        }

        public Sentiment Detect(string userInput)
        {
            string lowerInput = userInput.ToLower();

            foreach (var sentiment in _triggerWords)
            {
                foreach (var word in sentiment.Value)
                {
                    if (lowerInput.Contains(word))
                    {
                        return sentiment.Key;
                    }
                }
            }

            return Sentiment.Neutral;
        }

        public string GetSentimentResponse(Sentiment sentiment)
        {
            switch (sentiment)
            {
                case Sentiment.Worried:
                    return "🤗 It's completely understandable to feel that way. " +
                           "Cybersecurity can seem overwhelming, but let me help you stay safe.\n\n";

                case Sentiment.Curious:
                    return "😊 That's great that you're curious about cybersecurity! " +
                           "Asking questions is the first step to staying safe online.\n\n";

                case Sentiment.Frustrated:
                    return "😅 I know cybersecurity can be frustrating at times. " +
                           "Let me break this down in a simpler way for you.\n\n";

                case Sentiment.Happy:
                    return "😄 I'm glad to hear that! Staying positive about security " +
                           "makes learning easier. Here's something useful:\n\n";

                default:
                    return "";
            }
        }
    }
}
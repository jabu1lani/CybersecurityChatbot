using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class MemoryStore
    {
        public string? UserName { get; set; }
        public string? FavouriteTopic { get; set; }
        private Dictionary<string, string> _additionalMemory = null!;

        public MemoryStore()
        {
            _additionalMemory = new Dictionary<string, string>();
        }

        public void Store(string key, string value)
        {
            if (_additionalMemory.ContainsKey(key))
            {
                _additionalMemory[key] = value;
            }
            else
            {
                _additionalMemory.Add(key, value);
            }
        }

        public string? Recall(string key)
        {
            if (_additionalMemory.ContainsKey(key))
            {
                return _additionalMemory[key];
            }
            return null;
        }

        public string GetPersonalisedOpener()
        {
            if (!string.IsNullOrEmpty(FavouriteTopic))
            {
                return $"As someone interested in {FavouriteTopic}, ";
            }
            return "";
        }

        public bool HasUserInfo()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}
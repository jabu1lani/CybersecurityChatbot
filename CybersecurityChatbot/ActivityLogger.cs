using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
    public class ActivityLogger
    {
        private List<string> _activityLog = new List<string>();
        private int _maxLogEntries = 10;

        public void LogAction(string action)
        {
            string timestampedAction = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {action}";
            _activityLog.Insert(0, timestampedAction);

            // Keep only last 10 entries
            if (_activityLog.Count > _maxLogEntries)
            {
                _activityLog = _activityLog.Take(_maxLogEntries).ToList();
            }
        }

        public List<string> GetRecentActions()
        {
            return _activityLog.ToList();
        }

        public string GetFormattedLog()
        {
            if (_activityLog.Count == 0)
            {
                return "No activities logged yet.";
            }

            string log = "📋 RECENT ACTIVITY LOG:\n\n";
            int counter = 1;
            foreach (var action in _activityLog)
            {
                log += $"{counter}. {action}\n";
                counter++;
            }
            return log;
        }

        public void ClearLog()
        {
            _activityLog.Clear();
        }
    }
}
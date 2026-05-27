using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
    public class KeywordResponder
    {
        private Dictionary<string, List<string>> _responses = null!;
        private Random _random = null!;
        private string? _lastMatchedKeyword;

        public KeywordResponder()
        {
            InitializeResponses();
        }

        private void InitializeResponses()
        {
            _random = new Random();
            _responses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
                {
                    "🔐 PASSWORD SAFETY TIPS:\n• Use long passwords (at least 12 characters)\n• Combine uppercase, lowercase, numbers, and symbols\n• Never reuse passwords across different sites\n• Use a password manager like Bitwarden or LastPass\n• Enable two-factor authentication (2FA) whenever possible",

                    "🔑 STRONG PASSWORD GUIDE:\n• Avoid personal information (birthdays, names)\n• Use passphrases: 'Correct-Horse-Battery-Staple'\n• Change passwords only when compromised\n• Don't share passwords via email or text\n• Consider biometric authentication where available",

                    "💪 PASSWORD BEST PRACTICES:\n• Unique password for every account\n• Regular security checkups\n• Avoid dictionary words\n• Enable breach alerts in password manager\n• Never store passwords in plain text files"
                },

                ["phishing"] = new List<string>
                {
                    "🎣 PHISHING WARNING SIGNS:\n• Urgent or threatening language\n• Suspicious sender email addresses\n• Spelling and grammar mistakes\n• Requests for personal information\n• Links that don't match the displayed text\n\nNEVER click suspicious links or download attachments from unknown senders!",

                    "📧 SPOT PHISHING ATTEMPTS:\n• Hover over links before clicking\n• Verify sender email domain carefully\n• Don't trust unexpected attachments\n• Call the company directly to verify requests\n• Report phishing emails to your IT department",

                    "🛡️ PROTECT AGAINST PHISHING:\n• Use email filters and spam protection\n• Install browser anti-phishing extensions\n• Enable 2FA even if credentials are stolen\n• Educate family members about phishing\n• Never enter credentials from email links"
                },

                ["privacy"] = new List<string>
                {
                    "🛡️ PRIVACY PROTECTION TIPS:\n• Review privacy settings on social media\n• Limit personal information shared online\n• Use VPN on public Wi-Fi networks\n• Encrypt sensitive files and communications\n• Regularly clear browser cookies and cache",

                    "🔒 ONLINE PRIVACY GUIDE:\n• Use privacy-focused browsers (Firefox, Brave)\n• Install ad-blockers and tracker blockers\n• Opt out of data collection where possible\n• Use temporary email for signups\n• Check 'Have I Been Pwned' for breaches",

                    "👁️ CONTROL YOUR DIGITAL FOOTPRINT:\n• Google yourself regularly\n• Adjust Google account privacy settings\n• Use incognito/private browsing modes\n• Disable location tracking for apps\n• Review app permissions on your devices"
                },

                ["scam"] = new List<string>
                {
                    "⚠️ COMMON ONLINE SCAMS:\n• Lottery or prize scams\n• Romance scams on dating apps\n• Tech support callback scams\n• Fake online shopping websites\n• Investment and cryptocurrency scams\n\nIf it sounds too good to be true, it probably is!",

                    "📞 CALL AND SMS SCAMS:\n• Never share OTPs or PINs\n• Government agencies won't call for money\n• Hang up on 'Microsoft' security calls\n• Don't press numbers for 'refunds'\n• Block and report scam numbers",

                    "💸 PROTECT YOUR MONEY:\n• Use credit cards for online purchases\n• Research sellers before buying\n• Avoid wire transfers to strangers\n• Check for secure payment gateways\n• Trust your instincts - walk away if unsure"
                },

                ["malware"] = new List<string>
                {
                    "🦠 MALWARE PREVENTION:\n• Keep operating system updated\n• Install reputable antivirus software\n• Don't download from untrusted sites\n• Avoid clicking pop-up ads\n• Use ad-blockers and script blockers",

                    "💻 PROTECT YOUR DEVICE:\n• Enable firewall protection\n• Regular system scans\n• Be cautious with USB drives\n• Disable macros in Office documents\n• Keep backups of important files",

                    "🛡️ RECOGNIZE MALWARE SIGNS:\n• Slow computer performance\n• Unexpected pop-ups\n• Browser homepage changes\n• Unusual network activity\n• Files becoming inaccessible"
                },

                ["safe browsing"] = new List<string>
                {
                    "🌐 SAFE BROWSING HABITS:\n• Look for 'https://' and padlock icon\n• Don't download from untrusted websites\n• Keep browser and extensions updated\n• Use ad-blockers and anti-tracking tools\n• Clear browsing data regularly",

                    "🔍 BROWSER SECURITY TIPS:\n• Enable 'Do Not Track' requests\n• Disable autofill for sensitive data\n• Review saved passwords regularly\n• Use separate browser for banking\n• Install security-focused extensions"
                }
            };
        }

        public string? GetResponse(string userInput)
        {
            string lowerInput = userInput.ToLower();

            foreach (var keyword in _responses.Keys)
            {
                if (lowerInput.Contains(keyword))
                {
                    _lastMatchedKeyword = keyword;
                    List<string> responses = _responses[keyword];
                    return responses[_random.Next(responses.Count)];
                }
            }

            return null;
        }

        public string? GetMatchedKeyword(string userInput)
        {
            string lowerInput = userInput.ToLower();

            foreach (var keyword in _responses.Keys)
            {
                if (lowerInput.Contains(keyword))
                {
                    return keyword;
                }
            }

            return _lastMatchedKeyword;
        }

        public string? GetFollowUpResponse(string topic)
        {
            if (_responses.ContainsKey(topic))
            {
                List<string> responses = _responses[topic];
                string response = responses[_random.Next(responses.Count)];
                return $"Here's another tip about {topic}:\n\n{response}";
            }

            return null;
        }

        public List<string> GetAllKeywords()
        {
            return _responses.Keys.ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prog_part2_CyberSecurityChatBot
{
    internal class ResponseManager
    {
            private Dictionary<string, List<string>> topicResponses;
            private Random random = new Random();
            private string lastTopic = string.Empty;

            public string LastTopic
            {
                get { return lastTopic; }
                set { lastTopic = value; }
            }

            public ResponseManager()
            {
                InitializeResponses();
            }

            private void InitializeResponses()
            {
                topicResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
                {
                    ["hello"] = new List<string>
                {
                    "Hello! How can I help you with cybersecurity today?",
                    "Hi there! Ask me anything about online safety.",
                    "Greetings! I'm your security assistant."
                },
                    ["hi"] = new List<string>
                {
                    "Hi! What cybersecurity topic would you like to learn about?",
                    "Hello! Ask me about passwords, phishing, or safe browsing."
                },
                    ["how are you"] = new List<string>
                {
                    "I'm functioning well, thanks! Ready to help you stay safe online.",
                    "All systems go! What can I help you with today?"
                },
                    ["password"] = new List<string>
                {
                    "🔐 Create strong passwords with 12+ characters, mixing uppercase, lowercase, numbers, and symbols!",
                    "🔑 Never reuse passwords across different accounts. Use a password manager!",
                    "🛡️ Enable Two-Factor Authentication (2FA) whenever possible for extra security.",
                    "💡 A good password is like a toothbrush - don't share it and change it regularly!"
                },
                    ["phish"] = new List<string>
                {
                    "🎣 Phishing emails often have urgent language, spelling mistakes, or suspicious links. Always check the sender!",
                    "📧 Never click links in unexpected emails. Hover over the link to see the real URL.",
                    "🚨 If an email asks for personal info, contact the company directly using a known phone number.",
                    "⚠️ Urgent language like 'Your account will be closed' is a major red flag!"
                },
                    ["scam"] = new List<string>
                {
                    "⚠️ Scammers often create urgency. Take a moment to verify before acting!",
                    "📞 Never share personal information over phone calls you didn't initiate.",
                    "💡 If something seems too good to be true, it probably is a scam!",
                    "🔍 Research before you buy - check reviews and company credentials."
                },
                    ["privacy"] = new List<string>
                {
                    "🔒 Review privacy settings on social media regularly. Limit what you share publicly!",
                    "📱 Check app permissions on your phone. Many apps request unnecessary access.",
                    "🕵️ Use a VPN on public Wi-Fi to protect your browsing privacy.",
                    "🌐 Use private browsing mode for sensitive searches."
                },
                    ["brows"] = new List<string>
                {
                    "🌐 Look for HTTPS (padlock icon) before entering personal information online.",
                    "🔄 Keep your browser and extensions updated for the latest security patches.",
                    "🚫 Use ad-blockers and avoid clicking on pop-up advertisements."
                },
                    ["2fa"] = new List<string>
                {
                    "📱 Two-Factor Authentication adds a second verification step to your login.",
                    "🔐 Use authenticator apps (Google Authenticator, Microsoft Authenticator) instead of SMS.",
                    "✅ Enable 2FA on your email, banking, and social media accounts first.",
                    "🔑 2FA blocks 99.9% of automated account attacks!"
                },
                    ["malware"] = new List<string>
                {
                    "🦠 Malware includes viruses, worms, trojans, and spyware. Keep your antivirus updated!",
                    "📥 Only download software from official sources.",
                    "⚠️ Be cautious with email attachments, even from known senders.",
                    "🔍 Run regular antivirus scans on your computer."
                },
                    ["vpn"] = new List<string>
                {
                    "🔒 A VPN encrypts your internet traffic and hides your IP address.",
                    "🌐 Use a VPN on public Wi-Fi to protect your data from hackers.",
                    "🛡️ Choose a reputable VPN service that doesn't keep logs."
                },
                    ["backup"] = new List<string>
                {
                    "💾 Follow the 3-2-1 backup rule: 3 copies, 2 different media, 1 offsite!",
                    "☁️ Use cloud backup AND an external hard drive for important files.",
                    "🔄 Test your backups regularly to ensure you can restore data."
                },
                    ["help"] = new List<string>
                {
                    "💡 I can help you with: Passwords, Phishing, Scams, Privacy, Safe Browsing, 2FA, Malware, VPN, and Backups.",
                    "📚 Try asking: 'Tell me about passwords' or 'How to spot a phishing email?'",
                    "🔍 You can also ask for 'another tip' on the same topic!"
                },
                    ["purpose"] = new List<string>
                {
                    "🎯 I'm here to educate you about cybersecurity and help you stay safe online.",
                    "🛡️ My purpose is to teach you how to protect yourself from cyber threats."
                },
                    ["bye"] = new List<string>
                {
                    "👋 Stay safe online! Remember: cybersecurity is everyone's responsibility.",
                    "🛡️ Goodbye! Keep your digital life secure.",
                    "🔐 Take care! Never share your passwords with anyone."
                }
                };
            }

            public string GenerateResponse(string input)
            {
                string lowerInput = input.ToLower();

                foreach (var topic in topicResponses.Keys)
                {
                    if (lowerInput.Contains(topic))
                    {
                        lastTopic = topic;
                        return GetRandomResponse(topic);
                    }
                }

                if (lowerInput.Contains("another") || lowerInput.Contains("more tip") || lowerInput.Contains("tell me more"))
                {
                    if (!string.IsNullOrEmpty(lastTopic))
                    {
                        return GetRandomResponse(lastTopic);
                    }
                    return "Please ask about a specific topic first, like 'Tell me about passwords'.";
                }

                if (lowerInput.Contains("explain") || lowerInput.Contains("what is") || lowerInput.Contains("tell me about"))
                {
                    return "I'd love to help! Please be more specific. Try asking about passwords, phishing, scams, or privacy.";
                }

                if (lowerInput.Contains("bye") || lowerInput.Contains("goodbye") || lowerInput.Contains("exit") || lowerInput.Contains("quit"))
                {
                    return GetRandomResponse("bye");
                }

                string[] fallbacks = {
                "I'm not sure I understand. Try asking about passwords, phishing, or privacy.",
                "Hmm, I didn't quite get that. Type 'help' to see what topics I can assist with!",
                "I'm still learning! Could you ask about cybersecurity topics like passwords or scams?"
            };
                return fallbacks[random.Next(fallbacks.Length)];
            }

            private string GetRandomResponse(string topic)
            {
                if (topicResponses.ContainsKey(topic))
                {
                    var responses = topicResponses[topic];
                    return responses[random.Next(responses.Count)];
                }
                return "I have information on that topic. Could you be more specific?";
            }

            public bool IsGoodbye(string input)
            {
                string lowerInput = input.ToLower();
                return lowerInput.Contains("bye") || lowerInput.Contains("goodbye") ||
                       lowerInput.Contains("exit") || lowerInput.Contains("quit");
            }

            public bool ContainsInterest(string input)
            {
                return input.ToLower().Contains("interested in");
            }

            public bool IsFollowUp(string input)
            {
                string lowerInput = input.ToLower();
                return lowerInput.Contains("another") || lowerInput.Contains("more tip") ||
                       lowerInput.Contains("tell me more");
            }

            public string GetHelpMessage()
            {
                return "💡 Try asking about: passwords, phishing, scams, privacy, 2FA, malware, VPN, or backups.";
            }
        }
    }



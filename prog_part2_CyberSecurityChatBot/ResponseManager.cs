using System;
using System.Collections.Generic;

namespace prog_part2_CyberSecurityChatBot
{
    public class ResponseManager
    {
        private Dictionary<string, List<string>> topicResponses;
        private Dictionary<string, string[]> sentimentKeywords;
        private Dictionary<string, List<string>> sentimentResponses;
        private Random random = new Random();
        private string lastTopic = string.Empty;

        public string LastTopic
        {
            get { return lastTopic; }
            set { lastTopic = value; }
        }

        public ResponseManager()
        {
            InitializeSentimentKeywords();
            InitializeSentimentResponses();
            InitializeResponses();
        }

        private void InitializeSentimentKeywords()
        {
            sentimentKeywords = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                ["worried"] = new string[] { "worried", "scared", "nervous", "anxious", "afraid", "concerned", "panic", "fear", "terrified" },
                ["frustrated"] = new string[] { "frustrated", "annoyed", "angry", "mad", "upset", "irritated", "frustrating", "fed up" },
                ["curious"] = new string[] { "curious", "interested", "wondering", "want to learn", "tell me", "explain", "teach me" },
                ["confused"] = new string[] { "confused", "don't understand", "not clear", "unclear", "lost", "what does", "huh" },
                ["happy"] = new string[] { "happy", "great", "good", "awesome", "excellent", "fantastic", "wonderful", "amazing" },
                ["sad"] = new string[] { "sad", "upset", "depressed", "unhappy", "terrible", "awful", "bad", "feeling down" },
                ["grateful"] = new string[] { "thanks", "thank you", "appreciate", "helpful", "useful", "grateful", "appreciated" }
            };
        }

        private void InitializeSentimentResponses()
        {
            sentimentResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["worried"] = new List<string>
                {
                    "😟 It's completely understandable to feel worried about cybersecurity. Let me share some practical tips to help you feel more secure.",
                    "🛡️ Don't worry - most security issues can be prevented. Let me guide you through some simple steps to protect yourself.",
                    "💪 I understand your concern. The good news is that basic cybersecurity practices can prevent most common attacks!",
                    "🔒 Feeling worried is normal, but knowledge is power. Let me teach you how to stay safe online."
                },
                ["frustrated"] = new List<string>
                {
                    "😤 I understand this can be frustrating. Let's break it down into simple, manageable steps.",
                    "🎯 Cybersecurity can feel overwhelming, but I'm here to help. Let's start with one thing at a time.",
                    "💡 I hear your frustration. Let me explain this in a simpler way that's easier to understand.",
                    "🔄 Take a deep breath! I'll help you work through this step by step."
                },
                ["curious"] = new List<string>
                {
                    "🤔 That's a great question! I'm glad you're curious about cybersecurity. Here's what you should know:",
                    "📚 Excellent curiosity! Cybersecurity is fascinating. Let me share some important information with you.",
                    "💡 I love that you're asking questions! Knowledge is your best defense against cyber threats.",
                    "🎓 Great curiosity! Let me teach you about this topic."
                },
                ["confused"] = new List<string>
                {
                    "🤨 No worries - cybersecurity can be confusing at first. Let me explain it more clearly:",
                    "📖 I understand it's not clear. Let me break this down in simpler terms.",
                    "💡 Let me rephrase that to make it easier to understand.",
                    "🎯 It's okay to be confused. Let me give you a simpler explanation."
                },
                ["happy"] = new List<string>
                {
                    "😊 I'm glad to hear that! Staying positive helps with learning cybersecurity.",
                    "🎉 That's wonderful! A positive attitude is great for staying security-aware.",
                    "🌟 Awesome! Let's keep that positive energy going while we learn about online safety.",
                    "😄 Great to hear! How else can I help you with cybersecurity today?"
                },
                ["sad"] = new List<string>
                {
                    "😔 I'm sorry you're feeling this way. Cybersecurity can feel overwhelming, but I'm here to help.",
                    "💙 I understand. Taking small steps toward better security might help you feel more in control.",
                    "🤗 It's okay to feel down. Let me share some positive security tips that might help.",
                    "🌈 I hope things get better. In the meantime, let me help you stay safe online."
                },
                ["grateful"] = new List<string>
                {
                    "🙏 You're very welcome! I'm here to help keep you safe online.",
                    "😊 Happy to help! Cybersecurity is important and I'm glad you're taking it seriously.",
                    "💙 Thank you for your kind words! Let me know if you have more questions.",
                    "🎯 You're welcome! Stay safe and keep learning about online security."
                }
            };
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

        // Analyze sentiment from user input
        private string AnalyzeSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string lowerInput = input.ToLower();

            foreach (var sentiment in sentimentKeywords)
            {
                foreach (var keyword in sentiment.Value)
                {
                    if (lowerInput.Contains(keyword))
                    {
                        return sentiment.Key;
                    }
                }
            }
            return null;
        }

        // Get sentiment-based response
        private string GetSentimentResponse(string sentiment)
        {
            if (sentimentResponses.ContainsKey(sentiment))
            {
                var responses = sentimentResponses[sentiment];
                if (responses != null && responses.Count > 0)
                {
                    return responses[random.Next(responses.Count)];
                }
            }
            return null;
        }

        // Main method to generate response with sentiment analysis
        public string GenerateResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return GetHelpMessage();
            }

            string lowerInput = input.ToLower();

            // FIRST: Check for sentiment (if user is expressing emotion)
            string sentiment = AnalyzeSentiment(input);
            if (!string.IsNullOrEmpty(sentiment))
            {
                // If it's gratitude, we can still answer their question but acknowledge thanks
                if (sentiment == "grateful")
                {
                    string thanksResponse = GetSentimentResponse(sentiment);
                    // Check if they also asked a question
                    if (ContainsExplainRequest(lowerInput) || HasTopicKeyword(lowerInput))
                    {
                        return thanksResponse + " " + GetTopicResponse(lowerInput);
                    }
                    return thanksResponse;
                }

                // For other sentiments, acknowledge their feeling then provide help
                string sentimentResponse = GetSentimentResponse(sentiment);
                if (!string.IsNullOrEmpty(sentimentResponse))
                {
                    // If they also asked about a specific topic, combine responses
                    if (HasTopicKeyword(lowerInput))
                    {
                        return sentimentResponse + "\n\n" + GetTopicResponse(lowerInput);
                    }
                    return sentimentResponse + " What specific cybersecurity topic would you like to learn about?";
                }
            }

            // Check for goodbye
            if (IsGoodbye(lowerInput))
            {
                return GetRandomResponse("bye");
            }

            // Check for follow-up questions
            if (IsFollowUp(lowerInput))
            {
                if (!string.IsNullOrEmpty(lastTopic))
                {
                    return GetRandomResponse(lastTopic);
                }
                return "Please ask about a specific topic first, like 'Tell me about passwords'.";
            }

            // Check for specific topics
            string topicResponse = GetTopicResponse(lowerInput);
            if (topicResponse != null)
            {
                return topicResponse;
            }

            // Check for explanation requests
            if (ContainsExplainRequest(lowerInput))
            {
                return "I'd love to help! Please be more specific. Try asking about passwords, phishing, scams, or privacy.";
            }

            // Fallback responses
            return GetFallbackResponse();
        }

        // Helper method to check if input has any topic keyword
        private bool HasTopicKeyword(string input)
        {
            foreach (var topic in topicResponses.Keys)
            {
                if (input.Contains(topic))
                {
                    return true;
                }
            }
            return false;
        }

        // Get response for a specific topic
        private string GetTopicResponse(string input)
        {
            foreach (var topic in topicResponses.Keys)
            {
                if (input.Contains(topic))
                {
                    lastTopic = topic;
                    return GetRandomResponse(topic);
                }
            }
            return null;
        }

        private string GetRandomResponse(string topic)
        {
            if (topicResponses.ContainsKey(topic))
            {
                var responses = topicResponses[topic];
                if (responses != null && responses.Count > 0)
                {
                    return responses[random.Next(responses.Count)];
                }
            }
            return "I have information on that topic. Could you be more specific?";
        }

        private string GetFallbackResponse()
        {
            string[] fallbacks = {
                "I'm not sure I understand. Try asking about passwords, phishing, or privacy.",
                "Hmm, I didn't quite get that. Type 'help' to see what topics I can assist with!",
                "I'm still learning! Could you ask about cybersecurity topics like passwords or scams?"
            };
            return fallbacks[random.Next(fallbacks.Length)];
        }

        public bool IsGoodbye(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string lowerInput = input.ToLower();
            return lowerInput.Contains("bye") || lowerInput.Contains("goodbye") ||
                   lowerInput.Contains("exit") || lowerInput.Contains("quit");
        }

        public bool ContainsInterest(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return input.ToLower().Contains("interested in");
        }

        public bool IsFollowUp(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string lowerInput = input.ToLower();
            return lowerInput.Contains("another") || lowerInput.Contains("more tip") ||
                   lowerInput.Contains("tell me more") || lowerInput.Contains("more information");
        }

        private bool ContainsExplainRequest(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string lowerInput = input.ToLower();
            return lowerInput.Contains("explain") || lowerInput.Contains("what is") ||
                   lowerInput.Contains("tell me about") || lowerInput.Contains("what are");
        }

        public string GetHelpMessage()
        {
            return "💡 Try asking about: passwords, phishing, scams, privacy, 2FA, malware, VPN, or backups.";
        }

        // Additional utility method to get a tip for a specific topic
        public string GetTip(string topic)
        {
            if (topicResponses.ContainsKey(topic.ToLower()))
            {
                return GetRandomResponse(topic.ToLower());
            }
            return GetHelpMessage();
        }

        // Public method to get sentiment analysis (useful for logging or additional features)
        public string GetSentiment(string input)
        {
            return AnalyzeSentiment(input);
        }
    }
}
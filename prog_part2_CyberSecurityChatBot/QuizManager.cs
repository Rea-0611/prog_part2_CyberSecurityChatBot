using System;
using System.Collections.Generic;

namespace prog_part2_CyberSecurityChatBot
{
    public class QuizManager
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex;
        private int score;
        private bool quizActive;
        private ActivityLogManager logManager;

        public bool QuizActive => quizActive;
        public int TotalQuestions => questions.Count;
        public int Score => score;
        public int CurrentQuestionNumber => currentQuestionIndex + 1;

        public QuizManager(ActivityLogManager logMgr)
        {
            logManager = logMgr;
            InitializeQuestions();
            ResetQuiz();
        }

        private void InitializeQuestions()
        {
            questions = new List<QuizQuestion>
            {
                // ============================================
                // PASSWORDS (1-4)
                // ============================================
                new QuizQuestion
                {
                    Id = 1,
                    Category = "Passwords",
                    Question = "What is the minimum recommended length for a strong password?",
                    Options = new List<string> { "8 characters", "12 characters", "16 characters", "6 characters" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Security experts recommend at least 12 characters with a mix of uppercase, lowercase, numbers, and symbols."
                },
                new QuizQuestion
                {
                    Id = 2,
                    Category = "Passwords",
                    Question = "Which of these is the strongest password?",
                    Options = new List<string> { "12345678", "Password123", "CorrectHorseBatteryStaple", "qwerty" },
                    CorrectAnswerIndex = 2,
                    Explanation = "CorrectHorseBatteryStaple is a strong passphrase because it's long, memorable, and uses random words."
                },
                new QuizQuestion
                {
                    Id = 3,
                    Category = "Passwords",
                    Question = "What is a password manager?",
                    Options = new List<string> { "A physical device", "Software that stores passwords", "A type of lock", "A person who manages passwords" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Password managers securely store and generate unique passwords for all your accounts."
                },
                new QuizQuestion
                {
                    Id = 4,
                    Category = "Passwords",
                    Question = "Should you use the same password for multiple accounts?",
                    Options = new List<string> { "Yes, it's easier to remember", "Only for unimportant accounts", "Never", "Only if the password is strong" },
                    CorrectAnswerIndex = 2,
                    Explanation = "If one account is compromised, attackers will try the same password on all your other accounts."
                },

                // ============================================
                // PHISHING (5-8)
                // ============================================
                new QuizQuestion
                {
                    Id = 5,
                    Category = "Phishing",
                    Question = "What is phishing?",
                    Options = new List<string> { "A type of fishing hobby", "A scam to steal personal information", "A type of malware", "A security software" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Phishing is when attackers pretend to be legitimate companies to steal your information."
                },
                new QuizQuestion
                {
                    Id = 6,
                    Category = "Phishing",
                    Question = "What is a common sign of a phishing email?",
                    Options = new List<string> { "It uses your full name correctly", "It has urgent language", "It has no spelling mistakes", "It comes from a known contact" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Phishing emails often use urgent language to pressure you into acting quickly without thinking."
                },
                new QuizQuestion
                {
                    Id = 7,
                    Category = "Phishing",
                    Question = "What should you do if you receive a suspicious email asking for personal information?",
                    Options = new List<string> { "Reply with the information", "Delete it", "Report it and delete it", "Forward it to friends" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Report suspicious emails and then delete them. Never click on links or reply with personal info."
                },
                new QuizQuestion
                {
                    Id = 8,
                    Category = "Phishing",
                    Question = "What is 'vishing'?",
                    Options = new List<string> { "Phishing via text message", "Phishing via voice/phone calls", "Phishing via social media", "Phishing via postal mail" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Vishing (voice phishing) is a phone scam where attackers pretend to be legitimate organizations."
                },

                // ============================================
                // SOCIAL ENGINEERING (9-10)
                // ============================================
                new QuizQuestion
                {
                    Id = 9,
                    Category = "Social Engineering",
                    Question = "What is social engineering?",
                    Options = new List<string> { "Building social networks", "Manipulating people for information", "Creating fake profiles", "Engineering social events" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Social engineering uses psychological manipulation to trick people into giving up sensitive information."
                },
                new QuizQuestion
                {
                    Id = 10,
                    Category = "Social Engineering",
                    Question = "Is it safe to share your password with IT support over the phone?",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Legitimate IT support will NEVER ask for your password. This is a common social engineering tactic."
                },

                // ============================================
                // TWO-FACTOR AUTHENTICATION (11-13)
                // ============================================
                new QuizQuestion
                {
                    Id = 11,
                    Category = "2FA",
                    Question = "What is Two-Factor Authentication (2FA)?",
                    Options = new List<string> { "A second password", "A verification code sent to your phone/app", "A fingerprint only", "A security question" },
                    CorrectAnswerIndex = 1,
                    Explanation = "2FA adds an extra layer of security by requiring a second verification method beyond your password."
                },
                new QuizQuestion
                {
                    Id = 12,
                    Category = "2FA",
                    Question = "Which 2FA method is generally considered more secure?",
                    Options = new List<string> { "SMS text messages", "Authenticator app", "Security questions", "Email verification" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Authenticator apps (like Google Authenticator) are more secure than SMS because they're not vulnerable to SIM-swapping."
                },
                new QuizQuestion
                {
                    Id = 13,
                    Category = "2FA",
                    Question = "What is a backup code for?",
                    Options = new List<string> { "To reset your password", "To access your account if you lose your 2FA device", "To share with friends", "To save for emergencies only" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Backup codes allow you to access your account even if you lose your phone or authenticator app."
                },

                // ============================================
                // MALWARE & RANSOMWARE (14-17)
                // ============================================
                new QuizQuestion
                {
                    Id = 14,
                    Category = "Malware",
                    Question = "Which is a sign that your computer might have malware?",
                    Options = new List<string> { "It runs faster than usual", "Pop-up ads appear frequently", "The screen is brighter", "Files are organized" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Frequent pop-up ads, slow performance, and unexpected crashes are common signs of malware infection."
                },
                new QuizQuestion
                {
                    Id = 15,
                    Category = "Malware",
                    Question = "What is ransomware?",
                    Options = new List<string> { "Software that steals passwords", "Malware that encrypts files and demands payment", "A type of antivirus", "A type of firewall" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Ransomware encrypts your files and demands payment to unlock them. Never pay the ransom!"
                },
                new QuizQuestion
                {
                    Id = 16,
                    Category = "Malware",
                    Question = "What is the best defense against ransomware?",
                    Options = new List<string> { "Paying the ransom", "Regular backups", "Ignoring it", "Sending emails" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Regular backups ensure you can restore your files without paying the ransom."
                },
                new QuizQuestion
                {
                    Id = 17,
                    Category = "Malware",
                    Question = "Should you open email attachments from unknown senders?",
                    Options = new List<string> { "Yes, always", "Only if it looks interesting", "Never", "Only if it's a PDF" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Email attachments from unknown senders can contain malware. Always be cautious."
                },

                // ============================================
                // SAFE BROWSING (18-20)
                // ============================================
                new QuizQuestion
                {
                    Id = 18,
                    Category = "Safe Browsing",
                    Question = "What does the padlock icon in your browser address bar indicate?",
                    Options = new List<string> { "The website is completely safe", "The connection is encrypted (HTTPS)", "It's a government site", "You've been hacked" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The padlock indicates the connection is encrypted, but the site itself could still be unsafe."
                },
                new QuizQuestion
                {
                    Id = 19,
                    Category = "Safe Browsing",
                    Question = "Are HTTPS websites always safe to use?",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "HTTPS means the connection is encrypted, but the website itself could still be malicious."
                },
                new QuizQuestion
                {
                    Id = 20,
                    Category = "Safe Browsing",
                    Question = "What should you check before entering personal information online?",
                    Options = new List<string> { "The website design", "The HTTPS padlock", "The number of ads", "The website's color scheme" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Always look for HTTPS (padlock icon) before entering personal information on any website."
                },

                // ============================================
                // PRIVACY (21-23)
                // ============================================
                new QuizQuestion
                {
                    Id = 21,
                    Category = "Privacy",
                    Question = "What should you do before downloading an app?",
                    Options = new List<string> { "Check its privacy policy", "Download immediately", "Share it with friends", "Ignore permissions" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Always review what permissions an app requests. Many apps ask for unnecessary access to your data."
                },
                new QuizQuestion
                {
                    Id = 22,
                    Category = "Privacy",
                    Question = "What is a VPN used for?",
                    Options = new List<string> { "To make your internet faster", "To encrypt your internet traffic", "To download files faster", "To create social media accounts" },
                    CorrectAnswerIndex = 1,
                    Explanation = "A VPN (Virtual Private Network) encrypts your traffic and hides your online activity."
                },
                new QuizQuestion
                {
                    Id = 23,
                    Category = "Privacy",
                    Question = "Is it safe to share personal information on public Wi-Fi?",
                    Options = new List<string> { "Yes, it's safe", "No, it's risky", "Only if you trust the network", "Only during the day" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Public Wi-Fi is often unencrypted. Use a VPN to protect your data on public networks."
                },

                // ============================================
                // BACKUPS & UPDATES (24-26)
                // ============================================
                new QuizQuestion
                {
                    Id = 24,
                    Category = "Backups",
                    Question = "How often should you backup your important files?",
                    Options = new List<string> { "Once a year", "Weekly or monthly", "Never", "Only when you remember" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Regular backups (weekly or monthly) are essential. The 3-2-1 backup rule is recommended."
                },
                new QuizQuestion
                {
                    Id = 25,
                    Category = "Backups",
                    Question = "What is the 3-2-1 backup rule?",
                    Options = new List<string> { "3 copies, 2 different media, 1 offsite", "3 copies, 2 days, 1 week", "3 backups, 2 months, 1 year", "3 passwords, 2 emails, 1 phone" },
                    CorrectAnswerIndex = 0,
                    Explanation = "The 3-2-1 rule: 3 copies of your data, 2 different storage types, 1 copy offsite."
                },
                new QuizQuestion
                {
                    Id = 26,
                    Category = "Updates",
                    Question = "Should you install software updates immediately?",
                    Options = new List<string> { "Never install updates", "Only major updates", "Yes, immediately", "Only if you have time" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Software updates often contain important security patches. Install them as soon as possible."
                },

                // ============================================
                // CLOUD & AI (27-28)
                // ============================================
                new QuizQuestion
                {
                    Id = 27,
                    Category = "Cloud",
                    Question = "How can you protect data stored in the cloud?",
                    Options = new List<string> { "Use strong passwords and 2FA", "Share files publicly", "Never use cloud storage", "Only use it for non-sensitive data" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Use strong passwords, enable 2FA, and regularly review who has access to your cloud files."
                },
                new QuizQuestion
                {
                    Id = 28,
                    Category = "AI",
                    Question = "What are deepfakes?",
                    Options = new List<string> { "Fake videos created by AI", "A type of malware", "A social engineering tactic", "A password manager" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Deepfakes use AI to create realistic fake videos or audio of real people."
                },

                // ============================================
                // IDENTITY THEFT (29-30)
                // ============================================
                new QuizQuestion
                {
                    Id = 29,
                    Category = "Identity Theft",
                    Question = "What is identity theft?",
                    Options = new List<string> { "Someone steals your wallet", "Someone uses your personal info fraudulently", "Someone copies your ID card", "Someone steals your car" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Identity theft happens when someone uses your personal information fraudulently."
                },
                new QuizQuestion
                {
                    Id = 30,
                    Category = "Identity Theft",
                    Question = "How can you protect yourself from identity theft?",
                    Options = new List<string> { "Monitor your accounts", "Share your ID with everyone", "Use the same password for everything", "Ignore your bank statements" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Monitor your bank statements, use strong passwords, and regularly check your credit report."
                },

                // ============================================
                // ENCRYPTION & ZERO-DAY (31-32)
                // ============================================
                new QuizQuestion
                {
                    Id = 31,
                    Category = "Encryption",
                    Question = "What is encryption?",
                    Options = new List<string> { "A type of password", "Converting data into unreadable code", "Deleting files", "Copying files" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Encryption protects your data by converting it into unreadable code that only authorized users can decrypt."
                },
                new QuizQuestion
                {
                    Id = 32,
                    Category = "Zero-Day",
                    Question = "What is a zero-day attack?",
                    Options = new List<string> { "An attack on day zero", "An attack that exploits unknown vulnerabilities", "An attack on your bank account", "A type of malware" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Zero-day attacks exploit unknown vulnerabilities before they are patched by the software vendor."
                },

                // ============================================
                // GENERAL SECURITY (33-35)
                // ============================================
                new QuizQuestion
                {
                    Id = 33,
                    Category = "General Security",
                    Question = "What is the most common way attackers gain access to systems?",
                    Options = new List<string> { "Hacking passwords", "Human error and social engineering", "Malware attacks", "Physical break-ins" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Human error, such as falling for phishing emails, is the most common entry point for attackers."
                },
                new QuizQuestion
                {
                    Id = 34,
                    Category = "General Security",
                    Question = "What should you do if you suspect a security breach?",
                    Options = new List<string> { "Ignore it", "Change passwords immediately", "Wait and see", "Call the police first" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Immediately change passwords, contact IT support, and monitor your accounts for suspicious activity."
                },
                new QuizQuestion
                {
                    Id = 35,
                    Category = "General Security",
                    Question = "What is the best way to stay safe online?",
                    Options = new List<string> { "Use the same password everywhere", "Stay informed about cybersecurity", "Ignore security updates", "Share your passwords with friends" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Stay informed about cybersecurity, use strong passwords, enable 2FA, and keep your software updated."
                }
            };
        }

        public void ResetQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
            quizActive = true;
            ShuffleQuestions();
        }

        private void ShuffleQuestions()
        {
            Random rng = new Random();
            int n = questions.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = questions[k];
                questions[k] = questions[n];
                questions[n] = value;
            }
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (currentQuestionIndex < questions.Count)
                return questions[currentQuestionIndex];
            return null;
        }

        public bool SubmitAnswer(int selectedIndex, out bool isCorrect, out string explanation)
        {
            var question = GetCurrentQuestion();
            isCorrect = selectedIndex == question.CorrectAnswerIndex;
            explanation = question.Explanation;

            if (isCorrect)
                score++;

            logManager?.AddLog("Quiz Answer", $"Q{currentQuestionIndex + 1}: {(isCorrect ? "Correct ✓" : "Incorrect ✗")} - {question.Category}");

            currentQuestionIndex++;
            return currentQuestionIndex >= questions.Count;
        }

        public string GetFinalFeedback()
        {
            int percentage = (score * 100) / questions.Count;

            if (percentage >= 90)
                return "🏆 Outstanding! You're a cybersecurity expert! You have excellent knowledge of online safety!";
            else if (percentage >= 75)
                return "🌟 Great job! You have a strong understanding of cybersecurity. Keep up the good work!";
            else if (percentage >= 60)
                return "👍 Good job! You have a solid foundation in cybersecurity. Review the topics you missed!";
            else if (percentage >= 40)
                return "📚 Not bad! You have some knowledge, but there's room for improvement. Review the explanations!";
            else
                return "💪 Keep learning! Cybersecurity is important and there's always more to learn. Try the quiz again!";
        }
    }
}
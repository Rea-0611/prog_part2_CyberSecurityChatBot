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
                // PHISHING
                // ============================================
                new QuizQuestion
                {
                    Id = 1,
                    Category = "Phishing",
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams and protects others from falling victim."
                },
                new QuizQuestion
                {
                    Id = 2,
                    Category = "Phishing",
                    Question = "What is a common sign of a phishing email?",
                    Options = new List<string> { "It uses your full name correctly", "It has urgent language and asks for personal info", "It has no spelling mistakes", "It comes from a known contact" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Phishing emails often use urgent language to pressure you into acting quickly."
                },
                new QuizQuestion
                {
                    Id = 3,
                    Category = "Phishing",
                    Question = "What is 'vishing'?",
                    Options = new List<string> { "Phishing via text message", "Phishing via voice/phone calls", "Phishing via social media", "Phishing via postal mail" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Vishing (voice phishing) is a phone scam where attackers pretend to be from legitimate organizations."
                },
                // ============================================
                // PASSWORDS
                // ============================================
                new QuizQuestion
                {
                    Id = 4,
                    Category = "Passwords",
                    Question = "Which of these is a strong password?",
                    Options = new List<string> { "123456", "password", "CorrectHorseBatteryStaple", "qwerty" },
                    CorrectAnswerIndex = 2,
                    Explanation = "A strong password uses a mix of words, numbers, and symbols."
                },
                new QuizQuestion
                {
                    Id = 5,
                    Category = "Passwords",
                    Question = "Using the same password for multiple accounts is:",
                    Options = new List<string> { "Safe if the password is strong", "Never safe", "Safe for non-important accounts", "Only safe with 2FA" },
                    CorrectAnswerIndex = 1,
                    Explanation = "If one account is compromised, attackers can access all your accounts that use the same password."
                },
                new QuizQuestion
                {
                    Id = 6,
                    Category = "Passwords",
                    Question = "How often should you change your passwords?",
                    Options = new List<string> { "Every day", "Every week", "Every 3-6 months or if compromised", "Never" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Regular password changes are good practice, but change immediately if you suspect a breach."
                },
                // ============================================
                // TWO-FACTOR AUTHENTICATION
                // ============================================
                new QuizQuestion
                {
                    Id = 7,
                    Category = "Authentication",
                    Question = "What is Two-Factor Authentication (2FA)?",
                    Options = new List<string> { "A second password", "A verification code sent to your phone or app", "A fingerprint scan only", "A security question" },
                    CorrectAnswerIndex = 1,
                    Explanation = "2FA adds an extra layer of security by requiring a second verification method."
                },
                new QuizQuestion
                {
                    Id = 8,
                    Category = "Authentication",
                    Question = "Which 2FA method is generally considered more secure?",
                    Options = new List<string> { "SMS text messages", "Authenticator app (Google/Microsoft Authenticator)", "Security questions", "Email verification" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Authenticator apps are more secure than SMS because they are not vulnerable to SIM-swapping attacks."
                },
                // ============================================
                // MALWARE
                // ============================================
                new QuizQuestion
                {
                    Id = 9,
                    Category = "Malware",
                    Question = "Which is a sign that your computer might have malware?",
                    Options = new List<string> { "It runs faster than usual", "Pop-up ads appear frequently", "The screen is brighter", "Files are organized perfectly" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Frequent pop-up ads, slow performance, and unexpected crashes are common signs of malware infection."
                },
                new QuizQuestion
                {
                    Id = 10,
                    Category = "Malware",
                    Question = "What is ransomware?",
                    Options = new List<string> { "Software that steals passwords", "Malware that encrypts files and demands payment", "A type of antivirus", "A type of firewall" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Ransomware encrypts your files and demands payment to unlock them. Never pay the ransom!"
                },
                // ============================================
                // SAFE BROWSING & PRIVACY
                // ============================================
                new QuizQuestion
                {
                    Id = 11,
                    Category = "Safe Browsing",
                    Question = "What does the padlock icon in your browser address bar indicate?",
                    Options = new List<string> { "The website is completely safe", "The connection is encrypted (HTTPS)", "The website is a government site", "You have been hacked" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The padlock indicates the connection is encrypted, but the site itself could still be unsafe."
                },
                new QuizQuestion
                {
                    Id = 12,
                    Category = "Safe Browsing",
                    Question = "HTTPS websites are always safe to use.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "HTTPS means the connection is encrypted, but the website itself could still be malicious."
                },
                new QuizQuestion
                {
                    Id = 13,
                    Category = "Privacy",
                    Question = "What should you do before downloading an app?",
                    Options = new List<string> { "Check its privacy policy and permissions", "Download it immediately", "Share it with friends first", "Ignore the permissions" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Always review what permissions an app requests. Many apps ask for unnecessary access."
                },
                // ============================================
                // SOCIAL ENGINEERING
                // ============================================
                new QuizQuestion
                {
                    Id = 14,
                    Category = "Social Engineering",
                    Question = "It's safe to share your password with IT support over the phone.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Legitimate IT support will NEVER ask for your password. This is a common social engineering tactic."
                },
                new QuizQuestion
                {
                    Id = 15,
                    Category = "Social Engineering",
                    Question = "What is social engineering?",
                    Options = new List<string> { "Building social networks", "Manipulating people to reveal confidential information", "Creating fake social media profiles", "Engineering social events" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Social engineering uses psychological manipulation to trick people into giving up sensitive information."
                },
                // ============================================
                // BACKUPS
                // ============================================
                new QuizQuestion
                {
                    Id = 16,
                    Category = "Backups",
                    Question = "You should only backup your files once a year.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Regular backups (weekly or monthly) are essential. The 3-2-1 backup rule is recommended."
                },
                new QuizQuestion
                {
                    Id = 17,
                    Category = "Backups",
                    Question = "What is the 3-2-1 backup rule?",
                    Options = new List<string> { "3 copies, 2 different media, 1 offsite", "3 copies, 2 days, 1 week", "3 backups, 2 months, 1 year", "3 passwords, 2 emails, 1 phone" },
                    CorrectAnswerIndex = 0,
                    Explanation = "The 3-2-1 rule is a backup strategy: 3 copies, 2 different storage types, 1 copy offsite."
                },
                // ============================================
                // GENERAL SECURITY
                // ============================================
                new QuizQuestion
                {
                    Id = 18,
                    Category = "General Security",
                    Question = "What is the most common way attackers gain access to systems?",
                    Options = new List<string> { "Hacking passwords", "Human error and social engineering", "Malware attacks", "Physical break-ins" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Human error, such as falling for phishing emails, is the most common entry point for attackers."
                },
                new QuizQuestion
                {
                    Id = 19,
                    Category = "General Security",
                    Question = "What should you do if you suspect a security breach?",
                    Options = new List<string> { "Ignore it", "Change passwords immediately", "Wait and see", "Call the police first" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Immediately change passwords, contact IT support, and monitor your accounts for suspicious activity."
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
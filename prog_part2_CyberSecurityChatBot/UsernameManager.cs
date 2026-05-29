using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace prog_part2_CyberSecurityChatBot
{
    internal class UsernameManager
    {
            private string currentUsername = string.Empty;
            private string userFile = "users.txt";
            private string interestsFile = "user_interests.txt";
            private int messageCounter = 0;

            public string CurrentUsername
            {
                get { return currentUsername; }
            }

            public int MessageCounter
            {
                get { return messageCounter; }
                set { messageCounter = value; }
            }

            public bool ProcessUsername(string enteredName, out string welcomeMessage, out bool isNewUser)
            {
                isNewUser = false;

                if (string.IsNullOrWhiteSpace(enteredName))
                {
                    welcomeMessage = "Please enter a valid name.";
                    return false;
                }

                currentUsername = enteredName.Trim();

                if (!File.Exists(userFile))
                {
                    File.AppendAllText(userFile, "auto_create\n");
                }

                isNewUser = !CheckIfUserExists(currentUsername);

                if (isNewUser)
                {
                    File.AppendAllText(userFile, currentUsername + "\n");
                    welcomeMessage = $"Hey {currentUsername}! Welcome to AI Cybersecurity Assistant! 🎉\n\nI'm here to help you stay safe online.";
                }
                else
                {
                    welcomeMessage = $"Hey {currentUsername}! Welcome back! How can I help you today? 😊";
                }

                return true;
            }

            private bool CheckIfUserExists(string name)
            {
                if (!File.Exists(userFile)) return false;

                string[] users = File.ReadAllLines(userFile);
                foreach (string user in users)
                {
                    if (user.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }

            public void SaveUserInterest(string input)
            {
                string lowerInput = input.ToLower();
                string[] words = lowerInput.Split(' ');

                List<string> interests = new List<string>();
                bool afterInterested = false;

                foreach (string word in words)
                {
                    if (afterInterested && word.Length > 3 && !word.Contains("interested") && !word.Contains("in"))
                    {
                        string clean = Regex.Replace(word, @"[^a-zA-Z]", "");
                        if (clean.Length > 2)
                            interests.Add(clean);
                    }
                    if (word.Contains("interested"))
                        afterInterested = true;
                }

                if (interests.Count > 0)
                {
                    string interestList = string.Join(", ", interests);
                    string line = $"{currentUsername} interested in: {interestList}\n";

                    if (File.Exists(interestsFile))
                    {
                        string[] lines = File.ReadAllLines(interestsFile);
                        bool updated = false;

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].StartsWith(currentUsername))
                            {
                                lines[i] = line.Trim();
                                updated = true;
                                break;
                            }
                        }

                        if (updated)
                            File.WriteAllLines(interestsFile, lines);
                        else
                            File.AppendAllText(interestsFile, line);
                    }
                    else
                    {
                        File.AppendAllText(interestsFile, line);
                    }
                }
            }

            public string GetSavedInterests()
            {
                if (File.Exists(interestsFile))
                {
                    string[] lines = File.ReadAllLines(interestsFile);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith(currentUsername))
                        {
                            int colonIndex = line.IndexOf("interested in:");
                            if (colonIndex > 0)
                            {
                                return line.Substring(colonIndex + 14).Trim();
                            }
                        }
                    }
                }
                return string.Empty;
            }

            public void IncrementMessageCounter()
            {
                messageCounter++;
            }

            public bool ShouldShowInterestReminder()
            {
                return messageCounter > 0 && messageCounter % 5 == 0;
            }
        }
    }

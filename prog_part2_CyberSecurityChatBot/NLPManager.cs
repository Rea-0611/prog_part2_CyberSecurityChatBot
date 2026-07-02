using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prog_part2_CyberSecurityChatBot
{
    public class NLPManager
    {

            private ActivityLogManager logManager;

            // Intent patterns
            private Dictionary<string, List<string>> intentPatterns;

            public NLPManager(ActivityLogManager logMgr)
            {
                logManager = logMgr;
                InitializeIntentPatterns();
            }

            private void InitializeIntentPatterns()
            {
                intentPatterns = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
                {
                    ["add_task"] = new List<string>
                {
                    "add task", "new task", "create task", "add a task", "task to",
                    "remind me to", "remind me about", "set reminder", "create reminder",
                    "make a task", "add reminder", "create a task"
                },
                    ["show_tasks"] = new List<string>
                {
                    "show tasks", "view tasks", "list tasks", "my tasks", "what tasks",
                    "show my tasks", "display tasks", "task list"
                },
                    ["complete_task"] = new List<string>
                {
                    "complete task", "done task", "finish task", "mark complete",
                    "task done", "completed"
                },
                    ["delete_task"] = new List<string>
                {
                    "delete task", "remove task", "clear task", "task delete",
                    "remove this task", "cancel task"
                },
                    ["start_quiz"] = new List<string>
                {
                    "start quiz", "take quiz", "play quiz", "quiz me", "test me",
                    "cybersecurity quiz", "start game", "let's quiz"
                },
                    ["show_log"] = new List<string>
                {
                    "show activity log", "activity log", "what have you done",
                    "show log", "view log", "recent actions", "summary"
                },
                    ["help"] = new List<string>
                {
                    "help", "what can you do", "commands", "how to", "help me"
                }
                };
            }

            public string DetectIntent(string input)
            {
                string lowerInput = input.ToLower();

                foreach (var intent in intentPatterns)
                {
                    foreach (string pattern in intent.Value)
                    {
                        if (lowerInput.Contains(pattern))
                        {
                            logManager.AddLog("NLP Intent", $"Intent detected: {intent.Key} from input: '{input}'");
                            return intent.Key;
                        }
                    }
                }

                // Check for other common patterns
                if (lowerInput.Contains("what is") || lowerInput.Contains("tell me about") ||
                    lowerInput.Contains("how to") || lowerInput.Contains("explain"))
                {
                    return "cybersecurity_question";
                }

                return "unknown";
            }

            public string ExtractTaskName(string input)
            {
                string lowerInput = input.ToLower();
                string[] removePatterns = {
                "add task", "new task", "create task", "add a task",
                "remind me to", "remind me about", "task to",
                "create a task", "add reminder", "make a task"
            };

                string result = input;
                foreach (string pattern in removePatterns)
                {
                    if (lowerInput.Contains(pattern))
                    {
                        int index = lowerInput.IndexOf(pattern) + pattern.Length;
                        if (index < input.Length)
                            result = input.Substring(index).Trim();
                        else
                            result = "";
                        break;
                    }
                }

                // Clean up
                result = result.TrimStart(' ', '-', ':', '.');
                result = System.Text.RegularExpressions.Regex.Replace(result, @"^(to|for|about|with)\s+", "",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                return string.IsNullOrWhiteSpace(result) ? input : result;
            }

            public DateTime? ParseReminderDate(string input)
            {
                string lowerInput = input.ToLower();
                DateTime? date = null;

                if (lowerInput.Contains("tomorrow"))
                    date = DateTime.Now.AddDays(1);
                else if (lowerInput.Contains("in") && lowerInput.Contains("day"))
                {
                    int days = ExtractNumber(input);
                    if (days > 0)
                        date = DateTime.Now.AddDays(days);
                }
                else if (lowerInput.Contains("week"))
                {
                    int weeks = ExtractNumber(input);
                    if (weeks > 0)
                        date = DateTime.Now.AddDays(weeks * 7);
                    else
                        date = DateTime.Now.AddDays(7);
                }
                else if (lowerInput.Contains("month"))
                {
                    int months = ExtractNumber(input);
                    if (months > 0)
                        date = DateTime.Now.AddMonths(months);
                    else
                        date = DateTime.Now.AddMonths(1);
                }

                return date;
            }

            private int ExtractNumber(string text)
            {
                string digits = "";
                foreach (char c in text)
                {
                    if (char.IsDigit(c))
                        digits += c;
                }
                return digits.Length > 0 ? int.Parse(digits) : 0;
            }

            public bool IsYes(string input)
            {
                string lower = input.ToLower();
                return lower == "yes" || lower == "y" || lower.Contains("yeah") ||
                       lower.Contains("sure") || lower.Contains("okay") || lower.Contains("ok");
            }

            public bool IsNo(string input)
            {
                string lower = input.ToLower();
                return lower == "no" || lower == "n" || lower.Contains("nah") ||
                       lower.Contains("not") || lower.Contains("cancel");
            }
        }
    

}

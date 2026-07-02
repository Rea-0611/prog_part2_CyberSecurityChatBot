using System;
using System.Data;

namespace prog_part2_CyberSecurityChatBot
{
    public class TaskManager
    {
        private DatabaseHelper dbHelper;
        private ActivityLogManager logManager;
        private string currentUsername;

        public TaskManager(string username, ActivityLogManager logMgr, DatabaseHelper db)
        {
            currentUsername = username;
            logManager = logMgr;
            dbHelper = db;
        }

        public bool AddTask(string title, string description, DateTime? reminderDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;

            bool success = dbHelper.AddTask(currentUsername, title, description, reminderDate);

            if (success)
            {
                string logDetails = $"Task added: '{title}'";
                if (reminderDate.HasValue)
                    logDetails += $" (Reminder set for {reminderDate.Value.ToShortDateString()})";
                logManager.AddLog("Task Added", logDetails);
            }

            return success;
        }

        public DataTable GetTasks()
        {
            return dbHelper.GetTasks(currentUsername);
        }

        public bool UpdateTask(int taskId, string title, string description, DateTime? reminderDate)
        {
            bool success = dbHelper.UpdateTask(taskId, title, description, reminderDate);
            if (success)
            {
                logManager.AddLog("Task Updated", $"Task '{title}' updated");
            }
            return success;
        }

        public bool CompleteTask(int taskId, string title)
        {
            bool success = dbHelper.CompleteTask(taskId);
            if (success)
            {
                logManager.AddLog("Task Completed", $"Task completed: '{title}'");
            }
            return success;
        }

        public bool DeleteTask(int taskId, string title)
        {
            bool success = dbHelper.DeleteTask(taskId);
            if (success)
            {
                logManager.AddLog("Task Deleted", $"Task deleted: '{title}'");
            }
            return success;
        }

        public string ExtractTaskTitle(string input)
        {
            string title = input;
            string[] removeWords = { "add task", "remind me to", "remind me about", "add a task", "create task", "new task" };
            foreach (string word in removeWords)
            {
                if (title.ToLower().Contains(word))
                {
                    int index = title.ToLower().IndexOf(word) + word.Length;
                    if (index < title.Length)
                        title = title.Substring(index).Trim();
                    else
                        title = "";
                    break;
                }
            }
            return title;
        }

        public DateTime? ParseReminderFromInput(string input)
        {
            string lowerInput = input.ToLower();
            DateTime? date = null;

            if (lowerInput.Contains("tomorrow"))
                date = DateTime.Now.AddDays(1);
            else if (lowerInput.Contains("today"))
                date = DateTime.Now;
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

        public string FormatTasksForDisplay(DataTable tasks)
        {
            if (tasks.Rows.Count == 0)
                return "You have no tasks.";

            string result = "📋 Your Tasks:\n\n";
            for (int i = 0; i < tasks.Rows.Count; i++)
            {
                var row = tasks.Rows[i];
                string status = Convert.ToBoolean(row["is_completed"]) ? "✅" : "⬜";
                string title = row["title"].ToString();
                string reminder = row["reminder_date"] != DBNull.Value ?
                    $" (Reminder: {Convert.ToDateTime(row["reminder_date"]).ToShortDateString()})" : "";
                result += $"{status} {title}{reminder}\n";
            }
            return result;
        }
    }
}
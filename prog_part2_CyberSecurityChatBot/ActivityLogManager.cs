using System;
using System.Collections.Generic;
using System.Data;

namespace prog_part2_CyberSecurityChatBot
{
    public class ActivityLogManager
    {
        private DatabaseHelper dbHelper;
        private string currentUsername;
        private List<LogEntry> memoryLog;

        public ActivityLogManager(string username, DatabaseHelper db)
        {
            currentUsername = username;
            dbHelper = db;
            memoryLog = new List<LogEntry>();
        }

        public void AddLog(string action, string details)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            memoryLog.Insert(0, new LogEntry { Action = action, Details = details, Timestamp = timestamp });

            if (memoryLog.Count > 20)
                memoryLog.RemoveAt(memoryLog.Count - 1);

            dbHelper.AddActivityLog(currentUsername, action, details);
            System.Diagnostics.Debug.WriteLine($"[LOG] {action}: {details}");
        }

        public DataTable GetRecentLogs(int limit = 10)
        {
            return dbHelper.GetRecentActivity(currentUsername, limit);
        }

        public DataTable GetLogsByDate(DateTime fromDate, DateTime toDate)
        {
            return dbHelper.GetActivityByDate(currentUsername, fromDate, toDate);
        }

        public bool ClearLogs()
        {
            memoryLog.Clear();
            return dbHelper.ClearActivityLog(currentUsername);
        }

        public string GetLogSummary(int limit = 10)
        {
            var logs = GetRecentLogs(limit);
            if (logs.Rows.Count == 0)
                return "No recent activity to show.";

            string summary = "";
            for (int i = 0; i < logs.Rows.Count && i < limit; i++)
            {
                var row = logs.Rows[i];
                summary += $"{i + 1}. [{row["timestamp"]}] {row["action"]}: {row["details"]}\n";
            }
            return summary.TrimEnd('\n');
        }
    }

    public class LogEntry
    {
        public string Action { get; set; }
        public string Details { get; set; }
        public string Timestamp { get; set; }
    }
}
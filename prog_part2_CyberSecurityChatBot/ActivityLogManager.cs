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
            DataTable logs = GetRecentLogs(limit);
            
            // Check if table is null or empty safely
            if (logs == null || logs.Rows.Count == 0)
                return "No recent activity to show.";

            string summary = "";
            for (int i = 0; i < logs.Rows.Count && i < limit; i++)
            {
                DataRow row = logs.Rows[i];

                // 1. Safe extraction of the 'timestamp' column from SQL Server
                string dateStr = "Unknown Time";
                if (logs.Columns.Contains("timestamp") && row["timestamp"] != DBNull.Value)
                {
                    dateStr = Convert.ToDateTime(row["timestamp"]).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (logs.Columns.Contains("Timestamp") && row["Timestamp"] != DBNull.Value)
                {
                    dateStr = Convert.ToDateTime(row["Timestamp"]).ToString("yyyy-MM-dd HH:mm:ss");
                }

                // 2. Safe extraction of 'action' and 'details' columns
                string actionStr = logs.Columns.Contains("action") ? row["action"].ToString() : "Unknown Action";
                string detailsStr = logs.Columns.Contains("details") ? row["details"].ToString() : "";

                summary += $"{i + 1}. [{dateStr}] {actionStr}: {detailsStr}\n";
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
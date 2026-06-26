using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prog_part2_CyberSecurityChatBot.Database;

namespace prog_part2_CyberSecurityChatBot
{
    public class ActivityLogManager
    {

        
        
            private DatabaseHelper dbHelper;
            private string currentUsername;
            private List<LogEntry> memoryLog; // In-memory cache
            private int maxMemoryLog = 20;

            public ActivityLogManager(string username)
            {
                currentUsername = username;
                dbHelper = new DatabaseHelper();
                memoryLog = new List<LogEntry>();
            }

            public void AddLog(string action, string details)
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Add to memory cache
                memoryLog.Insert(0, new LogEntry
                {
                    Action = action,
                    Details = details,
                    Timestamp = timestamp
                });

                // Keep memory log size manageable
                if (memoryLog.Count > maxMemoryLog)
                    memoryLog.RemoveAt(memoryLog.Count - 1);

                // Add to database
                dbHelper.AddActivityLog(currentUsername, action, details);

                System.Diagnostics.Debug.WriteLine($"[LOG] {action}: {details}");
            }

            public DataTable GetRecentLogs(int limit = 10)
            {
                return dbHelper.GetRecentActivity(currentUsername, limit);
            }

            public List<LogEntry> GetMemoryLogs(int limit = 10)
            {
                int count = Math.Min(limit, memoryLog.Count);
                return memoryLog.GetRange(0, count);
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
                var logs = GetMemoryLogs(limit);
                if (logs.Count == 0)
                    return "No recent activity to show.";

                string summary = "";
                for (int i = 0; i < logs.Count; i++)
                {
                    summary += $"{i + 1}. [{logs[i].Timestamp}] {logs[i].Action}: {logs[i].Details}\n";
                }
                return summary.TrimEnd('\n');
            }

            public string GetLogSummaryWithDatabase(int limit = 10)
            {
                var dt = GetRecentLogs(limit);
                if (dt.Rows.Count == 0)
                    return "No recent activity in database.";

                string summary = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var row = dt.Rows[i];
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


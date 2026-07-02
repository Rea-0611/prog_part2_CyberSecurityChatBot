using Org.BouncyCastle.Tls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using System.Windows.Markup;

namespace prog_part2_CyberSecurityChatBot
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            // UPDATE THIS FOR YOUR SQL SERVER
            // For Windows Authentication:
            connectionString = @"(localdb)\MSSQLLocalDB; Initial Catalog = master; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False;";

            // For SQL Server Authentication:
            // connectionString = "Server=localhost;Database=cybersecurity_chatbot;User Id=sa;Password=yourpassword;";
        }

        public bool TestConnection()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Connection failed: {ex.Message}");
                return false;
            }
        }

        // ============================================
        // TASK OPERATIONS
        // ============================================

        public bool AddTask(string username, string title, string description, DateTime? reminderDate)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO tasks (username, title, description, reminder_date) 
                                    VALUES (@username, @title, @description, @reminderDate)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description ?? "");
                        cmd.Parameters.AddWithValue("@reminderDate", (object)reminderDate ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddTask Error: {ex.Message}");
                return false;
            }
        }

        public DataTable GetTasks(string username)
        {
            var dataTable = new DataTable();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT id, title, description, reminder_date, is_completed, created_at 
                                    FROM tasks WHERE username = @username ORDER BY created_at DESC";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetTasks Error: {ex.Message}");
            }
            return dataTable;
        }

        public bool UpdateTask(int taskId, string title, string description, DateTime? reminderDate)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE tasks 
                                    SET title = @title, description = @description, reminder_date = @reminderDate 
                                    WHERE id = @id";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description ?? "");
                        cmd.Parameters.AddWithValue("@reminderDate", (object)reminderDate ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        public bool CompleteTask(int taskId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE tasks SET is_completed = 1 WHERE id = @id";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        public bool DeleteTask(int taskId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM tasks WHERE id = @id";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        // ============================================
        // ACTIVITY LOG OPERATIONS
        // ============================================

        public bool AddActivityLog(string username, string action, string details)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO activity_log (username, action, details) 
                                    VALUES (@username, @action, @details)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@action", action);
                        cmd.Parameters.AddWithValue("@details", details);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddActivityLog Error: {ex.Message}");
                return false;
            }
        }

        public DataTable GetRecentActivity(string username, int limit = 10)
        {
            var dataTable = new DataTable();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT TOP (@limit) action, details, timestamp 
                                    FROM activity_log 
                                    WHERE username = @username 
                                    ORDER BY timestamp DESC";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@limit", limit);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetRecentActivity Error: {ex.Message}");
            }
            return dataTable;
        }

        // ✅ ADD THIS METHOD
        public DataTable GetActivityByDate(string username, DateTime fromDate, DateTime toDate)
        {
            var dataTable = new DataTable();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT action, details, timestamp 
                                    FROM activity_log 
                                    WHERE username = @username 
                                    AND timestamp BETWEEN @fromDate AND @toDate
                                    ORDER BY timestamp DESC";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetActivityByDate Error: {ex.Message}");
            }
            return dataTable;
        }

        // ✅ ADD THIS METHOD
        public bool ClearActivityLog(string username)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM activity_log WHERE username = @username";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ClearActivityLog Error: {ex.Message}");
                return false;
            }
        }

        // ============================================
        // QUIZ SCORE OPERATIONS
        // ============================================

        public bool SaveQuizScore(string username, int score, int total)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO quiz_scores (username, score, total) 
                                    VALUES (@username, @score, @total)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@score", score);
                        cmd.Parameters.AddWithValue("@total", total);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveQuizScore Error: {ex.Message}");
                return false;
            }
        }

        public DataTable GetQuizHistory(string username, int limit = 5)
        {
            var dataTable = new DataTable();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT TOP (@limit) score, total, date_taken 
                                    FROM quiz_scores 
                                    WHERE username = @username 
                                    ORDER BY date_taken DESC";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@limit", limit);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetQuizHistory Error: {ex.Message}");
            }
            return dataTable;
        }

        public int GetAverageQuizScore(string username)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT AVG(CAST(score AS DECIMAL)) FROM quiz_scores WHERE username = @username";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        var result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch { return 0; }
        }
    }
}
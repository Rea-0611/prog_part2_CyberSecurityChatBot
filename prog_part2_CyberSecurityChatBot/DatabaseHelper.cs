using System;
using System.Data;
using System.Data.SqlClient;

namespace prog_part2_CyberSecurityChatBot
{
    public class DatabaseHelper
    {
        // Connection string targeting your LocalDB instance with the mandatory security trust parameters
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=cybersecurity_chatbot;Trusted_Connection=True;TrustServerCertificate=True;";

        // 1. Connection Test method used at application startup
        public bool TestConnection()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Startup Connection Test Failed: " + ex.Message);
                    return false;
                }
            }
        }

        // ==========================================
        // ACTIVITY LOG METHODS (Handles any inputs!)
        // ==========================================

        public void AddActivityLog(string username, string action, string details)
        {
            string query = "INSERT INTO activity_log (username, action, details) VALUES (@username, @action, @details);";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 100).Value = username ?? "Unknown";
                    cmd.Parameters.Add("@action", SqlDbType.NVarChar, 255).Value = action ?? "";
                    cmd.Parameters.Add("@details", SqlDbType.NVarChar, -1).Value = details ?? "";

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error in AddActivityLog: " + ex.Message);
                    }
                }
            }
        }

        public DataTable GetRecentActivity(string username, int limit)
        {
            DataTable dt = new DataTable();
            string query = "SELECT TOP (@limit) id, username, action, details, timestamp FROM activity_log WHERE username = @username ORDER BY timestamp DESC;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 100).Value = username ?? "Unknown";
                    cmd.Parameters.Add("@limit", SqlDbType.Int).Value = limit;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            conn.Open();
                            adapter.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error in GetRecentActivity: " + ex.Message);
                        }
                    }
                }
            }
            return dt;
        }

        public DataTable GetActivityByDate(string username, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            string query = "SELECT id, username, action, details, timestamp FROM activity_log WHERE username = @username AND timestamp BETWEEN @fromDate AND @toDate ORDER BY timestamp DESC;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 100).Value = username ?? "Unknown";
                    cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                    cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            conn.Open();
                            adapter.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error in GetActivityByDate: " + ex.Message);
                        }
                    }
                }
            }
            return dt;
        }

        public bool ClearActivityLog(string username)
        {
            string query = "DELETE FROM activity_log WHERE username = @username;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 100).Value = username ?? "Unknown";

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error in ClearActivityLog: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // ==========================================
        // TASK MANAGEMENT METHODS
        // ==========================================

        public bool AddTask(string username, string title, string description, DateTime? reminderDate)
        {
            string query = "INSERT INTO tasks (username, title, description, reminder_date, is_completed) VALUES (@username, @title, @description, @reminderDate, 0);";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 100).Value = username ?? "Unknown";
                    cmd.Parameters.Add("@title", SqlDbType.NVarChar, 200).Value = title ?? "";
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar, -1).Value = description ?? "";
                    cmd.Parameters.Add("@reminderDate", SqlDbType.DateTime).Value = (object)reminderDate ?? DBNull.Value;

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error in AddTask: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public DataTable GetTasks(string username)
        {
            DataTable dt = new DataTable();
            string query = "SELECT id, title, description, reminder_date, is_completed, created_at FROM tasks WHERE username = @username ORDER BY created_at DESC;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 100).Value = username ?? "Unknown";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            conn.Open();
                            adapter.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error in GetTasks: " + ex.Message);
                        }
                    }
                }
            }
            return dt;
        }

        public bool UpdateTask(int taskId, string title, string description, DateTime? reminderDate)
        {
            string query = "UPDATE tasks SET title = @title, description = @description, reminder_date = @reminderDate WHERE id = @taskId;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@taskId", SqlDbType.Int).Value = taskId;
                    cmd.Parameters.Add("@title", SqlDbType.NVarChar, 200).Value = title ?? "";
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar, -1).Value = description ?? "";
                    cmd.Parameters.Add("@reminderDate", SqlDbType.DateTime).Value = (object)reminderDate ?? DBNull.Value;

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error in UpdateTask: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool CompleteTask(int taskId, bool isCompleted = true)
        {
            string query = "UPDATE tasks SET is_completed = @isCompleted WHERE id = @taskId;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@taskId", SqlDbType.Int).Value = taskId;
                    cmd.Parameters.Add("@isCompleted", SqlDbType.Bit).Value = isCompleted;

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error in CompleteTask: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeleteTask(int taskId)
        {
            string query = "DELETE FROM tasks WHERE id = @taskId;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@taskId", SqlDbType.Int).Value = taskId;

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error in DeleteTask: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // ==========================================
        // QUIZ MANAGEMENT METHODS
        // ==========================================

        public bool SaveQuizScore(string username, int score, int total)
        {
            string query = "INSERT INTO quiz_scores (username, score, total) VALUES (@username, @score, @total);";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 100).Value = username ?? "Unknown";
                    cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
                    cmd.Parameters.Add("@total", SqlDbType.Int).Value = total;

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error in SaveQuizScore: " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}
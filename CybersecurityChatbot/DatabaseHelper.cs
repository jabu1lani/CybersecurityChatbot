using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CybersecurityChatbot
{
    public class DatabaseHelper
    {
        private string _connectionString;
        private string _connectionString = "Server=localhost;Database=cybersecurity_bot;Uid=root;Pwd=yourpassword;";

        public DatabaseHelper()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS tasks (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        title VARCHAR(255) NOT NULL,
                        description TEXT,
                        reminder_date DATETIME,
                        is_completed BOOLEAN DEFAULT FALSE,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";
                using (var cmd = new MySqlCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int AddTask(string title, string description, DateTime? reminderDate = null)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO tasks (title, description, reminder_date) 
                                VALUES (@title, @description, @reminderDate);
                                SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@reminderDate", reminderDate ?? (object)DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public List<Task> GetTasks()
        {
            var tasks = new List<Task>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM tasks ORDER BY created_at DESC";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                            ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? null : reader.GetDateTime("reminder_date"),
                            IsCompleted = reader.GetBoolean("is_completed"),
                            CreatedAt = reader.GetDateTime("created_at")
                        });
                    }
                }
            }
            return tasks;
        }

        public void DeleteTask(int taskId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM tasks WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void MarkTaskCompleted(int taskId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
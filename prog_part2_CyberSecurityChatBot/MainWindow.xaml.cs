using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace prog_part2_CyberSecurityChatBot
{
    public partial class MainWindow : Window
    {
        // Managers
        private GreetingManager greetingManager;
        private UsernameManager usernameManager;
        private ResponseManager responseManager;

        // Part 3 Managers
        private DatabaseHelper dbHelper;
        private ActivityLogManager logManager;
        private TaskManager taskManager;
        private QuizManager quizManager;

        private string currentUsername = "";
        private bool isUsernameSet = false;
        private bool isDatabaseConnected = false;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize managers
            usernameManager = new UsernameManager();
            responseManager = new ResponseManager();
            greetingManager = new GreetingManager();

            // Initialize database
            dbHelper = new DatabaseHelper();

            // Check database connection
            CheckDatabaseConnection();

            // Play voice greeting
            greetingManager.PlayVoiceGreeting();
        }

        private void CheckDatabaseConnection()
        {
            try
            {
                isDatabaseConnected = dbHelper.TestConnection();

                if (isDatabaseConnected)
                {
                    AddChatMessage("📢 System", "✅ Database connected successfully!");
                }
                else
                {
                    AddChatMessage("📢 System", "⚠️ Database connection failed!");
                    AddChatMessage("📢 System", "💡 Make sure SQL Server is running and database exists.");
                }
            }
            catch (Exception ex)
            {
                AddChatMessage("📢 System", $"❌ Database error: {ex.Message}");
                isDatabaseConnected = false;
            }
        }

        private void SetUsername(string username)
        {
            currentUsername = username;
            isUsernameSet = true;

            // Initialize managers that need username
            logManager = new ActivityLogManager(username, dbHelper);
            taskManager = new TaskManager(username, logManager, dbHelper);
            quizManager = new QuizManager(logManager);

            // Update UI
            user_greeting.Text = $"Welcome, {username}!";
            AddChatMessage("🤖 CyberBot", $"Welcome, {username}! 🎉");

            if (isDatabaseConnected)
            {
                AddChatMessage("🤖 CyberBot", "💡 Try these commands:");
                AddChatMessage("🤖 CyberBot", "• 'add task [title]' - Add a new task");
                AddChatMessage("🤖 CyberBot", "• 'show tasks' - View your tasks");
                AddChatMessage("🤖 CyberBot", "• 'start quiz' - Take a cybersecurity quiz");
                AddChatMessage("🤖 CyberBot", "• 'show log' - View recent activity");
            }
            AddChatMessage("🤖 CyberBot", "• 'help' - Show all commands");
        }

        private void AddChatMessage(string sender, string message)
        {
            var chatItem = new
            {
                Sender = sender,
                Message = message,
                Background = sender.Contains("CyberBot") ?
                    new SolidColorBrush(Color.FromRgb(50, 50, 70)) :
                    sender.Contains("System") ?
                    new SolidColorBrush(Color.FromRgb(46, 125, 50)) :
                    new SolidColorBrush(Color.FromRgb(0, 115, 230)),
                Alignment = sender.Contains("CyberBot") ?
                    HorizontalAlignment.Left :
                    sender.Contains("System") ?
                    HorizontalAlignment.Center :
                    HorizontalAlignment.Right
            };

            chats.Items.Add(chatItem);
            if (chats.Items.Count > 0)
                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }

        // ============================================
        // NAVIGATION
        // ============================================

        private void welcome(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
            usernames_input.Focus();
        }

        private void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUsername();
                e.Handled = true;
            }
        }

        private void submit_name(object sender, RoutedEventArgs e)
        {
            ProcessUsername();
        }

        private void ProcessUsername()
        {
            string name = usernames_input.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                error_message.Visibility = Visibility.Visible;
                return;
            }

            error_message.Visibility = Visibility.Collapsed;

            username_grid.Visibility = Visibility.Hidden;
            chat_grid.Visibility = Visibility.Visible;

            SetUsername(name);
            question.Focus();
        }

        // ============================================
        // CHAT INPUT
        // ============================================

        private void Question_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessQuestion();
                e.Handled = true;
            }
        }

        private void send(object sender, RoutedEventArgs e)
        {
            ProcessQuestion();
        }

        private void ProcessQuestion()
        {
            string input = question.Text.Trim();
            if (string.IsNullOrWhiteSpace(input))
                return;

            AddChatMessage(currentUsername, input);
            question.Clear();

            if (!isUsernameSet)
            {
                SetUsername(input);
                return;
            }

            logManager?.AddLog("User Input", input);

            string lowerInput = input.ToLower();

            if (lowerInput == "help")
            {
                ShowHelp();
                return;
            }

            if (lowerInput.Contains("add task") || lowerInput.Contains("remind me"))
            {
                HandleAddTask(input);
                return;
            }

            if (lowerInput.Contains("show tasks") || lowerInput.Contains("view tasks"))
            {
                ShowTasks();
                return;
            }

            if (lowerInput.Contains("start quiz") || lowerInput.Contains("take quiz"))
            {
                StartQuiz();
                return;
            }

            if (lowerInput.Contains("show log") || lowerInput.Contains("show activity"))
            {
                ShowActivityLog();
                return;
            }

            string response = responseManager.GenerateResponse(input);
            AddChatMessage("🤖 CyberBot", response);
            logManager?.AddLog("Bot Response", response);
        }

        // ============================================
        // HELP
        // ============================================

        private void ShowHelp()
        {
            string help = @"📚 **Available Commands:**

💬 **Chat:**
• Ask about: passwords, phishing, scams, privacy, 2FA, malware, VPN

📋 **Tasks:**
• `add task [title]` - Add a new task
• `remind me [title]` - Add task with reminder
• `show tasks` - View your tasks

🎮 **Quiz:**
• `start quiz` - Take the cybersecurity quiz

📊 **Activity Log:**
• `show log` - Show recent activity

💡 **Examples:**
• ""add task Enable two-factor authentication""
• ""start quiz""";

            AddChatMessage("🤖 CyberBot", help);
            logManager?.AddLog("Help", "User viewed help");
        }

        // ============================================
        // TASK HANDLING
        // ============================================

        private void HandleAddTask(string input)
        {
            if (taskManager == null)
            {
                AddChatMessage("🤖 CyberBot", "Please set your username first.");
                return;
            }

            if (!isDatabaseConnected)
            {
                AddChatMessage("🤖 CyberBot", "⚠️ Database is not connected. Cannot add tasks.");
                return;
            }

            string title = taskManager.ExtractTaskTitle(input);

            if (string.IsNullOrWhiteSpace(title))
            {
                AddChatMessage("🤖 CyberBot", "Please specify what task you want to add. Example: 'add task Enable 2FA'");
                return;
            }

            bool success = taskManager.AddTask(title, "", null);
            if (success)
            {
                AddChatMessage("🤖 CyberBot", $"✅ Task added: '{title}'");
                logManager.AddLog("Task Added", $"Task: '{title}'");
                if (MainTabControl.SelectedIndex == 1)
                    LoadTasks();
            }
            else
            {
                AddChatMessage("🤖 CyberBot", "⚠️ Could not add task. Check database connection.");
            }
        }

        private void ShowTasks()
        {
            if (taskManager == null)
            {
                AddChatMessage("🤖 CyberBot", "Please set your username first.");
                return;
            }

            if (!isDatabaseConnected)
            {
                AddChatMessage("🤖 CyberBot", "⚠️ Database is not connected. Cannot show tasks.");
                return;
            }

            var tasks = taskManager.GetTasks();
            string taskList = taskManager.FormatTasksForDisplay(tasks);
            AddChatMessage("🤖 CyberBot", taskList);

            MainTabControl.SelectedIndex = 1;
            LoadTasks();
        }

        private void LoadTasks()
        {
            if (taskManager == null) return;

            try
            {
                var tasks = taskManager.GetTasks();
                TaskListView.ItemsSource = tasks.DefaultView;
                TaskStatus.Text = $"📋 {tasks.Rows.Count} tasks pending";
            }
            catch (Exception ex)
            {
                TaskStatus.Text = $"⚠️ Error: {ex.Message}";
            }
        }

        // ============================================
        // QUIZ HANDLING
        // ============================================

        private void StartQuiz()
        {
            if (quizManager == null)
            {
                AddChatMessage("🤖 CyberBot", "Please set your username first.");
                return;
            }

            quizManager.ResetQuiz();
            AddChatMessage("🎮 CyberBot", "Starting Cybersecurity Quiz! Test your knowledge.");

            MainTabControl.SelectedIndex = 2;
            ShowNextQuestion();
        }

        private void ShowNextQuestion()
        {
            var question = quizManager.GetCurrentQuestion();
            if (question == null)
            {
                QuizComplete();
                return;
            }

            QuizQuestionText.Text = question.Question;
            QuizScoreDisplay.Text = $"{quizManager.Score}/{quizManager.TotalQuestions}";
            QuizProgressDisplay.Text = $"Question {quizManager.CurrentQuestionNumber} of {quizManager.TotalQuestions}";
            QuizFeedbackBorder.Visibility = Visibility.Collapsed;
            QuizFeedback.Text = "";

            for (int i = 0; i < 4; i++)
            {
                Button optionButton = GetOptionButton(i);
                if (i < question.Options.Count)
                {
                    optionButton.Content = $"{char.ConvertFromUtf32(65 + i)}) {question.Options[i]}";
                    optionButton.Visibility = Visibility.Visible;
                    optionButton.Background = new SolidColorBrush(Color.FromRgb(45, 45, 68));
                    optionButton.IsEnabled = true;
                }
                else
                {
                    optionButton.Visibility = Visibility.Collapsed;
                }
            }

            StartQuizButton.Visibility = Visibility.Collapsed;
        }

        private Button GetOptionButton(int index)
        {
            switch (index)
            {
                case 0: return QuizOption1;
                case 1: return QuizOption2;
                case 2: return QuizOption3;
                case 3: return QuizOption4;
                default: return null;
            }
        }

        private void QuizOption_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int selectedIndex = int.Parse(button.Tag.ToString());

            bool isComplete = quizManager.SubmitAnswer(selectedIndex, out bool isCorrect, out string explanation);

            QuizFeedbackBorder.Visibility = Visibility.Visible;
            QuizFeedback.Text = isCorrect ? $"✅ Correct! {explanation}" : $"❌ Incorrect. {explanation}";
            QuizFeedback.Foreground = isCorrect ?
                new SolidColorBrush(Color.FromRgb(76, 175, 80)) :
                new SolidColorBrush(Color.FromRgb(255, 107, 107));

            for (int i = 0; i < 4; i++)
            {
                var opt = GetOptionButton(i);
                if (opt != null)
                    opt.IsEnabled = false;
            }

            QuizScoreDisplay.Text = $"{quizManager.Score}/{quizManager.TotalQuestions}";

            if (isComplete)
            {
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(2.5);
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    QuizComplete();
                };
                timer.Start();
            }
            else
            {
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(2);
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    ShowNextQuestion();
                };
                timer.Start();
            }
        }

        private void QuizComplete()
        {
            int percentage = (quizManager.Score * 100) / quizManager.TotalQuestions;
            string feedback = quizManager.GetFinalFeedback();

            QuizQuestionText.Text = $"🎉 Quiz Complete!\n\nScore: {quizManager.Score}/{quizManager.TotalQuestions} ({percentage}%)\n\n{feedback}";

            QuizOption1.Visibility = Visibility.Collapsed;
            QuizOption2.Visibility = Visibility.Collapsed;
            QuizOption3.Visibility = Visibility.Collapsed;
            QuizOption4.Visibility = Visibility.Collapsed;
            QuizFeedbackBorder.Visibility = Visibility.Collapsed;
            QuizFeedback.Text = "";

            StartQuizButton.Visibility = Visibility.Visible;
            StartQuizButton.Content = "🔄 Take Quiz Again";
            QuizProgressDisplay.Text = "Complete!";

            logManager?.AddLog("Quiz Complete", $"Score: {quizManager.Score}/{quizManager.TotalQuestions} ({percentage}%)");

            if (isDatabaseConnected)
                dbHelper.SaveQuizScore(currentUsername, quizManager.Score, quizManager.TotalQuestions);

            AddChatMessage("🎮 CyberBot", $"Quiz complete! You scored {quizManager.Score}/{quizManager.TotalQuestions} ({percentage}%)");
            AddChatMessage("🎮 CyberBot", feedback);
        }

        // ============================================
        // ACTIVITY LOG
        // ============================================

        private void ShowActivityLog()
        {
            if (logManager == null)
            {
                AddChatMessage("🤖 CyberBot", "Please set your username first.");
                return;
            }

            string logSummary = logManager.GetLogSummary(10);
            AddChatMessage("📊 CyberBot", $"📋 Activity Log:\n\n{logSummary}");

            MainTabControl.SelectedIndex = 3;
            LoadActivityLog();
        }

        private void LoadActivityLog()
        {
            try
            {
                if (!isDatabaseConnected)
                {
                    ActivityLogListView.ItemsSource = null;
                    return;
                }

                var logs = logManager.GetRecentLogs(20);
                ActivityLogListView.ItemsSource = logs.DefaultView;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Log error: {ex.Message}");
            }
        }

        // ============================================
        // TASK TAB EVENT HANDLERS
        // ============================================

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (taskManager == null)
            {
                TaskStatus.Text = "⚠️ Please set username first in the chat tab.";
                return;
            }

            if (!isDatabaseConnected)
            {
                TaskStatus.Text = "⚠️ Database not connected. Cannot add tasks.";
                return;
            }

            string title = TaskTitleInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(title) || title == "Enter task title...")
            {
                TaskStatus.Text = "⚠️ Please enter a task title.";
                return;
            }

            bool success = taskManager.AddTask(title, "", null);
            if (success)
            {
                TaskTitleInput.Text = "";
                LoadTasks();
                TaskStatus.Text = $"✅ Task added: '{title}'";
                logManager.AddLog("Task Added", $"Task: '{title}'");
            }
            else
            {
                TaskStatus.Text = "⚠️ Could not add task. Check database connection.";
            }
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int taskId = Convert.ToInt32(button.Tag);

            var task = (DataRowView)button.DataContext;
            string title = task["title"].ToString();

            bool success = taskManager.CompleteTask(taskId, title);
            if (success)
            {
                LoadTasks();
                TaskStatus.Text = $"✅ Task completed: '{title}'";
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int taskId = Convert.ToInt32(button.Tag);

            var task = (DataRowView)button.DataContext;
            string title = task["title"].ToString();

            if (MessageBox.Show($"Delete task '{title}'?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool success = taskManager.DeleteTask(taskId, title);
                if (success)
                {
                    LoadTasks();
                    TaskStatus.Text = $"🗑️ Task deleted: '{title}'";
                }
            }
        }

        private void TaskTitleInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TaskTitleInput.Text == "Enter task title...")
            {
                TaskTitleInput.Text = "";
                TaskTitleInput.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void TaskTitleInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskTitleInput.Text))
            {
                TaskTitleInput.Text = "Enter task title...";
                TaskTitleInput.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
            }
        }

        // ============================================
        // QUIZ TAB EVENT HANDLERS
        // ============================================

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isUsernameSet)
            {
                MessageBox.Show("Please set your username in the chat tab first.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            StartQuiz();
        }

        // ============================================
        // LOG TAB EVENT HANDLERS
        // ============================================

        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            LoadActivityLog();
        }

        // ============================================
        // TAB SELECTION
        // ============================================

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 1 && taskManager != null)
                LoadTasks();
            else if (MainTabControl.SelectedIndex == 3 && logManager != null)
                LoadActivityLog();
        }
    }
}
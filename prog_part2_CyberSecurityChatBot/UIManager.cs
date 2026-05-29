using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace prog_part2_CyberSecurityChatBot
{
    public class UIManager
    {
            // ============================================
            // UI REFERENCES (set from MainWindow)
            // ============================================
            private Grid homeGrid;
            private Grid usernameGrid;
            private Grid chatGrid;
            private TextBlock userGreetingText;
            private TextBlock errorMessageText;

            // Manager references
            private UsernameManager usernameManager;
            private ResponseManager responseManager;

            // ============================================
            // CONSTRUCTOR
            // ============================================
            public UIManager(UsernameManager userMgr, ResponseManager respMgr)
            {
                usernameManager = userMgr;
                responseManager = respMgr;
            }

            // Set UI references (called from MainWindow after InitializeComponent)
            public void SetUILinks(Grid home, Grid username, Grid chat, TextBlock greeting, TextBlock error)
            {
                homeGrid = home;
                usernameGrid = username;
                chatGrid = chat;
                userGreetingText = greeting;
                errorMessageText = error;
            }

            // ============================================
            // NAVIGATION METHODS (was Class 2)
            // ============================================
            public void ShowHomePage()
            {
                ShowPage(homeGrid, usernameGrid, chatGrid);
            }

            public void ShowUsernamePage()
            {
                ShowPage(usernameGrid, homeGrid, chatGrid);
            }

            public void ShowChatPage()
            {
                ShowPage(chatGrid, homeGrid, usernameGrid);
            }

            private void ShowPage(Grid pageToShow, params Grid[] pagesToHide)
            {
                if (pageToShow != null)
                    pageToShow.Visibility = Visibility.Visible;

                foreach (var page in pagesToHide)
                {
                    if (page != null)
                        page.Visibility = Visibility.Hidden;
                }
            }

            public bool IsOnChatPage()
            {
                return chatGrid != null && chatGrid.Visibility == Visibility.Visible;
            }

            // ============================================
            // UI METHODS (chat bubbles, scrolling, etc.)
            // ============================================
            public void AddUserMessage(string username, string message, ListView chatDisplay)
            {
                var chatItem = new
                {
                    Sender = $"👤 {username}",
                    Message = message,
                    Background = new SolidColorBrush(Color.FromRgb(0, 115, 230)),
                    Alignment = HorizontalAlignment.Right
                };

                chatDisplay.Items.Add(chatItem);
                ScrollToBottom(chatDisplay);
            }

            public void AddBotMessage(string message, ListView chatDisplay)
            {
                var chatItem = new
                {
                    Sender = "🤖 CyberBot",
                    Message = message,
                    Background = new SolidColorBrush(Color.FromRgb(50, 50, 70)),
                    Alignment = HorizontalAlignment.Left
                };

                chatDisplay.Items.Add(chatItem);
                ScrollToBottom(chatDisplay);
            }

            public void AddSystemMessage(string message, ListView chatDisplay)
            {
                var chatItem = new
                {
                    Sender = "📢 System",
                    Message = message,
                    Background = new SolidColorBrush(Color.FromRgb(46, 125, 50)),
                    Alignment = HorizontalAlignment.Center
                };

                chatDisplay.Items.Add(chatItem);
                ScrollToBottom(chatDisplay);
            }

            private void ScrollToBottom(ListView chatDisplay)
            {
                if (chatDisplay.Items.Count > 0)
                    chatDisplay.ScrollIntoView(chatDisplay.Items[chatDisplay.Items.Count - 1]);
            }

            public void UpdateUserGreeting(string username)
            {
                if (userGreetingText != null)
                    userGreetingText.Text = $"Welcome, {username}!";
            }

            public void ClearInput(TextBox inputBox)
            {
                if (inputBox != null)
                    inputBox.Clear();
            }

            public void FocusInput(TextBox inputBox)
            {
                if (inputBox != null)
                    inputBox.Focus();
            }

            public void ShowUsernameError(bool show)
            {
                if (errorMessageText != null)
                    errorMessageText.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            }

            // ============================================
            // CHAT PROCESSING METHODS (was Class 5)
            // ============================================
            public void ProcessUserQuestion(string questionText, ListView chatDisplay, TextBox inputBox)
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(questionText))
                {
                    AddBotMessage("Please enter a question. I'm here to help! 😊", chatDisplay);
                    ClearInput(inputBox);
                    FocusInput(inputBox);
                    return;
                }

                // Display user message
                AddUserMessage(usernameManager.CurrentUsername, questionText, chatDisplay);
                ClearInput(inputBox);

                // Check for goodbye
                if (responseManager.IsGoodbye(questionText))
                {
                    AddBotMessage(responseManager.GenerateResponse(questionText), chatDisplay);
                    return;
                }

                // Check for interests
                if (responseManager.ContainsInterest(questionText))
                {
                    usernameManager.SaveUserInterest(questionText);
                    AddBotMessage("Great! I'll remember your interests! 📝", chatDisplay);
                }

                // Generate and display response
                string response = responseManager.GenerateResponse(questionText);
                AddBotMessage(response, chatDisplay);

                // Increment counter and check for interest reminder
                usernameManager.IncrementMessageCounter();
                if (usernameManager.ShouldShowInterestReminder())
                {
                    string interests = usernameManager.GetSavedInterests();
                    if (!string.IsNullOrEmpty(interests))
                    {
                        AddBotMessage($"💡 Just a reminder, you're interested in {interests}. Want to learn more?", chatDisplay);
                    }
                }

                FocusInput(inputBox);
            }

            public void SendWelcomeMessages(ListView chatDisplay)
            {
                AddBotMessage("I'm here to help you stay safe online. What would you like to learn about?", chatDisplay);
                AddBotMessage(responseManager.GetHelpMessage(), chatDisplay);
            }

            public void ProcessUsernameSubmission(string enteredName, ListView chatDisplay, TextBox inputBox)
            {
                string welcomeMessage;
                bool isNewUser;

                if (usernameManager.ProcessUsername(enteredName, out welcomeMessage, out isNewUser))
                {
                    ShowUsernameError(false);
                    UpdateUserGreeting(usernameManager.CurrentUsername);

                    AddBotMessage(welcomeMessage, chatDisplay);
                    SendWelcomeMessages(chatDisplay);

                    ShowChatPage();
                    FocusInput(inputBox);
                }
                else
                {
                    ShowUsernameError(true);
                }
            }
        }
    }



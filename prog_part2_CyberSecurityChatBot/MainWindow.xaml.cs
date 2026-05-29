using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prog_part2_CyberSecurityChatBot
{//start of namespace

    public partial class MainWindow : Window
    {//start of class

     
       
            // All managers
            private GreetingManager greetingManager;
            private NavigationManager navigationManager;
            private UsernameManager usernameManager;
            private ResponseManager responseManager;
            private UIManager uiManager;
            private ChatManager chatManager;

            public MainWindow()
            {
                InitializeComponent();

                // Initialize all managers
                InitializeManagers();

                // Play voice greeting
                greetingManager.PlayVoiceGreeting();
            }

            private void InitializeManagers()
            {
                // Create UI Manager first (handles visual updates)
                uiManager = new UIManager();

                // Create other managers
                greetingManager = new GreetingManager();
                navigationManager = new NavigationManager(home_grid, username_grid, chat_grid);
                usernameManager = new UsernameManager();
                responseManager = new ResponseManager();
                chatManager = new ChatManager(usernameManager, responseManager, uiManager);
            }

            // Start button click - navigates to username page
            private void welcome(object sender, RoutedEventArgs e)
            {
                navigationManager.ShowUsernamePage();
                usernames_input.Focus();
            }

            // Enter key in username textbox
            private void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SubmitUsername();
                    e.Handled = true;
                }
            }

            // Submit username button click
            private void submit_name(object sender, RoutedEventArgs e)
            {
                SubmitUsername();
            }

            private void SubmitUsername()
            {
                string enteredName = usernames_input.Text.Trim();
                string welcomeMessage;
                bool isNewUser;

                if (usernameManager.ProcessUsername(enteredName, out welcomeMessage, out isNewUser))
                {
                    uiManager.ShowUsernameError(error_message, false);
                    uiManager.UpdateUserGreeting(user_greeting, usernameManager.CurrentUsername);

                    uiManager.AddBotMessage(welcomeMessage, chats);
                    chatManager.SendWelcomeMessages(chats);

                    navigationManager.ShowChatPage();
                    uiManager.FocusInput(question);
                }
                else
                {
                    uiManager.ShowUsernameError(error_message, true);
                }
            }

            // Enter key in question textbox
            private void Question_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SendQuestion();
                    e.Handled = true;
                }
            }

            // Send button click
            private void send(object sender, RoutedEventArgs e)
            {
                SendQuestion();
            }

            private void SendQuestion()
            {
                chatManager.ProcessUserQuestion(question.Text, chats, question);
            }
        }
    }
    

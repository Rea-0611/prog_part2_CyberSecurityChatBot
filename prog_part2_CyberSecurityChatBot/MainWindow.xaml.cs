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
        // Move these to class level (outside constructor)
        private GreetingManager greetingManager;
        private UsernameManager usernameManager;
        private ResponseManager responseManager;
        private UIManager uiManager;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize managers (order matters!)
            usernameManager = new UsernameManager();
            responseManager = new ResponseManager();
            uiManager = new UIManager(usernameManager, responseManager);
            greetingManager = new GreetingManager();

            // Pass UI references to UIManager
            uiManager.SetUILinks(home_grid, username_grid, chat_grid, user_greeting, error_message);

            // Play voice greeting on startup
            greetingManager.PlayVoiceGreeting();
        }

        // Start button click - navigates to username page
        private void welcome(object sender, RoutedEventArgs e)
        {
            uiManager.ShowUsernamePage();
            usernames_input.Focus();
        }

        // Enter key in username textbox
        private void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uiManager.ProcessUsernameSubmission(usernames_input.Text, chats, usernames_input);
                e.Handled = true;
            }
        }

        // Submit username button click
        private void submit_name(object sender, RoutedEventArgs e)
        {
            uiManager.ProcessUsernameSubmission(usernames_input.Text, chats, usernames_input);
        }

        // Enter key in question textbox
        private void Question_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uiManager.ProcessUserQuestion(question.Text, chats, question);
                e.Handled = true;
            }
        }

        // Send button click
        private void send(object sender, RoutedEventArgs e)
        {
            uiManager.ProcessUserQuestion(question.Text, chats, question);
        }
    }//end of class
}//end of namespace
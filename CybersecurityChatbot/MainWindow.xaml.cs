using System;
using System.Windows;
using System.Windows.Input;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        private ChatBot? _chatBot;
        private bool _isAsciiArtVisible = true;

        public MainWindow()
        {
            InitializeComponent();
            InitializeChatbot();
        }

        private void InitializeChatbot()
        {
            _chatBot = new ChatBot();

            // Play voice greeting
            _chatBot.PlayVoiceGreeting();

            // Display ASCII art
            LoadAsciiArt();

            // Get initial greeting from bot
            string greeting = _chatBot.GetGreeting();
            AppendBotMessage(greeting);
        }

        private void LoadAsciiArt()
        {
            string asciiArt = @"
    ╔══════════════════════════════════════════════════════════════════╗
    ║                                                                  ║
    ║      ██████╗██╗   ██╗██████╗ ███████╗██████╗                     ║
    ║     ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗                    ║
    ║     ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝                    ║
    ║     ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗                    ║
    ║     ╚██████╗   ██║   ██████╔╝███████╗██║  ██║                    ║
    ║      ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝                    ║
    ║                                                                  ║
    ║                                                                  ║
    ║                                                                  ║
    ║              ██████╗  ██████╗ ████████╗                          ║
    ║              ██╔══██╗██╔═══██╗╚══██╔══╝                          ║
    ║              ██████╔╝██║   ██║   ██║                             ║
    ║              ██╔══██╗██║   ██║   ██║                             ║
    ║              ██████╔╝╚██████╔╝   ██║                             ║
    ║              ╚═════╝  ╚═════╝    ╚═╝                             ║
    ║                                                                  ║
    ║           CYBERSECURITY AWARENESS ASSISTANT                      ║
    ║                                                                  ║
    ╚══════════════════════════════════════════════════════════════════╝
            ";

            AsciiArtBlock.Text = asciiArt;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            if (_chatBot == null) return;

            string userMessage = UserInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return;
            }

            // Display user message
            AppendUserMessage(userMessage);

            // Clear input box
            UserInput.Clear();

            // Process input through chatbot
            string botResponse = _chatBot.ProcessInput(userMessage);

            // Display bot response
            AppendBotMessage(botResponse);

            // Scroll to bottom
            ScrollToBottom();
        }

        private void AppendUserMessage(string message)
        {
            ChatDisplay.Text += $"\n\n👤 You: {message}\n";
            ChatDisplay.Text += new string('─', 60) + "\n";
        }

        private void AppendBotMessage(string message)
        {
            ChatDisplay.Text += $"\n🤖 Bot: {message}\n";
            ChatDisplay.Text += new string('─', 60) + "\n";
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToBottom();
        }
    }
}
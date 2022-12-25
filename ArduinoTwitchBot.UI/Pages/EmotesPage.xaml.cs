using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArduinoTwitchBot.UI.Pages
{
	using System.Windows.Media;
	using Code;
	using MaterialDesignThemes.Wpf;

	public partial class EmotesPage : Page
	{
		public static List<ChatMessageEntry> ChatMessages;
		
		public EmotesPage()
		{
			InitializeComponent();

			ChatMessages = UserSettings.ChatMessageEntries;
		}

		public void SaveEmotes()
		{
			ChatMessages.Clear();
			
			var subpanels = ChatMessageEntriesPanel.Children.OfType<StackPanel>();

			foreach (var panel in subpanels)
			{
				var id = panel.Name[5..];
				
				var chatMessage = panel.Children.OfType<TextBox>().First().Text;
				var signal = panel.Children.OfType<TextBox>().Last().Text;
				
				var signalType = (SignalType)Enum.Parse(typeof(SignalType), panel.Children.OfType<ComboBox>().First().Text);
				
				ChatMessages.Add(new ChatMessageEntry(id, chatMessage, signal, signalType));
			}
		}

		private void MainPageButton_Click(object sender, RoutedEventArgs e)
		{
			SaveEmotes();

			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new MainPage());
		}

		private void AddNewChatMessageAlertButton_OnClick(object sender, RoutedEventArgs e)
		{
			var panel = GenerateChatMessagePanel();
			ChatMessageEntriesPanel.Children.Add(panel);
		}

		private StackPanel GenerateChatMessagePanel()
		{
			var guid = Guid.NewGuid().ToString("N");

			var panel = new StackPanel()
			{
				Orientation = Orientation.Horizontal,
				HorizontalAlignment = HorizontalAlignment.Center,
				Margin = new Thickness(0,20,0,0),
				Name = "Panel" + guid
			};

			var chatMessageBox = new TextBox()
			{
				Width = 160,
				Margin = new Thickness(20, 0, 0, 0),
				ToolTip = "Enter a value that will trigger sending the specific signal",
				FontSize = 18,
				Name = "ChatMessageBox" + guid
			};
			HintAssist.SetHint(chatMessageBox, "Message");

			var chatMessageAlertSignalBox = new TextBox()
			{
				Width = 80,
				Margin = new Thickness(20, 0, 0, 0),
				ToolTip = "Enter a value that will be sent through the Serial Port",
				FontSize = 18,
				Name = "ChatMessageAlertSignalBox" + guid
			};
			HintAssist.SetHint(chatMessageAlertSignalBox, "Value");

			var chatMessageSignalTypeBox = new ComboBox()
			{
				Width = 80,
				Margin = new Thickness(10, 0, 0, 0),
				ToolTip = "Should the given value be interpreted as an int, or a string?",
				FontSize = 18,
				Name = "ChatMessageSignalTypeBox" + guid
			};
			
			var values = Enum.GetValues(typeof(SignalType)).Cast<SignalType>().ToList();
			chatMessageSignalTypeBox.ItemsSource = values;
			chatMessageSignalTypeBox.SelectedIndex = 0;

			var chatMessageDeleteEntryButton = new Button()
			{
				Width = 24,
				Height = 24,
				Margin = new Thickness(20, 0, 0, 0),
				Name = "ChatMessageDeleteEntryButton" + guid
			};
			chatMessageDeleteEntryButton.Click += ChatMessageDeleteEntryButton_OnClick;

			var icon = new PackIcon()
			{
				Kind = PackIconKind.Delete,
				Width = 20,
				Height = 20,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Foreground = Brushes.White
			};
			
			chatMessageDeleteEntryButton.Content = icon;

			panel.Children.Add(chatMessageBox);
			panel.Children.Add(chatMessageAlertSignalBox);
			panel.Children.Add(chatMessageSignalTypeBox);
			panel.Children.Add(chatMessageDeleteEntryButton);

			return panel;
		}

		private void ChatMessageDeleteEntryButton_OnClick(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var panel = button?.Parent as StackPanel;
			ChatMessageEntriesPanel.Children.Remove(panel);
		}
	}
}

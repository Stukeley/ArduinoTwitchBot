using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArduinoTwitchBot.UI.Pages
{
	public partial class EmotesPage : Page
	{
		public EmotesPage()
		{
			InitializeComponent();

			// Works even if EmotesList is null.
			EmotesTextBox.Text = string.Join(", ", UserSettings.EmotesList?.ToArray());
			EmotesTextBox.Focus();
		}

		public void SaveEmotes()
		{
			// Get Text from the RichTextBox.
			string text = EmotesTextBox.Text;
			// Save emotes to UserSettings.
			List<string> emotesList;

			// Valid separators: comma, semicolon, space.
			char[] separators = new[] { ',', ';', ' ' };
			emotesList = text.Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

			UserSettings.EmotesList = emotesList;
		}

		private void MainPageButton_Click(object sender, RoutedEventArgs e)
		{
			SaveEmotes();

			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new MainPage());
		}
	}
}

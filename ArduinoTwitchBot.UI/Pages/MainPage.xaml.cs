using ArduinoTwitchBot.Code;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArduinoTwitchBot.UI.Pages
{
	public partial class MainPage : Page
	{
		public MainPage()
		{
			InitializeComponent();

			RefreshSignalTypes();
		}

		private void RefreshSignalTypes()
		{
			var values = Enum.GetValues(typeof(SignalType)).Cast<SignalType>().ToList();

			FollowSignalTypeBox.ItemsSource = values;
			FollowSignalTypeBox.SelectedIndex = 0;

			SubSignalTypeBox.ItemsSource = values;
			SubSignalTypeBox.SelectedIndex = 0;

			BitsSignalTypeBox.ItemsSource = values;
			BitsSignalTypeBox.SelectedIndex = 0;

			HostSignalTypeBox.ItemsSource = values;
			HostSignalTypeBox.SelectedIndex = 0;

			RaidSignalTypeBox.ItemsSource = values;
			RaidSignalTypeBox.SelectedIndex = 0;

			EmoteSignalTypeBox.ItemsSource = values;
			EmoteSignalTypeBox.SelectedIndex = 0;
		}

		private bool ValidateText(string text)
		{
			// Check if the string is empty/whitespace.
			if (text == "" || string.IsNullOrWhiteSpace(text))
			{
				MessageBox.Show("One or more of the selected alert values were empty!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}

			return true;
		}

		private void SettingsPageButton_Click(object sender, RoutedEventArgs e)
		{
			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new SettingsPage());
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			// Create an array to keep track of Alerts easier.
			var alerts = new Alert[6];

			// Validate data.
			if (FollowAlertCheckbox.IsChecked.Value && ValidateText(FollowAlertSignalBox.Text))
			{
				alerts[0] = new Alert(true, FollowAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), FollowSignalTypeBox.Text));
			}
			else
			{
				alerts[0] = new Alert(false);
			}

			if (SubAlertCheckbox.IsChecked.Value && ValidateText(SubAlertSignalBox.Text))
			{
				alerts[1] = new Alert(true, SubAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), SubSignalTypeBox.Text));
			}
			else
			{
				alerts[1] = new Alert(false);
			}

			if (BitsAlertCheckbox.IsChecked.Value && ValidateText(BitsAlertSignalBox.Text))
			{
				alerts[2] = new Alert(true, BitsAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), BitsSignalTypeBox.Text));
			}
			else
			{
				alerts[2] = new Alert(false);
			}

			if (RaidAlertCheckbox.IsChecked.Value && ValidateText(RaidAlertSignalBox.Text))
			{
				alerts[3] = new Alert(true, RaidAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), RaidSignalTypeBox.Text));
			}
			else
			{
				alerts[3] = new Alert(false);
			}

			if (HostAlertCheckbox.IsChecked.Value && ValidateText(HostAlertSignalBox.Text))
			{
				alerts[4] = new Alert(true, HostAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), HostSignalTypeBox.Text));
			}
			else
			{
				alerts[4] = new Alert(false);
			}

			if (EmoteAlertCheckbox.IsChecked.Value && ValidateText(EmoteAlertSignalBox.Text))
			{
				alerts[5] = new Alert(true, EmoteAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), EmoteSignalTypeBox.Text));
			}
			else
			{
				alerts[5] = new Alert(false);
			}

			// Save user settings.
			UserSettings.Alerts = alerts;

			// Connect the bot.
			TwitchBot.Instance.Connect("", "", alerts[0], alerts[1], alerts[2], alerts[3], alerts[4], alerts[5]);
		}

		private void SetEmoteListButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new EmotesPage());
		}
	}
}

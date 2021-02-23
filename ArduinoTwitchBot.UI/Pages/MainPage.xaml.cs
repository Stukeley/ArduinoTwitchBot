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

			var alerts = UserSettings.Alerts;

			// Load data from UserSettings.
			if (alerts.Length > 0)
			{
				FollowAlertCheckbox.IsChecked = alerts[0].IsActive;
				FollowAlertSignalBox.Text = alerts[0].Signal;
				FollowSignalTypeBox.SelectedItem = alerts[0].SignalType;

				SubAlertCheckbox.IsChecked = alerts[1].IsActive;
				SubAlertSignalBox.Text = alerts[1].Signal;
				SubSignalTypeBox.SelectedItem = alerts[1].SignalType;

				BitsAlertCheckbox.IsChecked = alerts[2].IsActive;
				BitsAlertSignalBox.Text = alerts[2].Signal;
				BitsSignalTypeBox.SelectedItem = alerts[2].SignalType;

				RaidAlertCheckbox.IsChecked = alerts[3].IsActive;
				RaidAlertSignalBox.Text = alerts[3].Signal;
				RaidSignalTypeBox.SelectedItem = alerts[3].SignalType;

				HostAlertCheckbox.IsChecked = alerts[4].IsActive;
				HostAlertSignalBox.Text = alerts[4].Signal;
				HostSignalTypeBox.SelectedItem = alerts[4].SignalType;

				EmoteAlertCheckbox.IsChecked = alerts[5].IsActive;
				EmoteAlertSignalBox.Text = alerts[5].Signal;
				EmoteSignalTypeBox.SelectedItem = alerts[5].SignalType;
			}
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

		public void SaveAlerts()
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
		}

		private void SettingsPageButton_Click(object sender, RoutedEventArgs e)
		{
			SaveAlerts();

			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new SettingsPage());
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			// Save alerts to UserSettings.
			SaveAlerts();

			var alerts = UserSettings.Alerts;

			// Connect the bot.
			TwitchBot.Instance.Connect(UserSettings.ClientId, UserSettings.AccessToken, UserSettings.ChannelName, UserSettings.PortName, alerts);
		}

		private void SetEmoteListButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			SaveAlerts();

			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new EmotesPage());
		}
	}
}

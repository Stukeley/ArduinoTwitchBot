using ArduinoTwitchBot.Code;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArduinoTwitchBot.UI.Pages
{
	public partial class MainPage : Page
	{
		private string _botStatus;

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

				EmoteAlertCheckbox.IsChecked = alerts[5].IsActive;
			}

			//Re - set application theme.
			(Application.Current.MainWindow as MainWindow).ChangeApplicationTheme(UserSettings.IsDarkTheme);
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

			RaidSignalTypeBox.ItemsSource = values;
			RaidSignalTypeBox.SelectedIndex = 0;
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

			_botStatus = "Listening to: ";

			// Validate data.
			if (FollowAlertCheckbox.IsChecked.Value && ValidateText(FollowAlertSignalBox.Text))
			{
				alerts[0] = new Alert(true, FollowAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), FollowSignalTypeBox.Text));
				_botStatus += "Follows ";
			}
			else
			{
				alerts[0] = new Alert(false);
			}

			if (SubAlertCheckbox.IsChecked.Value && ValidateText(SubAlertSignalBox.Text))
			{
				alerts[1] = new Alert(true, SubAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), SubSignalTypeBox.Text));
				_botStatus += "Subs ";
			}
			else
			{
				alerts[1] = new Alert(false);
			}

			if (BitsAlertCheckbox.IsChecked.Value && ValidateText(BitsAlertSignalBox.Text))
			{
				alerts[2] = new Alert(true, BitsAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), BitsSignalTypeBox.Text));
				_botStatus += "Bits ";
			}
			else
			{
				alerts[2] = new Alert(false);
			}

			if (RaidAlertCheckbox.IsChecked.Value && ValidateText(RaidAlertSignalBox.Text))
			{
				alerts[3] = new Alert(true, RaidAlertSignalBox.Text, (SignalType)Enum.Parse(typeof(SignalType), RaidSignalTypeBox.Text));
				_botStatus += "Raids ";
			}
			else
			{
				alerts[3] = new Alert(false);
			}
			
			// For compatibility issues (for now), create an inactive Host alert.
			alerts[4] = new Alert(false);
			
			if (EmoteAlertCheckbox.IsChecked.Value)
			{
				alerts[5] = new Alert(true);
				_botStatus += "ChatMessages ";
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

			// Connect the PubSub client if any (or all) of the 1-3 alerts have been selected.
			// Connect the Chat client if any (or all) of the 4-6 alerts have been selected.
			try
			{
				// Initialize the bot.
				TwitchBot.Instance.InitializeBot(UserSettings.ClientId, UserSettings.AccessToken, UserSettings.ChannelName, UserSettings.PortName, alerts);

				if (alerts.Take(3).Any(x => x.IsActive))
				{
					TwitchBot.Instance.ConnectPubSubClient();
				}

				if (alerts.Skip(3).Any(x => x.IsActive))
				{
					TwitchBot.Instance.ConnectChatClient(UserSettings.ChatMessageEntries);
				}

				IsBotRunningBlock.Text = _botStatus;

				DisconnectButton.Visibility = Visibility.Visible;
				ConnectButton.Visibility = Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SetEmoteListButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			SaveAlerts();

			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new EmotesPage());
		}

		private void HistoryButton_Click(object sender, RoutedEventArgs e)
		{
			SaveAlerts();

			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new HistoryPage());
		}

		private void DisconnectButton_Click(object sender, RoutedEventArgs e)
		{
			TwitchBot.Instance.DisconnectChatClient();
			TwitchBot.Instance.DisconnectPubSubClient();

			DisconnectButton.Visibility = Visibility.Collapsed;
			ConnectButton.Visibility = Visibility.Visible;
			IsBotRunningBlock.Text = "";
		}
	}
}

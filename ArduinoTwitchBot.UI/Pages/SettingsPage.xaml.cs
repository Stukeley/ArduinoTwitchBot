using ArduinoTwitchBot.Code;
using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArduinoTwitchBot.UI.Pages
{
	public partial class SettingsPage : Page
	{
		public SettingsPage()
		{
			InitializeComponent();

			RefreshPortNames();
			RefreshSignalTypes();

			// Load data from UserSettings.
			ApiKeyBox.Password = UserSettings.ApiKey;
			ChannelNameBox.Text = UserSettings.ChannelName;
			PortSelectionBox.SelectedItem = UserSettings.PortName;
		}

		private void RefreshSignalTypes()
		{
			var values = Enum.GetValues(typeof(SignalType)).Cast<SignalType>().ToList();

			TestSignalTypeBox.ItemsSource = values;
			TestSignalTypeBox.SelectedIndex = 0;
		}

		private void RefreshPortNames()
		{
			PortSelectionBox.ItemsSource = SerialPort.GetPortNames();
			PortSelectionBox.SelectedIndex = 0;
		}

		private void ConnectionTestButton_Click(object sender, RoutedEventArgs e)
		{
			var text = ConnectionTestBox.Text.Trim();

			if (text == "" || string.IsNullOrWhiteSpace(text))
			{
				MessageBox.Show("The entered value must be a non-empty string.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				SerialPortHelper.SendMessage(PortSelectionBox.SelectedValue.ToString(), text, (SignalType)Enum.Parse(typeof(SignalType), TestSignalTypeBox.SelectedValue.ToString()));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ShowHideApiKey_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			if (ShowHideApiKey.Kind == PackIconKind.Eye)
			{
				ShowHideApiKey.Kind = PackIconKind.EyeOff;

				var enteredApiKey = ApiKeyBox.Password;

				VisibleApiKeyBox.Visibility = Visibility.Visible;
				VisibleApiKeyBox.Text = enteredApiKey;
				ApiKeyBox.Visibility = Visibility.Collapsed;
			}
			else
			{
				ShowHideApiKey.Kind = PackIconKind.Eye;

				var enteredPassword = VisibleApiKeyBox.Text;

				ApiKeyBox.Visibility = Visibility.Visible;
				ApiKeyBox.Password = enteredPassword;
				VisibleApiKeyBox.Visibility = Visibility.Collapsed;
			}
		}

		private void RefreshPortNamesButton_Click(object sender, RoutedEventArgs e)
		{
			RefreshPortNames();
		}

		private void ApiKeyHelpButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var processInfo = new ProcessStartInfo()
			{
				FileName = "https://dev.twitch.tv/docs/authentication#registration",
				UseShellExecute = true
			};

			Process.Start(processInfo);
		}

		private void MainPageButton_Click(object sender, RoutedEventArgs e)
		{
			// Save user settings.
			UserSettings.ApiKey = ShowHideApiKey.Kind == PackIconKind.Eye ? ApiKeyBox.Password : VisibleApiKeyBox.Text;
			UserSettings.ChannelName = ChannelNameBox.Text;
			UserSettings.PortName = PortSelectionBox.SelectedValue.ToString();

			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new MainPage());
		}
	}
}

using ArduinoTwitchBot.Code;
using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ArduinoTwitchBot.UI.Pages
{
	public partial class SettingsPage : Page
	{
		private bool _isDarkTheme;
		public bool IsDarkTheme
		{
			get
			{
				return _isDarkTheme;
			}
			set
			{
				// Change theme.
				_isDarkTheme = value;

				(Application.Current.MainWindow as MainWindow).ChangeApplicationTheme(value);

				// Set icon colors manually.
				var iconForeground = value ? new SolidColorBrush(Colors.LightSlateGray) : new SolidColorBrush(Colors.DarkSlateGray);
				ShowHideClientId.Foreground = iconForeground;
				ShowHideAccessToken.Foreground = iconForeground;
			}
		}

		public SettingsPage()
		{
			InitializeComponent();
			this.DataContext = this;

			RefreshPortNames();
			RefreshSignalTypes();

			// Load data from UserSettings.
			ClientIdBox.Password = UserSettings.ClientId;
			AccessTokenBox.Password = UserSettings.AccessToken;
			ChannelNameBox.Text = UserSettings.ChannelName;
			PortSelectionBox.SelectedItem = UserSettings.PortName;

			// Change IsDark to the right theme.
			IsDarkTheme = UserSettings.IsDarkTheme;
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

		public void SaveSettings()
		{
			// Save user settings.
			UserSettings.ClientId = ShowHideClientId.Kind == PackIconKind.Eye ? ClientIdBox.Password.Trim() : VisibleClientIdBox.Text.Trim();
			UserSettings.AccessToken = ShowHideAccessToken.Kind == PackIconKind.Eye ? AccessTokenBox.Password.Trim() : VisibleAccessTokenBox.Text.Trim();
			UserSettings.ChannelName = ChannelNameBox.Text.Trim();
			UserSettings.PortName = PortSelectionBox.SelectedValue.ToString();
			UserSettings.IsDarkTheme = IsDarkTheme;
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

		private void RefreshPortNamesButton_Click(object sender, RoutedEventArgs e)
		{
			RefreshPortNames();
		}

		private void MainPageButton_Click(object sender, RoutedEventArgs e)
		{
			SaveSettings();

			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new MainPage());
		}

		private void ShowHideClientId_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			if (ShowHideClientId.Kind == PackIconKind.Eye)
			{
				ShowHideClientId.Kind = PackIconKind.EyeOff;

				var enteredApiKey = ClientIdBox.Password;

				VisibleClientIdBox.Visibility = Visibility.Visible;
				VisibleClientIdBox.Text = enteredApiKey;
				ClientIdBox.Visibility = Visibility.Collapsed;
			}
			else
			{
				ShowHideClientId.Kind = PackIconKind.Eye;

				var enteredPassword = VisibleClientIdBox.Text;

				ClientIdBox.Visibility = Visibility.Visible;
				ClientIdBox.Password = enteredPassword;
				VisibleClientIdBox.Visibility = Visibility.Collapsed;
			}
		}

		private void ClientIdHelpButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var processInfo = new ProcessStartInfo()
			{
				FileName = "https://github.com/Stukeley/ArduinoTwitchBot#setup",
				UseShellExecute = true
			};

			Process.Start(processInfo);
		}

		private void ShowHideAccessToken_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			if (ShowHideAccessToken.Kind == PackIconKind.Eye)
			{
				ShowHideAccessToken.Kind = PackIconKind.EyeOff;

				var enteredApiKey = AccessTokenBox.Password;

				VisibleAccessTokenBox.Visibility = Visibility.Visible;
				VisibleAccessTokenBox.Text = enteredApiKey;
				AccessTokenBox.Visibility = Visibility.Collapsed;
			}
			else
			{
				ShowHideAccessToken.Kind = PackIconKind.Eye;

				var enteredPassword = VisibleAccessTokenBox.Text;

				AccessTokenBox.Visibility = Visibility.Visible;
				AccessTokenBox.Password = enteredPassword;
				VisibleAccessTokenBox.Visibility = Visibility.Collapsed;
			}
		}
	}
}

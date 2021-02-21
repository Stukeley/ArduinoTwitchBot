using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows;

namespace ArduinoTwitchBot.UI
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			PortSelectionBox.ItemsSource = SerialPort.GetPortNames();
			PortSelectionBox.SelectedIndex = 0;
		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("https://dev.twitch.tv/docs/authentication#registration");
		}

		private void ConnectionTestButton_Click(object sender, RoutedEventArgs e)
		{
			var text = ConnectionTestBox.Text;

			if (text == "" || string.IsNullOrWhiteSpace(text))
			{
				MessageBox.Show("The entered value must be a non-empty string.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			var port = new SerialPort(PortSelectionBox.SelectedValue.ToString())
			{
				BaudRate = 9600,
				ReadTimeout = 2000,
				WriteTimeout = 2000
			};

			try
			{
				port.Write(text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ShowHideApiKey_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (ShowHideApiKey.Kind == MaterialDesignThemes.Wpf.PackIconKind.Eye)
			{
				ShowHideApiKey.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;

				var enteredApiKey = ApiKeyBox.Password;

				VisibleApiKeyBox.Visibility = Visibility.Visible;
				VisibleApiKeyBox.Text = enteredApiKey;
				ApiKeyBox.Visibility = Visibility.Collapsed;
			}
			else
			{
				ShowHideApiKey.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;

				var enteredPassword = VisibleApiKeyBox.Text;

				ApiKeyBox.Visibility = Visibility.Visible;
				ApiKeyBox.Password = enteredPassword;
				VisibleApiKeyBox.Visibility = Visibility.Collapsed;
			}
		}
	}
}

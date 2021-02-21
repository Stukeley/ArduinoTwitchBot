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
		}

		private void SettingsPageButton_Click(object sender, RoutedEventArgs e)
		{
			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new SettingsPage());
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			// Connect the bot.
		}

		private void SetEmoteListButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{

		}
	}
}

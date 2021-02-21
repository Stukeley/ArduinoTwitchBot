using System.ComponentModel;
using System.Windows;

namespace ArduinoTwitchBot.UI
{
	// TODO:
	// 1. Different button style for test
	// 3. Different font
	// 6. zamknięcie aplikacji minimalizuje do system tray
	// 8. weryfikacja czy są na pewno podane wartości w zaznaczonych opcjach
	// 9. host, raid, emote alerts
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			// Save user settings before quitting.
			UserSettings.SaveUserSettings();

			base.OnClosing(e);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// Load UserSettings.
			UserSettings.LoadUserSettings();
		}
	}
}

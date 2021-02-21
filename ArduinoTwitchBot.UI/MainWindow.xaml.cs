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
	// 10. spakować bool czyAktywny + sygnał + typ sygnału w jakąś klasę, którą przekażemy do Bota?
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

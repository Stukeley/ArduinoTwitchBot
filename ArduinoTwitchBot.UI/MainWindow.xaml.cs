using System.ComponentModel;
using System.Windows;

namespace ArduinoTwitchBot.UI
{
	// TODO:
	// 3. Different font
	// 6. zamknięcie aplikacji minimalizuje do system tray (niemożliwe)
	// 11. testy
	// 12. zapisywanie w usersettings też pozostałych pól (checkboxów, sygnałów)
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

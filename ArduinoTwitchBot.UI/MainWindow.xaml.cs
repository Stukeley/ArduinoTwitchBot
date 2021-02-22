using ArduinoTwitchBot.UI.Pages;
using System.ComponentModel;
using System.Windows;

namespace ArduinoTwitchBot.UI
{
	// TODO:
	// 3. Different font
	// 6. zamknięcie aplikacji minimalizuje do system tray (niemożliwe)
	// 11. testy
	// 13. wczytywanie settings
	// 14. json zamiast settings
	// 15. settingsy mogą być overwritowane przez VS - sprawdzić release
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			// Check what the current page is, and save its contents.
			if (this.PageFrame.Content is MainPage mainPage)
			{
				mainPage.SaveAlerts();
			}
			else if (this.PageFrame.Content is SettingsPage settingsPage)
			{
				settingsPage.SaveSettings();
			}

			// Save user settings before quitting.
			UserSettings.SaveUserSettings();

			base.OnClosing(e);
		}
	}
}

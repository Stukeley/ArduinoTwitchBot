using ArduinoTwitchBot.Code;
using ArduinoTwitchBot.UI.Pages;
using System;
using System.ComponentModel;
using System.Windows;

namespace ArduinoTwitchBot.UI
{
	// TODO:
	// 1. Different font
	// 2. settingsy mogą być overwritowane przez VS - sprawdzić release
	// 3. Dark mode
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
			try
			{
				UserSettings.SaveUserSettings();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			// Disconnect the bot (if it's running).
			TwitchBot.Instance.Disconnect();

			base.OnClosing(e);
		}
	}
}

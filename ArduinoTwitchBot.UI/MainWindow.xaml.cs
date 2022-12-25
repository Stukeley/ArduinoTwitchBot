namespace ArduinoTwitchBot.UI;

using Code;
using Pages;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Windows;

public partial class MainWindow : Window
{
	private readonly PaletteHelper _paletteHelper = new PaletteHelper();

	public MainWindow()
	{
		InitializeComponent();
	}

	public void ChangeApplicationTheme(bool isDark)
	{
		// Switch theme based on the IsDark property.
		var theme = _paletteHelper.GetTheme();
		var baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
		theme.SetBaseTheme(baseTheme);
		_paletteHelper.SetTheme(theme);
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
		else if (this.PageFrame.Content is EmotesPage emotesPage)
		{
			emotesPage.SaveEmotes();
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

		// Disconnect both bots (if they are running).
		TwitchBot.Instance.DisconnectPubSubClient();
		TwitchBot.Instance.DisconnectChatClient();

		base.OnClosing(e);
	}
}
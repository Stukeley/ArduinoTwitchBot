namespace ArduinoTwitchBot.UI;

using System.Windows;

public partial class App : Application
{
	private void Application_Startup(object sender, StartupEventArgs e)
	{
		// Load UserSettings.
		UserSettings.LoadUserSettings();
	}
}
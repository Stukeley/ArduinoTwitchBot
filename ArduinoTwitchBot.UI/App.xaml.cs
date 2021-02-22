using System.Windows;

namespace ArduinoTwitchBot.UI
{
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			// Load UserSettings.
			UserSettings.LoadUserSettings();
		}
	}
}

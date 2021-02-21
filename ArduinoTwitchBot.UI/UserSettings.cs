using System;

namespace ArduinoTwitchBot.UI
{
	public static class UserSettings
	{
		public static string PortName { get; set; }
		public static string ApiKey { get; set; }
		public static string ChannelName { get; set; }

		// Called upon leaving the app.
		public static void SaveUserSettings()
		{
			try
			{
				// Check if property exists.
				Properties.Settings.Default["PortName"] = PortName;
			}
			catch (Exception)
			{
				// Property does not exist - create it.
				var property = new System.Configuration.SettingsProperty("PortName")
				{
					PropertyType = typeof(string),
					DefaultValue = ""
				};

				Properties.Settings.Default.Properties.Add(property);
				Properties.Settings.Default.Save();
			}

			try
			{
				// Check if property exists.
				Properties.Settings.Default["ApiKey"] = PortName;
			}
			catch (Exception)
			{
				// Property does not exist - create it.
				var property = new System.Configuration.SettingsProperty("ApiKey")
				{
					PropertyType = typeof(string),
					DefaultValue = ""
				};

				Properties.Settings.Default.Properties.Add(property);
				Properties.Settings.Default.Save();
			}

			try
			{
				// Check if property exists.
				Properties.Settings.Default["ChannelName"] = PortName;
			}
			catch (Exception)
			{
				// Property does not exist - create it.
				var property = new System.Configuration.SettingsProperty("ChannelName")
				{
					PropertyType = typeof(string),
					DefaultValue = ""
				};

				Properties.Settings.Default.Properties.Add(property);
				Properties.Settings.Default.Save();
			}
		}

		public static void LoadUserSettings()
		{
			try
			{
				// Check if properties exist.
				PortName = Properties.Settings.Default["PortName"].ToString();
				ApiKey = Properties.Settings.Default["ApiKey"].ToString();
				ChannelName = Properties.Settings.Default["ChannelName"].ToString();
			}
			catch (Exception ex)
			{
				// Properties were not found.
				PortName = "";
				ApiKey = "";
				ChannelName = "";
			}
		}
	}
}

using System;

namespace ArduinoTwitchBot.UI
{
	public static class UserSettings
	{
		public static string PortName { get; set; }
		public static string ApiKey { get; set; }
		public static string ChannelName { get; set; }
		public static string[] Alerts { get; set; }

		// Called upon leaving the app.
		public static void SaveUserSettings()
		{
			try
			{
				// Check if property exists.
				Properties.Settings.Default["PortName"] = PortName;
				Properties.Settings.Default.Save();
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
				Properties.Settings.Default["ApiKey"] = ApiKey;
				Properties.Settings.Default.Save();
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
				Properties.Settings.Default["ChannelName"] = ChannelName;
				Properties.Settings.Default.Save();
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

			try
			{
				// Convert string[] to StringCollection
				var collection = new System.Collections.Specialized.StringCollection();
				collection.AddRange(Alerts);

				// Check if property exists.
				Properties.Settings.Default.Alerts = collection;
				Properties.Settings.Default.Save();
			}
			catch (Exception)
			{

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
				Properties.Settings.Default.Alerts.CopyTo(Alerts, 0);
			}
			catch (Exception)
			{
				// Properties were not found.
				PortName = "";
				ApiKey = "";
				ChannelName = "";
				Alerts = null;
			}
		}
	}
}

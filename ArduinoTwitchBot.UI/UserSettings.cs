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
			// Check if property exists.
			if (Properties.Settings.Default["PortName"] is null)
			{
				// Create the property.
				var property = new System.Configuration.SettingsProperty("PortName")
				{
					PropertyType = typeof(string),
					DefaultValue = ""
				};

				Properties.Settings.Default.Properties.Add(property);
				Properties.Settings.Default.Save();
			}

			Properties.Settings.Default["PortName"] = PortName;

			// Check if property exists.
			if (Properties.Settings.Default["ApiKey"] is null)
			{
				// Create the property.
				var property = new System.Configuration.SettingsProperty("ApiKey")
				{
					PropertyType = typeof(string),
					DefaultValue = ""
				};

				Properties.Settings.Default.Properties.Add(property);
				Properties.Settings.Default.Save();
			}

			Properties.Settings.Default["ApiKey"] = PortName;

			// Check if property exists.
			if (Properties.Settings.Default["ChannelName"] is null)
			{
				// Create the property.
				var property = new System.Configuration.SettingsProperty("ChannelName")
				{
					PropertyType = typeof(string),
					DefaultValue = ""
				};

				Properties.Settings.Default.Properties.Add(property);
				Properties.Settings.Default.Save();
			}

			Properties.Settings.Default["ChannelName"] = PortName;
		}
	}
}

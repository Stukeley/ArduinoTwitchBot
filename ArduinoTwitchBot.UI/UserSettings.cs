using ArduinoTwitchBot.Code;
using System;
using System.Collections.Generic;

namespace ArduinoTwitchBot.UI
{
	using System.Windows;
	using Pages;

	public static class UserSettings
	{
		#region General properties

		public static string PortName { get; set; }
		public static string ClientId { get; set; }
		public static string AccessToken { get; set; }
		public static string ChannelName { get; set; }
		public static Alert[] Alerts { get; set; }
		public static bool IsDarkTheme { get; set; }
		public static List<ChatMessageEntry> ChatMessageEntries { get; set; }

		#endregion

		// Called upon leaving the app.
		public static void SaveUserSettings()
		{
			try
			{
				// Save user settings.
				Properties.Settings.Default["PortName"] = PortName;
				Properties.Settings.Default["ClientId"] = ClientId;
				Properties.Settings.Default["AccessToken"] = AccessToken;
				Properties.Settings.Default["ChannelName"] = ChannelName;
				Properties.Settings.Default["IsDarkTheme"] = IsDarkTheme;

				// Save alert settings.
				Properties.Settings.Default["FollowAlert"] = Alerts[0].IsActive;
				Properties.Settings.Default["FollowAlertValue"] = Alerts[0].Signal;
				Properties.Settings.Default["FollowAlertType"] = Alerts[0].SignalType.ToString();

				Properties.Settings.Default["SubAlert"] = Alerts[1].IsActive;
				Properties.Settings.Default["SubAlertValue"] = Alerts[1].Signal;
				Properties.Settings.Default["SubAlertType"] = Alerts[1].SignalType.ToString();

				Properties.Settings.Default["BitsAlert"] = Alerts[2].IsActive;
				Properties.Settings.Default["BitsAlertValue"] = Alerts[2].Signal;
				Properties.Settings.Default["BitsAlertType"] = Alerts[2].SignalType.ToString();

				Properties.Settings.Default["RaidAlert"] = Alerts[3].IsActive;
				Properties.Settings.Default["RaidAlertValue"] = Alerts[3].Signal;
				Properties.Settings.Default["RaidAlertType"] = Alerts[3].SignalType.ToString();

				// Host Alert is kept here for compatibility reasons, but it is not used anymore in the app.
				// https://help.twitch.tv/s/article/how-to-use-host-mode?language=en_US#faq
				Properties.Settings.Default["HostAlert"] = Alerts[4].IsActive;
				Properties.Settings.Default["HostAlertValue"] = Alerts[4].Signal;
				Properties.Settings.Default["HostAlertType"] = Alerts[4].SignalType.ToString();

				Properties.Settings.Default["EmoteAlert"] = Alerts[5].IsActive;

				// Save emotes list.
				Properties.Settings.Default["ChatMessageEntries"] = ChatMessageEntry.Serialize(ChatMessageEntries);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}", "Error saving user data", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				// Save whatever got written.
				Properties.Settings.Default.Save();
			}
		}

		public static void LoadUserSettings()
		{
			try
			{
				// Check if properties exist.
				PortName = Properties.Settings.Default.PortName.ToString();
				ClientId = Properties.Settings.Default["ClientId"].ToString();
				AccessToken = Properties.Settings.Default["AccessToken"].ToString();
				ChannelName = Properties.Settings.Default["ChannelName"].ToString();
				IsDarkTheme = (bool)Properties.Settings.Default["IsDarkTheme"];

				// Load alert settings.
				Alerts = new Alert[6]
				{
					new Alert(bool.Parse(Properties.Settings.Default["FollowAlert"].ToString()), Properties.Settings.Default["FollowAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["FollowAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["SubAlert"].ToString()), Properties.Settings.Default["SubAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["SubAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["BitsAlert"].ToString()), Properties.Settings.Default["BitsAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["BitsAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["RaidAlert"].ToString()), Properties.Settings.Default["RaidAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["RaidAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["HostAlert"].ToString()), Properties.Settings.Default["HostAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["HostAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["EmoteAlert"].ToString()))
				};

				// Load emotes list.
				ChatMessageEntries = ChatMessageEntry.Deserialize(Properties.Settings.Default["ChatMessageEntries"].ToString());
				EmotesPage.ChatMessages = ChatMessageEntries;
			}
			catch (Exception)
			{
				// Some (or all) properties were not found.
				PortName = PortName ?? "";
				ClientId = ClientId ?? "";
				AccessToken = AccessToken ?? "";
				ChannelName = ChannelName ?? "";
				Alerts = Alerts ?? Array.Empty<Alert>();
				ChatMessageEntries = ChatMessageEntries ?? new List<ChatMessageEntry>();
			}
		}
	}
}

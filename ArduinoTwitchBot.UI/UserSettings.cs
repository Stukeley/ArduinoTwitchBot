﻿using ArduinoTwitchBot.Code;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ArduinoTwitchBot.UI
{
	public static class UserSettings
	{
		#region General properties

		public static string PortName { get; set; }
		public static string ClientId { get; set; }
		public static string AccessToken { get; set; }
		public static string ChannelName { get; set; }
		public static Alert[] Alerts { get; set; }
		public static bool IsDarkTheme { get; set; }
		public static List<string> EmotesList { get; set; }

		#endregion

		#region Serialized properties

		public static bool FollowAlert { get; set; }
		public static string FollowAlertValue { get; set; }
		public static SignalType FollowAlertType { get; set; }

		public static bool SubAlert { get; set; }
		public static string SubAlertValue { get; set; }
		public static SignalType SubAlertType { get; set; }

		public static bool BitsAlert { get; set; }
		public static string BitsAlertValue { get; set; }
		public static SignalType BitsAlertType { get; set; }

		public static bool RaidAlert { get; set; }
		public static string RaidAlertValue { get; set; }
		public static SignalType RaidAlertType { get; set; }

		public static bool HostAlert { get; set; }
		public static string HostAlertValue { get; set; }
		public static SignalType HostAlertType { get; set; }

		public static bool EmoteAlert { get; set; }
		public static string EmoteAlertValue { get; set; }
		public static SignalType EmoteAlertType { get; set; }

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

				Properties.Settings.Default["HostAlert"] = Alerts[4].IsActive;
				Properties.Settings.Default["HostAlertValue"] = Alerts[4].Signal;
				Properties.Settings.Default["HostAlertType"] = Alerts[4].SignalType.ToString();

				Properties.Settings.Default["EmoteAlert"] = Alerts[5].IsActive;
				Properties.Settings.Default["EmoteAlertValue"] = Alerts[5].Signal;
				Properties.Settings.Default["EmoteAlertType"] = Alerts[5].SignalType.ToString();

				// Save emotes list.
				var stringCollection = new StringCollection();
				stringCollection.AddRange(EmotesList.ToArray());
				Properties.Settings.Default["EmotesList"] = stringCollection;
			}
			catch (Exception)
			{
				throw;
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
					new Alert(bool.Parse(Properties.Settings.Default["FollowerAlert"].ToString()), Properties.Settings.Default["FollowerAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["FollowerAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["SubAlert"].ToString()), Properties.Settings.Default["SubAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["SubAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["BitsAlert"].ToString()), Properties.Settings.Default["BitsAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["BitsAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["RaidAlert"].ToString()), Properties.Settings.Default["RaidAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["RaidAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["HostAlert"].ToString()), Properties.Settings.Default["HostAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["HostAlertType"].ToString())),

					new Alert(bool.Parse(Properties.Settings.Default["EmoteAlert"].ToString()), Properties.Settings.Default["EmoteAlertValue"].ToString(), (SignalType)Enum.Parse(typeof(SignalType),Properties.Settings.Default["EmoteAlertType"].ToString()))
				};

				// Load emotes list.
				EmotesList = (Properties.Settings.Default["EmotesList"] as StringCollection).Cast<string>().ToList();
			}
			catch (Exception)
			{
				// Some (or all) properties were not found.
				PortName = PortName is null ? "" : PortName;
				ClientId = ClientId is null ? "" : ClientId;
				AccessToken = AccessToken is null ? "" : AccessToken;
				ChannelName = ChannelName is null ? "" : ChannelName;
				Alerts = Alerts is null ? Array.Empty<Alert>() : Alerts;
				EmotesList = EmotesList is null ? new List<string>() : EmotesList;
			}
		}
	}
}

using System;

namespace ArduinoTwitchBot.Code
{
	public class TwitchEvent
	{
		public string Username { get; set; }
		public DateTime EventTime { get; set; }
		public TwitchEventType EventType { get; set; }

		public TwitchEvent(string username, DateTime eventTime, TwitchEventType eventType)
		{
			Username = username;
			EventTime = eventTime;
			EventType = eventType;
		}
	}
}

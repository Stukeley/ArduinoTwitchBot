using System.Collections.ObjectModel;

namespace ArduinoTwitchBot.Code
{
	public static class EventHistory
	{
		public static ObservableCollection<TwitchEvent> TwitchEvents { get; set; }

		static EventHistory()
		{
			TwitchEvents = new ObservableCollection<TwitchEvent>();
		}
	}
}

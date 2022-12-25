namespace ArduinoTwitchBot.Code;

using System.Collections.ObjectModel;

public static class EventHistory
{
	public static ObservableCollection<TwitchEvent> TwitchEvents { get; set; }

	static EventHistory()
	{
		TwitchEvents = new ObservableCollection<TwitchEvent>();
	}
}
namespace ArduinoTwitchBot.Code;

using System.Collections.Generic;
using System.Text.Json;

public class ChatMessageEntry
{
	public string Id { get; set; }
	public string Message { get; set; }
	public Alert Alert { get; set; }

	public ChatMessageEntry()
	{
	}
		
	public ChatMessageEntry(string id, string message, string signal, SignalType alertType)
	{
		Id = id;
		Message = message;
		Alert = new Alert(true, signal, alertType);
	}

	public static string Serialize(List<ChatMessageEntry> entries)
	{
		var json = JsonSerializer.Serialize(entries);
		return json;
	}

	public static List<ChatMessageEntry> Deserialize(string serialized)
	{
		var deserialized = JsonSerializer.Deserialize<List<ChatMessageEntry>>(serialized);
		return deserialized;
	}
}
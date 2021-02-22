namespace ArduinoTwitchBot.Code
{
	public class Alert
	{
		public bool IsActive { get; set; }
		public string Signal { get; set; }
		public SignalType SignalType { get; set; }

		public static bool operator true(Alert alert)
		{
			return alert.IsActive == true;
		}

		public static bool operator false(Alert alert)
		{
			return alert.IsActive == false;
		}

		public Alert(bool isActive, string signal = null, SignalType signalType = SignalType.String)
		{
			IsActive = isActive;
		}
	}
}

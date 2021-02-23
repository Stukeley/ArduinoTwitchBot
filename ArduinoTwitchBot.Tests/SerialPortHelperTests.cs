using ArduinoTwitchBot.Code;
using NUnit.Framework;

namespace ArduinoTwitchBot.Tests
{
	[TestFixture]
	public class SerialPortHelperTests
	{
		[Test]
		public void SendMessage_NoExceptionThrown()
		{
			string portName = "COM3";
			string message = "Test Running";
			SignalType signalType = SignalType.String;

			Assert.DoesNotThrow(() => SerialPortHelper.SendMessage(portName, message, signalType));
		}
	}
}

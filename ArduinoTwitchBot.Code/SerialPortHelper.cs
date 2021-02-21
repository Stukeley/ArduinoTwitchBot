using System;
using System.IO.Ports;

namespace ArduinoTwitchBot.Code
{
	public enum SignalType
	{
		String = 0, Int, Byte
	}
	public static class SerialPortHelper
	{
		private const int BAUD_RATE = 9600;
		private const int READ_TIMEOUT = 2000;
		private const int WRITE_TIMEOUT = 2000;

		// Sends a given message using the specified COM port name. The message is interpreted differently based on the SignalType.
		public static void SendMessage(string portName, string message, SignalType signalType)
		{
			var port = new SerialPort()
			{
				PortName = portName,
				BaudRate = BAUD_RATE,
				ReadTimeout = READ_TIMEOUT,
				WriteTimeout = WRITE_TIMEOUT
			};

			try
			{
				port.Open();

				// Parse the message.
				switch (signalType)
				{
					case SignalType.String:
						{
							// Send the message directly.
							port.Write(message);

							break;
						}

					case SignalType.Int:
						{
							// Convert the int to Byte Array.
							var bytes = BitConverter.GetBytes(int.Parse(message));

							port.Write(bytes, 0, 4);

							break;
						}

					case SignalType.Byte:
						{
							// Just like above, but only send one byte.
							var bytes = BitConverter.GetBytes(byte.Parse(message));

							port.Write(bytes, 0, 4);
							break;
						}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				port.Close();
			}
		}
	}
}

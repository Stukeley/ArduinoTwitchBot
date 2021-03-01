using System;
using System.IO.Ports;

namespace ArduinoTwitchBot.Code
{
	public static class SerialPortHelper
	{
		// There is no way to change these as of now.
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
							var isInt = int.TryParse(message, out int intMessage);
							if (isInt)
							{
								// Convert the int to Byte Array.
								var bytes = BitConverter.GetBytes(intMessage);

								port.Write(bytes, 0, 4);
							}

							break;
						}

					case SignalType.Byte:
						{
							var isByte = byte.TryParse(message, out byte byteMessage);
							if (isByte)
							{
								// Just like above, but only send one byte.
								var bytes = BitConverter.GetBytes(byteMessage);

								port.Write(bytes, 0, 1);
							}

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

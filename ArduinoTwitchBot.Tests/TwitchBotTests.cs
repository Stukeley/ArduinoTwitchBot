using ArduinoTwitchBot.Code;
using NUnit.Framework;

namespace ArduinoTwitchBot.Tests
{
	[TestFixture]
	public class TwitchBotTests
	{
		private TwitchBot _twitchBot;

		[SetUp]
		public void SetUp()
		{
			_twitchBot = new TwitchBot()
			{
				ClientId = Properties.Resources.ClientId,
				AccessToken = Properties.Resources.AccessToken
			};
		}

		[Test]
		public void GetChannelId_ValidResponse_ApiNotNull()
		{
			var channelName = "Stukeleyak";
			var result = _twitchBot.GetChannelId(channelName).Result;

			// ID for user of name Stukeleyak
			var expected = "81452434";

			Assert.AreEqual(expected, result);
			Assert.IsNotNull(_twitchBot._api);
		}

		[Test]
		public void Connect_NoExceptionThrown_ClientNotNull()
		{
			var channelName = "Stukeleyak";
			var portName = "COM3";

			var alerts = new Alert[6]
			{
				new Alert(true, "Follow"),
				new Alert(true, "Sub"),
				new Alert(true, "Bits"),
				new Alert(false),
				new Alert(true, "Raid"),
				new Alert(false)
			};

			Assert.DoesNotThrow(() => _twitchBot.Connect(Properties.Resources.ClientId, Properties.Resources.AccessToken, channelName, portName, alerts));
			Assert.IsNotNull(_twitchBot.ClientId);
			Assert.IsNotNull(_twitchBot.AccessToken);
			Assert.IsNotNull(_twitchBot.PortName);
			Assert.IsNotNull(_twitchBot.Alerts);
			Assert.IsNotNull(_twitchBot._client);
		}

		[Test]
		public void Disconnect_NoExceptionThrown()
		{
			Assert.DoesNotThrow(() => _twitchBot.Disconnect());
		}
	}
}

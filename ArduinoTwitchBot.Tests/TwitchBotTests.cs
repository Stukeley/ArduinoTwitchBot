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
		public void GetChannelId_ValidResponse()
		{
			var channelName = "Stukeleyak";
			var result = _twitchBot.GetChannelId(channelName).Result;

			// ID for user of name Stukeleyak
			var expected = "81452434";

			Assert.AreEqual(expected, result);
		}
	}
}

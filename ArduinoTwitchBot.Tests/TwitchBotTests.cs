using ArduinoTwitchBot.Code;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ArduinoTwitchBot.Tests
{
	[TestFixture]
	public class TwitchBotTests
	{
		private TwitchBot _twitchBot;

		[Test]
		public void InitializeBot_ThrowsWhenParametersNull()
		{
			try
			{
				_twitchBot.InitializeBot("", null, "Stukeleyak", "COM3", new Alert[5]);
				Assert.Fail("An exception should have been thrown - null parameters.");
			}
			catch (Exception)
			{
				Assert.Pass("An exception was correctly thrown.");
			}
		}

		[Test]
		public void InitializeBot_DoesNotThrow_FieldsNotNull()
		{
			_twitchBot = new TwitchBot();

			var alerts = new Alert[6]
			{
				new Alert(true, "Follow"),
				new Alert(true, "Sub"),
				new Alert(true, "Bits"),
				new Alert(false),
				new Alert(true, "Raid"),
				new Alert(false)
			};

			Assert.DoesNotThrow(() => _twitchBot.InitializeBot(Properties.Resources.ClientId, Properties.Resources.AccessToken, "Stukeleyak", "COM3", alerts));
			Assert.IsNotNull(_twitchBot.ClientId);
			Assert.IsNotNull(_twitchBot.AccessToken);
			Assert.IsNotNull(_twitchBot.ChannelName);
			Assert.IsNotNull(_twitchBot.PortName);
			Assert.IsNotNull(_twitchBot.Alerts);
		}

		[SetUp]
		public void SetUp()
		{
			// Set up for all other tests.
			_twitchBot = new TwitchBot();

			var alerts = new Alert[6]
			{
				new Alert(true, "Follow"),
				new Alert(true, "Sub"),
				new Alert(true, "Bits"),
				new Alert(false),
				new Alert(true, "Raid"),
				new Alert(false)
			};

			_twitchBot.InitializeBot(Properties.Resources.ClientId, Properties.Resources.AccessToken, "Stukeleyak", "COM3", alerts);
		}

		[Test]
		public void GetChannelId_ValidResponse_ApiNotNull()
		{
			var channelName = "Stukeleyak";
			var result = _twitchBot.GetChannelId(channelName).Result;

			// ID for user of name Stukeleyak.
			var expected = "81452434";

			Assert.AreEqual(expected, result);
			Assert.IsNotNull(_twitchBot.API);
		}

		[Test]
		public void Connect_NoExceptionThrown_ClientNotNull()
		{
			Assert.DoesNotThrow(() => _twitchBot.ConnectPubSubClient());
			Assert.IsNotNull(_twitchBot.PubSubClient);
		}


		[Test]
		public void DisconnectPubSubClient_NoExceptionThrown()
		{
			Assert.DoesNotThrow(() => _twitchBot.DisconnectPubSubClient());
		}

		[Test]
		public void ConnectChatClient_NoExceptionThrown_ChatClientNotNull()
		{
			var chatMessageEntries = new List<ChatMessageEntry>()
			{
				new ChatMessageEntry("123", "Kappa", "SIG", SignalType.String)
			};

			Assert.DoesNotThrow(() => _twitchBot.ConnectChatClient(chatMessageEntries));
			Assert.IsNotNull(_twitchBot.ChatClient);
			Assert.IsNotNull(_twitchBot.ChatMessageEntries);
		}

		[Test]
		public void DisconnectChatClient_NoExceptionThrown()
		{
			Assert.DoesNotThrow(() => _twitchBot.DisconnectChatClient());
		}
	}
}

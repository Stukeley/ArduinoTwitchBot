using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.PubSub;

namespace ArduinoTwitchBot.Code
{
	public class TwitchBot
	{
		#region Singleton
		private static TwitchBot _instance;
		public static TwitchBot Instance
		{
			get
			{
				if (_instance is null)
				{
					_instance = new TwitchBot();
				}
				return _instance;
			}
		}
		#endregion

		public TwitchBot()
		{
			Alerts = new Alert[6];
		}

		public string ClientId { get; set; }
		public string AccessToken { get; set; }
		public string PortName { get; set; }
		// Follows, subs, bits, raids, hosts and emotes, in this order.
		public Alert[] Alerts { get; set; }

		// API to get Channel Id from username.
		public TwitchAPI _api { get; private set; }

		// Client for listening to alerts (follows, subs, raids, hosts, bits).
		public TwitchPubSub _client { get; private set; }

		// Experimental client for listening to chat messages
		public TwitchClient _chatClient { get; private set; }
		public List<string> EmotesList { get; set; }

		// Connect the PubSub client.
		public async void Connect(string clientId, string accessToken, string channelName, string portName, Alert[] alerts)
		{
			// Check if the needed parameters are provided.
			if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(channelName) || string.IsNullOrEmpty(portName))
			{
				throw new Exception("One of the required parameters was not provided. Make sure to specify: ClientId, AccessToken, ChannelName and PortName.");
			}

			// Write ClientId and AccessToken.
			ClientId = clientId;
			AccessToken = accessToken;
			Alerts = alerts;
			PortName = portName;

			_client = new TwitchPubSub();

			string channelId = await GetChannelId(channelName);

			// Listen to events.
			_client.OnPubSubServiceConnected += Client_OnPubSubServiceConnected;
			_client.OnListenResponse += Client_OnListenResponse;

			if (alerts[0])
			{
				_client.ListenToFollows(channelId);
				_client.OnFollow += this.Client_OnFollow;
			}

			if (alerts[1])
			{
				_client.ListenToSubscriptions(channelId);
				_client.OnChannelSubscription += this.Client_OnChannelSubscription;
			}

			if (alerts[2])
			{
				_client.ListenToBitsEvents(channelId);
				_client.OnBitsReceived += this.Client_OnBitsReceived;
			}

			if (alerts[3])
			{
				_client.ListenToRaid(channelId);
				_client.OnRaidGo += this.Client_OnRaidGo;
			}

			if (alerts[4])
			{
				_client.ListenToRaid(channelId);
				_client.OnHost += this.Client_OnHost;
			}

			// ChatClient is connected from a different place.

			_client.Connect();
		}

		// Disconnect the PubSub client.
		public void Disconnect()
		{
			_client?.Disconnect();
		}

		// Connect the ClientChat (Emote alerts).
		public void ConnectChatClient(string accessToken, string channelName, string portName, Alert emoteAlert, List<string> emotesList, string botName = "ArduinoBot")
		{
			// Make sure EmotesList has been filled up by the user.
			if (EmotesList?.Count == 0)
			{
				throw new Exception("Emote list is empty - unable to listen to emotes sent in chat.");
			}

			AccessToken = accessToken;
			EmotesList = emotesList;
			PortName = portName;
			Alerts[5] = emoteAlert;

			_chatClient = new TwitchClient();
			var credentials = new ConnectionCredentials(botName, accessToken);
			_chatClient.Initialize(credentials, channelName);

			_chatClient.OnJoinedChannel += ChatClient_OnJoinedChannel;
			_chatClient.OnMessageReceived += ChatClient_OnMessageReceived;
			_chatClient.OnConnected += ChatClient_OnConnected; ;

			_chatClient.Connect();
		}

		private void ChatClient_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
		{
#if DEBUG
			Trace.WriteLine($"Connected bot of username: {e.BotUsername} to channel: {e.AutoJoinChannel}");
#endif
		}

		private void ChatClient_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
		{
			// Check if message contains an emote that is on the list.
			if (EmotesList.Any(x => e.ChatMessage.Message.Contains(x)))
			{
				// Send a signal.
				try
				{
#if DEBUG
					Trace.WriteLine("Emote received!");
#endif
					SerialPortHelper.SendMessage(PortName, Alerts[5].Signal, Alerts[5].SignalType);
				}
				catch (Exception ex)
				{
#if DEBUG
					Trace.WriteLine(ex.Message);
#endif
				}
			}
		}

		private void ChatClient_OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
		{
#if DEBUG
			Trace.WriteLine($"Joined channel: {e.Channel}");
#endif
		}

		// Disconnect the ClientChat (Emote alerts).
		public void DisconnectChatClient()
		{
			_chatClient?.Disconnect();
		}

		public async Task<string> GetChannelId(string channelName)
		{
			string channelId = "";

			try
			{
				_api = new TwitchAPI();

				_api.Settings.ClientId = ClientId;
				_api.Settings.AccessToken = AccessToken;

				var users = await _api.Helix.Users.GetUsersAsync(logins: new List<string>() { channelName });

				if (users.Users.Length > 0)
				{
					channelId = users.Users[0].Id;
				}
				else
				{
					throw new Exception("User of the given name not found");
				}
			}
			catch (Exception ex)
			{
#if DEBUG
				Trace.WriteLine(ex.Message);
#endif
			}

#if DEBUG
			Trace.WriteLine($"ChannelId for {channelName}: {channelId}");
#endif

			return channelId;
		}

		private void Client_OnListenResponse(object sender, TwitchLib.PubSub.Events.OnListenResponseArgs e)
		{
			if (!e.Successful)
			{
#if DEBUG
				Trace.WriteLine($"Failed to listen! Response: {e.Response.Error}");
#endif
			}
		}

		private void Client_OnPubSubServiceConnected(object sender, EventArgs e)
		{
			// Not sure what this is for.
			_client?.SendTopics(AccessToken);
		}

		// TODO! zły event, to jest host w drugą stronę
		private void Client_OnHost(object sender, TwitchLib.PubSub.Events.OnHostArgs e)
		{
#if DEBUG
			Trace.WriteLine("Host received!");
#endif
			try
			{
				SerialPortHelper.SendMessage(PortName, Alerts[4].Signal, Alerts[4].SignalType);
				//EventHistory.TwitchEvents.Add(new TwitchEvent(e.HostedChannel));
			}
			catch (Exception ex)
			{
#if DEBUG
				Trace.WriteLine(ex.Message);
#endif
			}
		}

		// TODO to chyba też
		private void Client_OnRaidGo(object sender, TwitchLib.PubSub.Events.OnRaidGoArgs e)
		{
#if DEBUG
			Trace.WriteLine("Raid received!");
#endif
			try
			{
				SerialPortHelper.SendMessage(PortName, Alerts[3].Signal, Alerts[3].SignalType);
			}
			catch (Exception ex)
			{
#if DEBUG
				Trace.WriteLine(ex.Message);
#endif
			}
		}

		private void Client_OnBitsReceived(object sender, TwitchLib.PubSub.Events.OnBitsReceivedArgs e)
		{
#if DEBUG
			Trace.WriteLine("Bits received!");
#endif
			try
			{
				SerialPortHelper.SendMessage(PortName, Alerts[2].Signal, Alerts[2].SignalType);
				EventHistory.TwitchEvents.Insert(0, new TwitchEvent(e.Username, DateTime.Now, TwitchEventType.Bits));
			}
			catch (Exception ex)
			{
#if DEBUG
				Trace.WriteLine(ex.Message);
#endif
			}
		}

		private void Client_OnChannelSubscription(object sender, TwitchLib.PubSub.Events.OnChannelSubscriptionArgs e)
		{
#if DEBUG
			Trace.WriteLine("Sub received!");
#endif
			try
			{
				SerialPortHelper.SendMessage(PortName, Alerts[1].Signal, Alerts[1].SignalType);
				EventHistory.TwitchEvents.Insert(0, new TwitchEvent(e.Subscription.Username, DateTime.Now, TwitchEventType.Sub));
			}
			catch (Exception ex)
			{
#if DEBUG
				Trace.WriteLine(ex.Message);
#endif
			}
		}

		private void Client_OnFollow(object sender, TwitchLib.PubSub.Events.OnFollowArgs e)
		{
#if DEBUG
			Trace.WriteLine("Follow received!");
#endif
			try
			{
				SerialPortHelper.SendMessage(PortName, Alerts[0].Signal, Alerts[0].SignalType);
				EventHistory.TwitchEvents.Insert(0, new TwitchEvent(e.DisplayName, DateTime.Now, TwitchEventType.Follow));
			}
			catch (Exception ex)
			{
#if DEBUG
				Trace.WriteLine(ex.Message);
#endif
			}
		}
	}
}

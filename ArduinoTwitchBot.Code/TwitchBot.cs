namespace ArduinoTwitchBot.Code;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.PubSub;

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

	// Public ClientId.
	public string ClientId { get; set; }

	// Private AccessToken - never to be shown anywhere.
	public string AccessToken { get; set; }

	// Port through which the signals will be sent.
	public string PortName { get; set; }

	// Channel name to which the bot will connect.
	public string ChannelName { get; set; }

	// Follows, subs, bits, raids, hosts and emotes, in this order. Should always have a Length of 6.
	public Alert[] Alerts { get; set; }

	// List of emotes to look for in chat.
	public List<ChatMessageEntry> ChatMessageEntries { get; set; }

	// API to get Channel Id from username.
	public TwitchAPI API { get; private set; }

	// Client for listening to alerts (follows, subs, bits).
	public TwitchPubSub PubSubClient { get; private set; }

	// Client for listening to alerts (raids, hosts, emote events).
	public TwitchClient ChatClient { get; private set; }

	// This method has to be called first, before connecting either of the clients.
	public void InitializeBot(string clientId, string accessToken, string channelName, string portName, Alert[] alerts)
	{
		// Check if the needed parameters are provided.
		if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(channelName) || string.IsNullOrEmpty(portName))
		{
			throw new Exception("One of the required parameters was not provided. Make sure to specify: ClientId, AccessToken, ChannelName and PortName. The bot was not connected.");
		}

		// Write ClientId and AccessToken.
		ClientId = clientId;
		AccessToken = accessToken;
		ChannelName = channelName;
		PortName = portName;

		// Make sure that alerts always has a Count of 6.
		if (alerts.Length < 6)
		{
			throw new Exception("Error - Alerts count was below 6. If this issue persists, please report it.");
		}

		// Write Alerts.
		Alerts = alerts;
	}

	#region PubSubClient
	// Connect the PubSub client.
	public async void ConnectPubSubClient()
	{
		PubSubClient = new TwitchPubSub();

		string channelId = await GetChannelId(ChannelName);

		// Listen to events.
		PubSubClient.OnPubSubServiceConnected += Client_OnPubSubServiceConnected;
		PubSubClient.OnListenResponse += Client_OnListenResponse;

		if (Alerts[0])
		{
			// Follow alert.
			PubSubClient.ListenToFollows(channelId);
			PubSubClient.OnFollow += Client_OnFollow;
		}

		if (Alerts[1])
		{
			// Sub alert.
			PubSubClient.ListenToSubscriptions(channelId);
			PubSubClient.OnChannelSubscription += Client_OnChannelSubscription;
		}

		if (Alerts[2])
		{
			// Bits alert.
			PubSubClient.ListenToBitsEventsV2(channelId);
			PubSubClient.OnBitsReceived += Client_OnBitsReceived;
		}

		PubSubClient.Connect();
	}

	// Disconnect the PubSub client.
	public void DisconnectPubSubClient()
	{
		PubSubClient?.Disconnect();
	}
	#endregion

	#region ChatClient
	// Connect the ClientChat (Host, Raid, Emote alerts).
	public void ConnectChatClient(List<ChatMessageEntry> entries = null, string botName = "ArduinoBot")
	{
		ChatMessageEntries = entries;

		ChatClient = new TwitchClient();
		var credentials = new ConnectionCredentials(botName, AccessToken);
		ChatClient.Initialize(credentials, ChannelName);

		ChatClient.OnJoinedChannel += ChatClient_OnJoinedChannel;
		ChatClient.OnConnected += ChatClient_OnConnected;

		if (Alerts[3])
		{
			// Raid alert.
			ChatClient.OnRaidNotification += ChatClient_OnRaidNotification;
		}
		if (Alerts[5])
		{
			// Emote alert.
			// Make sure EmotesList has been filled up by the user.
			if (ChatMessageEntries?.Count == 0)
			{
				throw new Exception("Emote list is empty - unable to listen to emotes sent in chat. The bot was not connected.");
			}
			ChatClient.OnMessageReceived += ChatClient_OnMessageReceived;
		}

		ChatClient.Connect();

#if DEBUG
		Trace.WriteLine("PubSub Client connected!");
#endif
	}

	// Disconnect the ClientChat (Emote alerts).
	public void DisconnectChatClient()
	{
		ChatClient?.Disconnect();
	}
	#endregion

	public async Task<string> GetChannelId(string channelName)
	{
		string channelId = "";

		try
		{
			API = new TwitchAPI();

			API.Settings.ClientId = ClientId;
			API.Settings.AccessToken = AccessToken;

			var users = await API.Helix.Users.GetUsersAsync(logins: new List<string>() { channelName });

			if (users.Users.Length > 0)
			{
				channelId = users.Users[0].Id;
			}
			else
			{
				throw new Exception("User of the given name not found.");
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

	#region Events

	private void ChatClient_OnRaidNotification(object sender, TwitchLib.Client.Events.OnRaidNotificationArgs e)
	{
#if DEBUG
		Trace.WriteLine("Raid received!");
#endif
		try
		{
			SerialPortHelper.SendMessage(PortName, Alerts[3].Signal, Alerts[3].SignalType);
			EventHistory.TwitchEvents.Add(new TwitchEvent(e.RaidNotification.DisplayName, DateTime.Now, TwitchEventType.Raid));
		}
		catch (Exception ex)
		{
#if DEBUG
			Trace.WriteLine(ex.Message);
#endif
		}
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
		foreach (var entry in ChatMessageEntries)
		{
			if (e.ChatMessage.Message.Contains(entry.Message))
			{
				// Send a signal.
				try
				{
#if DEBUG
					Trace.WriteLine("Emote received!");
#endif
					SerialPortHelper.SendMessage(PortName, entry.Alert.Signal, entry.Alert.SignalType);
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

	private void ChatClient_OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
	{
#if DEBUG
		Trace.WriteLine($"Joined channel: {e.Channel}");
#endif
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
		PubSubClient?.SendTopics(AccessToken);
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

	#endregion
}
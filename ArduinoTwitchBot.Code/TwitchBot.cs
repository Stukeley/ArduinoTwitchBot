using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client;
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

		public string ClientId { get; set; }
		public string AccessToken { get; set; }
		public string PortName { get; set; }
		public Alert[] Alerts { get; set; }

		// API to get Channel Id from username.
		public TwitchAPI _api { get; private set; }

		// Client for listening to alerts (follows, subs, raids, hosts, bits).
		public TwitchPubSub _client { get; private set; }

		// Experimental client for listening to chat messages
		private TwitchClient _chatClient;

		public async void Connect(string clientId, string accessToken, string channelName, string portName, Alert[] alerts)
		{
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

			if (alerts[5])
			{
				// TODO
			}

			_client.Connect();
		}

		public void Disconnect()
		{
			_client?.Disconnect();
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


		private void Client_OnHost(object sender, TwitchLib.PubSub.Events.OnHostArgs e)
		{
#if DEBUG
			Trace.WriteLine("Host received!");
#endif
			try
			{
				SerialPortHelper.SendMessage(PortName, Alerts[4].Signal, Alerts[4].SignalType);
			}
			catch (Exception ex)
			{
#if DEBUG
				Trace.WriteLine(ex.Message);
#endif
			}
		}

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

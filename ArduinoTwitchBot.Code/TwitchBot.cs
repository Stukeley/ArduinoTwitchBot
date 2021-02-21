using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Api;
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

		// API to get Channel Id from username.
		private TwitchAPI _api;

		// Client for listening to alerts (follows, subs, raids, hosts, bits).
		private TwitchPubSub _client;

		// Experimental client for listening to chat messages

		public async void Connect(string apiKey, string channelName, bool followerAlert = false, bool subAlert = false, bool bitsAlert = false, bool raidAlert = false, bool hostAlert = false, bool emoteAlert = false)
		{
			_client = new TwitchPubSub();

			string channelId = await GetChannelId(channelName);

			if (followerAlert)
			{
				_client.ListenToFollows(channelId);
				_client.OnFollow += this.Client_OnFollow;
			}

			if (subAlert)
			{
				_client.ListenToSubscriptions(channelId);
				_client.OnChannelSubscription += this.Client_OnChannelSubscription;
			}

			if (bitsAlert)
			{
				_client.ListenToBitsEvents(channelId);
				_client.OnBitsReceived += this.Client_OnBitsReceived;
			}

			if (raidAlert)
			{
				_client.ListenToRaid(channelId);
				_client.OnRaidGo += this.Client_OnRaidGo;
			}

			if (hostAlert)
			{
				_client.ListenToRaid(channelId);
				_client.OnHost += this.Client_OnHost;
			}

			if (emoteAlert)
			{
				// TODO
			}
		}

		private async Task<string> GetChannelId(string channelName)
		{
			string channelId = "";

			try
			{
				_api = new TwitchAPI();

				_api.Settings.ClientId = "";
				_api.Settings.AccessToken = "";

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
			catch (Exception)
			{

			}

			return channelId;
		}

		private void Client_OnHost(object sender, TwitchLib.PubSub.Events.OnHostArgs e)
		{
			throw new NotImplementedException();
		}

		private void Client_OnRaidGo(object sender, TwitchLib.PubSub.Events.OnRaidGoArgs e)
		{
			throw new NotImplementedException();
		}

		private void Client_OnBitsReceived(object sender, TwitchLib.PubSub.Events.OnBitsReceivedArgs e)
		{
			throw new NotImplementedException();
		}

		private void Client_OnChannelSubscription(object sender, TwitchLib.PubSub.Events.OnChannelSubscriptionArgs e)
		{
			throw new NotImplementedException();
		}

		private void Client_OnFollow(object sender, TwitchLib.PubSub.Events.OnFollowArgs e)
		{
			throw new NotImplementedException();
		}
	}
}

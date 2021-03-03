<p align="center">
  <img src="ArduinoTwitchBot.UI/ArduinoTwitch.png?raw=true">
</p>

<img src="https://img.shields.io/badge/platform-.NET-lightgrey"> <a href="https://dotnet.microsoft.com/download/dotnet/5.0"> <img src="https://img.shields.io/badge/.NET-5.0-orange"></a> <img src="https://img.shields.io/badge/language-C%23-red"> <img src="https://img.shields.io/badge/license-MIT-brightgreen"> <a href="https://github.com/Stukeley/TwitchArduinoBot/subscription"><img alt="Star this repo" src="https://img.shields.io/github/stars/Stukeley/ArduinoTwitchBot?style=social"></a>

# ArduinoTwitchBot  
A customizable Twitch chatbot that sends out signals to Arduino whenever an event (eg. follow) occurs.

Built using [TwitchLib](https://github.com/TwitchLib/TwitchLib) and [MaterialDesign for XAML](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit).

## Table of Contents
* [How it works](#how-it-works)
* [Features](#features)
* [Setup](#setup)
* [Design](#design)
* [Sample schematic](#sample-schematic)
* [Feature requests](#feature-requests)

## <a name="how-it-works"></a>How it works
The bot connects to a Twitch user's chat and listens to events (such as: follows, subs, bits, raids and hosts). Whenever an event occurs, a signal is sent over to an Arduino using a serial port.

The type of signal sent, its value, as well as the COM port used can all be configured by the user.

To make this work, you will need an OAUTH Access Token from Twitch - an explanation on how to get one can be found in the Setup section.

**Important** - access tokens are channel-unique. That means you can only use the bot on the channel, on which you generated the token. You cannot listen to events on a random stream on Twitch!

## <a name="features"></a>Features
- The ability to select which events are listened to (the ability to choose from: follows, subs, bits, raids, hosts and emotes)
- Signal type as well as its value can be customized (eg. we can send the value "3" either as a string, an int or a byte)
- COM ports can be selected from a list of all available ports for your computer - no more guessing!
- You can send a test signal to your Arduino to see if the connection works, without having to connect to a channel
- Access Token (which should be kept confidential!) is hidden by default, so it's okay if you accidentally show the app on stream!
- Experimental feature - emote alerts. Sends out a signal whenever a specific emote is sent in chat

## <a name="setup"></a>Setup
0. Make sure you have the .NET 5 runtime installed: you can get it from [here](https://dotnet.microsoft.com/download/dotnet/5.0) under .NET Desktop Runtime.
1. Download the code under Releases.
2. Run the application. Go over to the Settings tab.
3. You now need to generate an Access Token. Go over [here](https://twitchtokengenerator.com/). Select "Bot Chat Token". Authorize the website and copy both the Client ID and the Access Token. **Important - while Client ID is public information and can be exposed, Access Token has to be kept confidential.** ![Get Client ID and Access Token from here](https://i.imgur.com/F8TlnY2.png)
4. Paste Client Id and Access Token in the respective TextBoxes in the Settings tab. Enter your Twitch channel's username. Select your desired COM port from the list below.
5. Go back to the Home tab. Your settings will be saved automatically (they will be there even if you relaunch the app).
6. Select desired alerts and make sure to input a value for each of the selected events.
7. If you're unsure of anything, hover over an element on the UI and a tooltip with more information will appear.

Let me know if you need more help!

## <a name="design"></a>Design
<img width="426" height="353" src="https://i.imgur.com/KXXnSQ0.png"> <img width="426" height="353" src="https://i.imgur.com/Ga6nRVS.png"> <img width="426" height="353" src="https://i.imgur.com/PxSt6aH.png"> <img width="426" height="353" src="https://i.imgur.com/batMjeo.png"> 

## <a name="sample-schematic"></a>Sample schematic
A sample schematic along with the code needed on the Arduino side can be found in the **ArduinoTwitchBot.Example** folder.  
It consists of a simple project that will light up one of four LEDs on the breadboard depending on the signal (String type).  
It requires the following in-app setup:
- Follower Alert - ON (value: "Follow", type: "String")
- Sub Alert - ON (value: "Sub", type: "String")
- Host Alert - ON (value: "Host", type: "String")

Of course, you can read and interpret the incomming signals however you want. This is just an example - feel free to experiment!

## <a name="feature-requests"></a>Feature requests
Have an idea that you would like to see in the app? Feel free to open an issue and I'll try my best to help :)

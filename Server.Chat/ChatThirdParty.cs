using IO.Ably;
using IO.Ably.Realtime;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Chat
{
    //This implementations is for [PRIVATE_MODE](Private Chat Mode) [PUBLIC_MODE](Public Chat Mode) is communicated over the client ably connection.
    public class ChatThirdParty
    {
        private string _apiKey = "_vRLkA.dtMWdw:vxlwHwwbRD6t_uP8Qu0b5ouI8xd63937moEWiuQhxSo";


        private AblyRealtime _ably;
        private bool _isConnected;

        public ChatThirdParty() { 
            _ably = new AblyRealtime(new ClientOptions{ Key = _apiKey });
            _ably.Connection.On(args =>
            {
                switch (args.Current)
                {
                    case ConnectionState.Connected:
                        Console.WriteLine("Ably connected");
                        break;

                    case ConnectionState.Initialized:
                        Console.WriteLine("Innitialised");

                        break;

                    case ConnectionState.Connecting:
                        Console.WriteLine("Connecting!");
                        break;

                    case ConnectionState.Disconnected:
                        Console.WriteLine("Connection Has been Disconnected!");
                        break;

                    case ConnectionState.Closing:
                        Console.WriteLine("Connection Has been Closing!");
                        break;

                    case ConnectionState.Closed:
                    case ConnectionState.Failed:
                    case ConnectionState.Suspended:
                        Console.WriteLine("Connection Has been Suspended!");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();

                }
                _isConnected = args.Current == ConnectionState.Connected;
            });
        }

        public void InnitializeAbly()
        {
            ConnectToAbly();
        }

        private void ConnectToAbly()
        {
            if (_isConnected)
            {
                _ably.Close();
            }
            else
            {
                _ably.Connect();
            }
        }

        private void SucribeToChatRoom(string channelOrChatRoomId, string eventName)
        {
            var channelName = channelOrChatRoomId;
            _ably.Channels.Get(channelName).Subscribe(eventName, message =>
            {
                Console.WriteLine($"Received message <b>{message.Data}</b> from channel <b>{channelName}</b>");
            });
            Console.WriteLine($"Successfully subscribed to channel <b>{channelName}</b>");
        }

        private async void PublishMessageToChatRoom(string messagePayload, string channelOrChatRoomId, string eventName)
        {
            var channelName = channelOrChatRoomId;
            var payload = messagePayload;
            // async-await makes sure call is executed in the background and then result is posted on UnitySynchronizationContext/Main thread
            var result = await _ably.Channels.Get(channelName).PublishAsync(eventName, payload);
            Console.WriteLine("Send Message");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.Core;
using Newtonsoft.Json;
using Server.Chat;
using Microsoft.Extensions.Configuration;
using Server.DataAccess;
using Server.DataAccess.Models;

namespace Server.Messages
{

    public class MessageProxy
    {
        public short messageID;
        public object messageBody;
    }

    public class MessageHandler : Singliton<MessageHandler>
    {
        public delegate Task HandleAsync(Client client, object messageBody, object id, CancellationToken cancellationToken);

        public MessageProxy messageProxy;
        public NetworkManager networkManager;
        private readonly Dictionary<short, HandleAsync> _handlers;
        private Chats chat;
        Log logger;

        public MessageHandler(NetworkManager manager)
        {
            Instance = this;
            networkManager = manager;
            messageProxy = new MessageProxy();
            _handlers = new Dictionary<short, HandleAsync>();
            RegisterMessages();
        }

       
        public void RegisterMessages(short messageID, HandleAsync handle)
        {
            _handlers.Add(messageID, handle);
        }

        private void RegisterMessages()
        {
            _handlers.Add(MessageEvents.LOGIN_MESSAGE, HandleLoginMessage);
            _handlers.Add(MessageEvents.CHAT_MESSAGE, HandleChatMessage);

        }

        public static object SerializeMessage(short messageID, object message)
        {
            //string body = JsonConvert.SerializeObject(new { messageID = messageID, messageBody = message });
            MessageProxy proxy = new MessageProxy
            {
                messageID = messageID,
                messageBody = message
            };
            return SerializationHelper.Serialize(proxy);
        }


        public async void HandleMessageAsync(Client client, Datagram datagram)
        {
            using (var source = new CancellationTokenSource())
            {
                var token = source.Token;
                MessageProxy? proxy = JsonConvert.DeserializeObject<MessageProxy>(datagram.body.ToString());
                Console.WriteLine(proxy.messageID + " Message ID" + "MESSAGE BODY " + datagram.body.ToString());

                if (_handlers.TryGetValue(proxy.messageID, out HandleAsync handle))
                {
                    try
                    {
                        Console.WriteLine(handle);
                        await handle(client, proxy.messageBody, datagram.clientCallabckId, token);
                    }
                    catch (Exception)
                    {
                        NetworkManager.PublishMessage(client, MessageEvents.SYSTEM_MESSAGE, new SystemMessage { message = "Failed Reding the data", messageType = SystemMessageType.Error}, datagram.clientCallabckId);
                    }
                }
                else
                {
                    Console.WriteLine("Message Handle dose noe exist");
                }
            }
        }


        private async Task HandleLoginMessage(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {

            LoginMessage loginMessage = SerializationHelper.Deserialize<LoginMessage>(messageBody.ToString());
            Console.WriteLine(loginMessage.userId + " " + loginMessage.playfabId + " " + id);

            NetworkManager.PublishMessage(client,MessageEvents.LOGIN_MESSAGE, loginMessage, id);

            await Task.FromResult<object>(null);
        }

        public async Task HandleChatMessage(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            try
            {
                ChatMessage chatMessage = SerializationHelper.Deserialize<ChatMessage>(messageBody.ToString());
                Console.WriteLine(chatMessage.channelID + " " + chatMessage.messageBody + " " + id);

                chat = new Chats(networkManager.FetchConnectionString(), logger);
                Console.WriteLine(Guid.NewGuid() + " WARIS");
                await chat.StoreChat(new ChatModel{ id = Guid.NewGuid(), senderid = Guid.NewGuid(), receiverid = Guid.NewGuid(), msg = "Hola", chatroomid = Guid.NewGuid() });
                NetworkManager.PublishMessage(client, MessageEvents.CHAT_MESSAGE, chatMessage, id);
                await Task.FromResult<object>(null);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Task.FromResult<object>(null);
            }
        }

    }
}

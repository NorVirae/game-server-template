using System;
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
            _handlers.Add(MessageEvents.FETCH_CHATS_MESSAGES, HandleFetchChatMessage);
            _handlers.Add(MessageEvents.FETCH_CHAT_ROOMS, HandlePrivateChatRoom);



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

                if (_handlers.TryGetValue(proxy.messageID, out HandleAsync handle))
                {
                    try
                    {
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
            User user = new User(networkManager.FetchConnectionString(), logger);

            UserModel userData = await  user.FetchUser(loginMessage.userId);
            if (userData == null)
            {
                Console.WriteLine(userData + " USERDATA " + loginMessage.playfabId + "  " + loginMessage.userId);
              int result = await user.StoreUser(new UserModel { id = Guid.NewGuid(), playfabid = loginMessage.playfabId, playfabuserid = loginMessage.userId});
                Console.WriteLine($"Is successful! {result}");
            }
            NetworkManager.PublishMessage(client,MessageEvents.LOGIN_MESSAGE, loginMessage, id);

            await Task.FromResult<object>(null);
        }

        public async Task HandleChatMessage(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            try
            {
                ChatMessage chatMessage = SerializationHelper.Deserialize<ChatMessage>(messageBody.ToString());

                chat = new Chats(networkManager.FetchConnectionString(), logger);
                await chat.StoreChat(new ChatModel{ id = chatMessage.messageBody.id, senderid = chatMessage.messageBody.senderid, receiverid = chatMessage.messageBody.receiverid, msg = chatMessage.messageBody.msg, chatroomid = chatMessage.messageBody.chatroomid });
                NetworkManager.PublishMessage(client, MessageEvents.CHAT_MESSAGE, chatMessage, id);
                await Task.FromResult<object>(null);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Task.FromResult<object>(null);
            }
        }

        public async Task HandlePrivateChatRoom(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            try
            {
                ChatRoomMessage chatRoomMessage = SerializationHelper.Deserialize<ChatRoomMessage>(messageBody.ToString());
                List<ChatModel> chatMessages = new List<ChatModel>();
                ChatRoom chatRoom = new ChatRoom(networkManager.FetchConnectionString(), logger);
                User user = new User(networkManager.FetchConnectionString(), logger);
                Chats chat = new Chats(networkManager.FetchConnectionString(), logger);

                UserModel userData = await user.FetchUser(chatRoomMessage.messageBody.creatorid);
                Console.WriteLine(chatRoomMessage.messageBody.creatorid.ToString() + " CREATORID");
                ChatRoomModel chatRoomData = await chatRoom.FetchChatRoom(chatRoomMessage.messageBody.id);
                if (chatRoomData != null)
                {
                    chatMessages = await chat.FetchChatHistory(chatRoomData.id);

                }

                Guid chatRoomId = Guid.NewGuid();

                if(chatRoomData == null)
                {
                    Console.WriteLine(userData + " UserData");

                   int result = await chatRoom.StoreChatRoom(new ChatRoomModel { id = chatRoomId, title = chatRoomMessage.messageBody.title, creatorid = userData.playfabuserid });
                    chatRoomData = await chatRoom.FetchChatRoom(chatRoomId);
                    Console.WriteLine(chatRoomData + " CHATROOMDATA  RESULT: " + result + "  ChatroomId " + chatRoomMessage.messageBody.id);
                }
                chatRoomMessage.channelID = chatRoomData.id.ToString();
                chatRoomMessage.chats = chatMessages;
                 
                Console.WriteLine(chatRoomMessage.channelID + " CHANNEL ID  CALLBACKID" + id);

                NetworkManager.PublishMessage(client, MessageEvents.CHAT_MESSAGE, chatRoomMessage, id);
                await Task.FromResult<object>(null);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Task.FromResult<object>(null);
            }
        }

        public async Task HandleFetchChatMessage(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            try
            {
                ChatMessage chatMessage = SerializationHelper.Deserialize<ChatMessage>(messageBody.ToString());

                chat = new Chats(networkManager.FetchConnectionString(), logger);
                List<ChatModel> chatMessages = await chat.FetchChatHistory(chatMessage.messageBody.chatroomid);
                NetworkManager.PublishMessage(client, MessageEvents.CHAT_MESSAGE, chatMessage, id);
                await Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Task.FromResult<object>(null);
            }
        }

        

    }
}

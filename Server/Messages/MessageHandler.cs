
using Server.Core;
using Newtonsoft.Json;
using Server.Chat;
using Server.DataAccess;
using Server.DataAccess.Models;
using Server.Messages.Handlers;

namespace Server.Messages
{

    public class MessageHandler : Singliton<MessageHandler>
    {
        public delegate Task HandleAsync(Client client, object messageBody, object id, CancellationToken cancellationToken);

        public MessageProxy messageProxy;
        private NetworkManager networkManager;


        private readonly Dictionary<short, HandleAsync> _handlers;

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
            _handlers.Add(MessageEvents.CHAT_MESSAGE, ChatNetworkMessageHandler.HandleSendChat);
            _handlers.Add(MessageEvents.FETCH_CHATS_MESSAGES, ChatNetworkMessageHandler.HandleFetchPrivateChatHistory);
            //_handlers.Add(MessageEvents.FETCH_CHAT_ROOMS, ChatNetworkMessageHandler.HandleFetchPrivateChatHistory);
            _handlers.Add(MessageEvents.PLAYFAB_ADD_FRIEND, PlafabHandler.handleAddFriendRequest);
            _handlers.Add(MessageEvents.PLAYFAB_ACCEPT_FRIEND, PlafabHandler.handleAcceptFriendRequest);
            _handlers.Add(MessageEvents.JOIN_PUBLIC_CHAT, ChatNetworkMessageHandler.HandleJoinOrCreateChatRoomRequest);

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
                    Console.WriteLine("Message Handle does not exist "+ proxy.messageID);
                }
            }
        }


        private async Task HandleLoginMessage(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {

            LoginMessage loginMessage = SerializationHelper.Deserialize<LoginMessage>(messageBody.ToString());

            Login.HandleUserLogin(loginMessage, client);
           
            NetworkManager.PublishMessage(client, MessageEvents.LOGIN_MESSAGE, loginMessage, id);

            await Task.FromResult<object>(null);
        }

        
    }
}

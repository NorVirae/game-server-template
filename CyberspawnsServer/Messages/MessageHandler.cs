using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CyberspawnsServer.Core;
using Newtonsoft.Json;

namespace CyberspawnsServer.Messages
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
            Console.WriteLine(loginMessage.userId + " " + loginMessage.playfabId + " " + id);

            NetworkManager.PublishMessage(client,MessageEvents.LOGIN_MESSAGE, loginMessage, id);

            await Task.FromResult<object>(null);
        }

    }
}

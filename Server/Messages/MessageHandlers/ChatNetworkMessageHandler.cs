using Server.Chat;
using Server.Core;
using Server.DataAccess;
using Server.DataAccess.Models;
using Server.DataAccess.Repositories;
using System.Threading;

namespace Server.Messages.Handlers
{
    public static class ChatNetworkMessageHandler
    {
        public static async Task HandleSendChat(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            if (GameManager.Instance.GetPlayerSeesion(client.userID, out PlayerSession session))
            {
                await session.chatsActionHandler.HandleSendChat(client, messageBody, id, cancellationToken);
            }
            else
                throw new NetworkException("Failed to get Session");
        }



        //public static async Task HandleFetchChatMessage(Client client, object messageBody, object id, CancellationToken cancellationToken)
        //{


            //if (GameManager.Instance.GetPlayerSeesion(client.userID, out PlayerSession session))
            //{
               // await session.chatsActionHandler.HandleFetchChatMessage(client, messageBody, id, cancellationToken);
            //}
           // else
             //   throw new NetworkException("Failed to get Session");
        //}

        public static async Task HandleFetchPrivateChatHistory(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            if (GameManager.Instance.GetPlayerSeesion(client.userID, out PlayerSession session))
            {
                await session.chatsActionHandler.HandleFetchPrivateChatHistory(client, messageBody, id, cancellationToken);
            }
            else
                throw new NetworkException("Failed to get Sesiion");
        }

        //handle Join ChatRoom simply adds a user name to the list of members in a chat to receiver chats
        public static async Task HandleJoinOrCreateChatRoomRequest(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            if (GameManager.Instance.GetPlayerSeesion(client.userID, out PlayerSession session))
            {
                await session.chatsActionHandler.HandleJoinOrCreateChatRoom(client, messageBody, id, cancellationToken);
            }
            else
                throw new NetworkException("Failed to get Session");
        }
            
    }
}

using Server.Chat;
using Server.Core;
using Server.DataAccess;
using Server.DataAccess.Models;
using Server.DataAccess.Repositories;
using Server.Logic.Handlers;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities;
using PlayFab;
using PlayFab.ClientModels;

namespace Server.Messages.Handlers
{
    
    public static class PlafabHandler
    {
        // this function handles friend request sent, request acceptance happens on the client\
        

        public static async Task handleAddFriendRequest(Client client, object messageBody, object id, CancellationToken cancellationToke)
        {
            try
            {
                if (GameManager.Instance.GetPlayerSeesion(client.userID, out PlayerSession session))
                {

                    FriendRequestMessage friendRequestMessage = SerializationHelper.Deserialize<FriendRequestMessage>(messageBody.ToString());
                    string[] key = { Constants.Friends };
                    Console.WriteLine("ADD FRIEND SESSION:SENDER " + session.playfabId + " RECIEVER " + friendRequestMessage.PlayfabId);
                    Task<PlayFabResult<PlayFab.AdminModels.GetUserDataResult>> taskReceiver = session.playfabPlayerHandler.GetUserData(friendRequestMessage.PlayfabId, key);
                    Task<PlayFabResult<PlayFab.AdminModels.GetUserDataResult>> taskSender = session.playfabPlayerHandler.GetUserData(session.playfabId, key);

                    //Get list of friend requests and add new friendRequest

                    PlayFabResult<PlayFab.AdminModels.GetUserDataResult> resultReciever = taskReceiver.Result;
                    PlayFabResult<PlayFab.AdminModels.GetUserDataResult> resultSender = taskSender.Result;

                    if (resultReciever != null
                        && resultReciever.Result != null
                        && resultSender.Result != null
                        && resultSender != null)
                    {
                        PlayFab.AdminModels.GetUserDataResult userDataReceiver = resultReciever.Result;
                        PlayFab.AdminModels.GetUserDataResult userDataSender = resultSender.Result;

                        Dictionary<string, FriendRequestMessage> friendRequestObjectReceiver = userDataReceiver.Data.ContainsKey(Constants.Friends) ?
                            JSONHelper.Deserialize<Dictionary<string, FriendRequestMessage>>(userDataReceiver.Data[Constants.Friends].Value) :
                            new Dictionary<string, FriendRequestMessage>();

                        Dictionary<string, FriendRequestMessage> friendRequestObjectSender = userDataSender.Data.ContainsKey(Constants.Friends) ?
                            JSONHelper.Deserialize<Dictionary<string, FriendRequestMessage>>(userDataSender.Data[Constants.Friends].Value) :
                            new Dictionary<string, FriendRequestMessage>();

                        if (!friendRequestObjectReceiver.ContainsKey(session.playfabId) && !friendRequestObjectSender.ContainsKey(friendRequestMessage.PlayfabId))
                        {
                            friendRequestObjectReceiver.Add(session.playfabId, new FriendRequestMessage { PlayfabId = session.playfabId, FriendName = "Ikemba", IsSender = false, Status = FriendStatus.Pending, FriendAvatarUrl = "image.com" });

                            //add receiver to sender object class
                            friendRequestObjectSender.Add(friendRequestMessage.PlayfabId, new FriendRequestMessage { PlayfabId = friendRequestMessage.PlayfabId, FriendName = "Aisha", IsSender = true, Status = FriendStatus.Pending, FriendAvatarUrl = "image.com" });

                            string friendRequestReceiverToString = JSONHelper.Serialize(friendRequestObjectReceiver);
                            string friendRequestSenderToString = JSONHelper.Serialize(friendRequestObjectSender);

                            session.playfabPlayerHandler.SetUserData(new Dictionary<string, string>() { { Constants.Friends, friendRequestReceiverToString } }, friendRequestMessage.PlayfabId);
                            session.playfabPlayerHandler.SetUserData(new Dictionary<string, string>() { { Constants.Friends, friendRequestSenderToString } }, session.playfabId);

                            Console.WriteLine("Add friend request was completed playfabId was Added!");
                        }
                        else
                        {
                            throw new NetworkException("Friend Request has already been sent!");
                        }
                        

                    }
                    else
                    {
                        throw new NetworkException("Unable to get user data");
                    }

                    NetworkManager.PublishMessage(client, MessageEvents.PLAYFAB_ADD_FRIEND, friendRequestMessage, id);
                    await Task.FromResult<object>(null);
                }
                else
                    throw new NetworkException("Failed to get Session");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Task.FromResult<object>(null);
                NetworkManager.PublishMessage(client,
                    MessageEvents.SYSTEM_MESSAGE,
                    new SystemMessage { message = ex.Message,
                        messageType = SystemMessageType.Error }, id);
            }
        }


        public static async Task handleAcceptFriendRequest(Client client, object messageBody, object id, CancellationToken cancellationToke)
        {

            try
            {
                if (GameManager.Instance.GetPlayerSeesion(client.userID, out PlayerSession session))
                {

                    FriendRequestMessage friendRequestMessage = SerializationHelper.Deserialize<FriendRequestMessage>(messageBody.ToString());
                    string[] key = { Constants.Friends };

                    //The receiver is the player whose session is being run - in this methods context
                    Task<PlayFabResult<PlayFab.AdminModels.GetUserDataResult>> taskReceiver = session.playfabPlayerHandler.GetUserData(session.playfabId, key);
                    Task<PlayFabResult<PlayFab.AdminModels.GetUserDataResult>> taskSender = session.playfabPlayerHandler.GetUserData(friendRequestMessage.PlayfabId, key);

                    //Get list of friend requests and add new friendRequest

                    PlayFabResult<PlayFab.AdminModels.GetUserDataResult> resultReciever = taskReceiver.Result;
                    PlayFabResult<PlayFab.AdminModels.GetUserDataResult> resultSender = taskSender.Result;


                    Console.WriteLine("OLE receiver" + friendRequestMessage.PlayfabId + " Ole Sender" + session.playfabId + "CLIENT ID " + client.userID );
                    //Console.WriteLine(JSONHelper.Serialize(new { rec = resultReciever, sen = resultSender, acc = acceptFriendResult }));
                    if (resultReciever != null
                        && resultReciever.Result != null
                        && resultSender.Result != null
                        && resultSender != null
                        )
                    {
                        PlayFab.AdminModels.GetUserDataResult userDataReceiver = resultReciever.Result;
                        PlayFab.AdminModels.GetUserDataResult userDataSender = resultSender.Result;

                        Dictionary<string, FriendRequestMessage> friendRequestObjectReceiver = userDataReceiver.Data.ContainsKey(Constants.Friends) ?
                            JSONHelper.Deserialize<Dictionary<string, FriendRequestMessage>>(userDataReceiver.Data[Constants.Friends].Value) :
                            new Dictionary<string, FriendRequestMessage>();

                        Dictionary<string, FriendRequestMessage> friendRequestObjectSender = userDataSender.Data.ContainsKey(Constants.Friends) ?
                            JSONHelper.Deserialize<Dictionary<string, FriendRequestMessage>>(userDataSender.Data[Constants.Friends].Value) :
                            new Dictionary<string, FriendRequestMessage>();
                        Console.WriteLine(JSONHelper.Serialize(friendRequestObjectSender) + " OWW Accept");

                        //check if playfabId is absent if yes add and set user data for both the friend request sender and friend request receiver
                        if (friendRequestObjectReceiver.ContainsKey(friendRequestMessage.PlayfabId) 
                            && friendRequestObjectSender.ContainsKey(session.playfabId))
                        {
                            friendRequestObjectReceiver[friendRequestMessage.PlayfabId].Status = FriendStatus.Accepted;
                            friendRequestObjectSender[session.playfabId].Status = FriendStatus.Accepted;

                            string friendRequestReceiverToString = JSONHelper.Serialize(friendRequestObjectReceiver);
                            string friendRequestSenderToString = JSONHelper.Serialize(friendRequestObjectSender);

                            session.playfabPlayerHandler.SetUserData(new Dictionary<string, string>() { { Constants.Friends, friendRequestReceiverToString } }, session.playfabId);
                            session.playfabPlayerHandler.SetUserData(new Dictionary<string, string>() { { Constants.Friends, friendRequestSenderToString } }, friendRequestMessage.PlayfabId);
                            Console.WriteLine("Accept friend request was completed playfabId was Added!");

                        }
                        else
                        {
                            throw new NetworkException("Friend Request was not found");

                        }

                    }
                    else
                    {
                        throw new NetworkException("Unable to get user data or Accept friend request");
                    }

                    NetworkManager.PublishMessage(client, MessageEvents.PLAYFAB_ADD_FRIEND, friendRequestMessage, id);
                    await Task.FromResult<object>(null);

                }
                else
                    throw new NetworkException("Failed to get Session");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Task.FromResult<object>(null);
                NetworkManager.PublishMessage(client, MessageEvents.SYSTEM_MESSAGE, new SystemMessage { message = ex.Message, messageType = SystemMessageType.Error }, id);
            }
        }
    }
}

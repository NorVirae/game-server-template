using Server.Chat;
using Server.Core;
using Server.DataAccess.Models;
using Server.DataAccess.Repositories;
using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Logic.Handlers
{
    public class ChatsActionHandler : BaseHandler<PlayerSession>
    {
        Log logger;
        UserRepository user;
        ChatRepository chat;
        AblyChatManager ablyChatManager;
        ChatRoomRepository chatRoom;
        ChatRoomMembersRepository chatRoomMembers;


        public ChatsActionHandler(PlayerSession playerSession) : base(playerSession){
            user = new UserRepository(NetworkManager.instance.dataService);
            chat = new ChatRepository(NetworkManager.instance.dataService, logger);
            ablyChatManager = new AblyChatManager();
            chatRoom = new ChatRoomRepository(NetworkManager.instance.dataService, logger);
            chatRoomMembers = new ChatRoomMembersRepository(NetworkManager.instance.dataService, logger);

        }

        public async Task HandleSendChat(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            try
            {
                ChatMessage chatMessage = SerializationHelper.Deserialize<ChatMessage>(messageBody.ToString());


                int sentResult = await chat.StoreChat(new ChatModel {
                    Id = Guid.NewGuid(),
                    SenderPlayfabId = chatMessage.SenderPlayfabId,
                    ReceiverPlayfabId = chatMessage.ReceiverPlayfabId,
                    Content = chatMessage.Content,
                    ChatRoomId = chatMessage.ChatRoomId,
                    MediaUrl = "https://quiva.image",
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                });

                if (sentResult > 0)
                {
                    Console.WriteLine("message has been sent");
                }
                //send message to ably chat specified specified channel: channel name is chatroom id
                ablyChatManager.ConnectToAbly(chatMessage.SenderPlayfabId);

                ablyChatManager.SucribeToChatRoom(chatMessage.ChatRoomId.ToString(), "chat:message");
                ablyChatManager.PublishMessageToChatRoom(chatMessage.Content, chatMessage.ChatRoomId.ToString(), "chat:message");

                Console.WriteLine(chatMessage.Content, " MSG");
                //close connection
                ablyChatManager.DisConnectFromAbly();

                NetworkManager.PublishMessage(client, MessageEvents.CHAT_MESSAGE, chatMessage, id);
                await Task.FromResult<object>(null);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Task.FromResult<object>(null);
                NetworkManager.PublishMessage(client, MessageEvents.CHAT_MESSAGE, new SystemMessage{ message = ex.Message, messageType = SystemMessageType.Error }, id);

            }
        }



        //public async Task HandleFetchChatMessage(Client client, object messageBody, object id, CancellationToken cancellationToken)
        //{
           // try
        //    {
                
        //        ChatMessage chatMessage = SerializationHelper.Deserialize<ChatMessage>(messageBody.ToString());

         ///       List<ChatModel> chatMessages = await chat.FetchChatHistory(chatMessage.ChatRoomId);
         //       NetworkManager.PublishMessage(client, MessageEvents.CHAT_MESSAGE, chatMessage, id);
          //      await Task.FromResult<object>(null);
                
           // }
           // catch (Exception ex)
           // {
             //   Console.WriteLine(ex.ToString());
         //       await Task.FromResult<object>(null);
         //   }
        //}


        public async Task HandleFetchPrivateChatHistory(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            try
            {

                ChatRoomMessage chatRoomMessage = SerializationHelper.Deserialize<ChatRoomMessage>(messageBody.ToString());
                List<ChatModel> chatMessages = new List<ChatModel>();

                //Task<List<ChatRoomMembersModel>> chatRoomData = chatRoomMembers.FetchChatRoomIdMembersWithPlayersPlayfabId(chatRoomMessage.SenderPlayfabId, chatRoomMessage.ReceiverPlayfabId);
                Task<List<ChatModel>> chatMessageTask = chat.FetchChatHistory(chatRoomMessage.Id);

                Console.WriteLine("COunt Data" + chatMessageTask.Result.Count);
                if (chatMessageTask.Result != null)
                {
                    Console.WriteLine("ChatRoom data " + JSONHelper.Serialize(chatMessageTask.Result));
                    chatMessages = chatMessageTask.Result;
                    Console.WriteLine("COunt " + chatMessages.Count);

                }
                else
                {
                    Console.WriteLine("Creating new room");
                    Guid chatRoomId = Guid.NewGuid();
                    chatRoomMessage.Id = chatRoomId;
                    Guid newMembersChatRoomIdSender = Guid.NewGuid();
                    Guid newMembersChatRoomIdReceiver = Guid.NewGuid();

                    int newSenderChatRoomMember = await chatRoomMembers.StoreChatRoomMembers(new ChatRoomMembersModel
                    {
                        Id = newMembersChatRoomIdSender,
                        ChatRoomId = chatRoomId,
                        PlayerPlayfabId = chatRoomMessage.SenderPlayfabId,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,

                    });
                    int newReceiverChatRoomMember = await chatRoomMembers.StoreChatRoomMembers(new ChatRoomMembersModel
                    {
                        Id = newMembersChatRoomIdReceiver,
                        ChatRoomId = chatRoomId,
                        PlayerPlayfabId = chatRoomMessage.ReceiverPlayfabId,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                    });

                    int result = await chatRoom.StoreChatRoom(new ChatRoomModel {
                        Id = chatRoomId,
                        Name = chatRoomMessage.Name,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                    });
                }

                chatRoomMessage.Chats = chatMessages;

                NetworkManager.PublishMessage(client, MessageEvents.FETCH_CHATS_MESSAGES, chatRoomMessage, id);
                await Task.FromResult<object>(null);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Task.FromResult<object>(null);
                NetworkManager.PublishMessage(client, MessageEvents.FETCH_CHATS_MESSAGES, new SystemMessage{ message = ex.Message, messageType = SystemMessageType.Error }, id);

            }
        }

        //this function works well for creating public group chats
        public async Task HandleJoinOrCreateChatRoom(Client client, object messageBody, object id, CancellationToken cancellationToken)
        {
            try
            {
                ChatRoomMessage chatRoomMessage = SerializationHelper.Deserialize<ChatRoomMessage>(messageBody.ToString());
                ChatRoomModel chatRoomData = await chatRoom.FetchChatRoom(chatRoomMessage.Id);

                if (chatRoomData != null) {
                    int successCount = await chatRoomMembers.StoreChatRoomMembers(new ChatRoomMembersModel {
                        Id = Guid.NewGuid(),
                        PlayerPlayfabId=chatRoomMessage.SenderPlayfabId,
                        ChatRoomId = chatRoomData.Id,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    });
                    Console.WriteLine("CHECK THIS");

                    if (successCount > 0)
                    {
                        Console.WriteLine("Joining was successful!");
                    }
                    NetworkManager.PublishMessage(client,
                    MessageEvents.JOIN_PUBLIC_CHAT,
                    new ChatRoomMessage { Id = chatRoomData.Id, SenderPlayfabId=chatRoomMessage.SenderPlayfabId }, id);
                }
                else
                {
                    Guid newChatRoomId = Guid.NewGuid();
                    int successCountRoom =  await chatRoom.StoreChatRoom(new ChatRoomModel { 
                        Id = chatRoomMessage.Id != null? chatRoomMessage.Id: newChatRoomId,
                        Name = chatRoomMessage.Name,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow  });


                    int successCount = successCountRoom>0?
                        await chatRoomMembers.StoreChatRoomMembers(new ChatRoomMembersModel
                        {
                            Id = Guid.NewGuid(),
                            PlayerPlayfabId = chatRoomMessage.SenderPlayfabId,
                            ChatRoomId = newChatRoomId
                        }): 0;

                    NetworkManager.PublishMessage(client,
                    MessageEvents.JOIN_PUBLIC_CHAT,
                    new ChatRoomMessage {
                        Id = chatRoomMessage.Id != null ? chatRoomMessage.Id : Guid.NewGuid(),
                        Name = chatRoomMessage.Name
                    },
                     id);
                    Console.WriteLine("Chatroom does not exist, New ChatRoom was created");
                }

                await Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Task.FromResult<object>(null);
                NetworkManager.PublishMessage(client, MessageEvents.JOIN_PUBLIC_CHAT, new SystemMessage { message = ex.Message, messageType = SystemMessageType.Error }, id);

            }
        }

        protected override Task LoadDataFromDB()
        {
            throw new NotImplementedException();
        }
    }
}

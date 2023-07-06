using Server.DataAccess.Models;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnServer.Migrations.Migrations
{
    [Migration(202306230002)]
    public class InitialSeedMigration_202306230002 : Migration
    {
            public override void Down()
            {
                Delete.FromTable("chats")
                    .Row(new
                    {
                        Id = new Guid("f992e46e-7b95-4574-8b31-e485c92e0cd9"),
                        ReceiverId = "#b0e54e7c",
                        Content = "How far!",
                        ChatRoomId = new Guid("67fbac34-1ee1-4697-b916-1748861dd275"),
                        MediaUrl = "https://pexels.com/wonder.png",
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow

                    });

                Delete.FromTable("chatroom")
                    .Row(new ChatRoomModel
                    {
                        Id = new Guid("67fbac34-1ee1-4697-b916-1748861dd275"),
                        Name = "Test Address",
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    });

                Delete.FromTable("chatroommembers")
                        .Row(new ChatRoomMembersModel
                        {
                            Id = new Guid("d46ccdec-bdf6-4184-bd75-c008ff486786"),
                            ChatRoomId = new Guid("67fbac34-1ee1-4697-b916-1748861dd275"),
                            PlayerPlayfabId = "94C161535FB3CF34",
                            UpdatedAt = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow
                        });
            }

            public override void Up()
            {

            Insert.IntoTable("chatroom")
                    .Row(new ChatRoomModel
                    {
                        Id = new Guid("67fbac34-1ee1-4697-b916-1748861dd275"),
                        Name = "Test Address",
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    });

            Insert.IntoTable("chatroommembers")
                    .Row(new ChatRoomMembersModel
                    {
                        Id = new Guid("d46ccdec-bdf6-4184-bd75-c008ff486786"),
                        ChatRoomId = new Guid("67fbac34-1ee1-4697-b916-1748861dd275"),
                        PlayerPlayfabId = "94C161535FB3CF34",
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    });

            Insert.IntoTable("chats")
                    .Row(new ChatModel
                    {
                        Id = new Guid("f992e46e-7b95-4574-8b31-e485c92e0cd9"),
                        SenderPlayfabId = "#59c0d403",
                        ReceiverPlayfabId = "#b0e54e7c",
                        Content = "How far!",
                        ChatRoomId = new Guid("67fbac34-1ee1-4697-b916-1748861dd275"),
                        MediaUrl = "https://pexels.com/wonder.png",
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow

                    });
                
        }
    }
}



using FluentMigrator;
using FluentMigrator.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnServer.Migrations.Migrations
{
    [Migration(202306230001 )]

    public class InitialTablesMigration_202306230001 : Migration
    {

            public override void Down()
            {
                Delete.Table("chats");
                Delete.Table("chatroom");
                Delete.Table("chatroommembers");

        }

        public override void Up()
            {
                Create.Table("chats")
                        .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                        .WithColumn("SenderPlayfabId").AsString(50).NotNullable()
                        .WithColumn("ReceiverPlayfabId").AsString(50).NotNullable()
                        .WithColumn("Content").AsString(50).NotNullable()
                        .WithColumn("ChatRoomId").AsGuid().NotNullable()
                        .WithColumn("MediaUrl").AsString(50).NotNullable()
                        .WithColumn("UpdatedAt").AsDateTime().NotNullable()
                        .WithColumn("CreatedAt").AsDateTime().NotNullable();

                Create.Table("chatroom")
                        .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                        .WithColumn("Name").AsString(50).NotNullable()
                        .WithColumn("UpdatedAt").AsDateTime().NotNullable()
                        .WithColumn("CreatedAt").AsDateTime().NotNullable();

                Create.Table("chatroommembers")
                        .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                        .WithColumn("ChatRoomId").AsGuid().NotNullable()
                        .WithColumn("PlayerPlayfabId").AsString(50).NotNullable()
                        .WithColumn("UpdatedAt").AsDateTime().NotNullable()
                        .WithColumn("CreatedAt").AsDateTime().NotNullable();
        }
    }
}

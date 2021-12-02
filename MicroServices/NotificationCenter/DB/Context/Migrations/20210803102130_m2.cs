using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VDB.MicroServices.NotificationCenter.DB.Context.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationAudiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Receiver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationAudiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationContexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationContexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationAudienceNotificationContext",
                columns: table => new
                {
                    NotificationAudiencesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationContextsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationAudienceNotificationContext", x => new { x.NotificationAudiencesId, x.NotificationContextsId });
                    table.ForeignKey(
                        name: "FK_NotificationAudienceNotificationContext_NotificationAudiences_NotificationAudiencesId",
                        column: x => x.NotificationAudiencesId,
                        principalTable: "NotificationAudiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationAudienceNotificationContext_NotificationContexts_NotificationContextsId",
                        column: x => x.NotificationContextsId,
                        principalTable: "NotificationContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationAudienceNotificationType",
                columns: table => new
                {
                    NotificationAudiencesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationAudienceNotificationType", x => new { x.NotificationAudiencesId, x.NotificationTypesId });
                    table.ForeignKey(
                        name: "FK_NotificationAudienceNotificationType_NotificationAudiences_NotificationAudiencesId",
                        column: x => x.NotificationAudiencesId,
                        principalTable: "NotificationAudiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationAudienceNotificationType_NotificationTypes_NotificationTypesId",
                        column: x => x.NotificationTypesId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationContextNotificationType",
                columns: table => new
                {
                    NotificationContextsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationContextNotificationType", x => new { x.NotificationContextsId, x.NotificationTypesId });
                    table.ForeignKey(
                        name: "FK_NotificationContextNotificationType_NotificationContexts_NotificationContextsId",
                        column: x => x.NotificationContextsId,
                        principalTable: "NotificationContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationContextNotificationType_NotificationTypes_NotificationTypesId",
                        column: x => x.NotificationTypesId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationAudienceNotificationContext_NotificationContextsId",
                table: "NotificationAudienceNotificationContext",
                column: "NotificationContextsId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationAudienceNotificationType_NotificationTypesId",
                table: "NotificationAudienceNotificationType",
                column: "NotificationTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationContextNotificationType_NotificationTypesId",
                table: "NotificationContextNotificationType",
                column: "NotificationTypesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationAudienceNotificationContext");

            migrationBuilder.DropTable(
                name: "NotificationAudienceNotificationType");

            migrationBuilder.DropTable(
                name: "NotificationContextNotificationType");

            migrationBuilder.DropTable(
                name: "NotificationAudiences");

            migrationBuilder.DropTable(
                name: "NotificationContexts");

            migrationBuilder.DropTable(
                name: "NotificationTypes");
        }
    }
}

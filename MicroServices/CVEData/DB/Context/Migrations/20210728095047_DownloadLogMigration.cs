using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VDB.MicroServices.CVEData.DB.Context.Migrations
{
    public partial class DownloadLogMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVENodes_CVE_CVEId",
                table: "CVENodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CVE",
                table: "CVE");

            migrationBuilder.RenameTable(
                name: "CVE",
                newName: "CVEs");

            migrationBuilder.RenameIndex(
                name: "IX_CVE_DeletedAt_CVEID",
                table: "CVEs",
                newName: "IX_CVEs_DeletedAt_CVEID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CVEs",
                table: "CVEs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CVEDownloadLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CVEDataTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDownloadBySearch = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVEDownloadLogs", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CVENodes_CVEs_CVEId",
                table: "CVENodes",
                column: "CVEId",
                principalTable: "CVEs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVENodes_CVEs_CVEId",
                table: "CVENodes");

            migrationBuilder.DropTable(
                name: "CVEDownloadLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CVEs",
                table: "CVEs");

            migrationBuilder.RenameTable(
                name: "CVEs",
                newName: "CVE");

            migrationBuilder.RenameIndex(
                name: "IX_CVEs_DeletedAt_CVEID",
                table: "CVE",
                newName: "IX_CVE_DeletedAt_CVEID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CVE",
                table: "CVE",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CVENodes_CVE_CVEId",
                table: "CVENodes",
                column: "CVEId",
                principalTable: "CVE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

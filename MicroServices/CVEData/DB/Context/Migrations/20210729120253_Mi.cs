using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VDB.MicroServices.CVEData.DB.Context.Migrations
{
    public partial class Mi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CVEs");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "CVEs");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CVENodes");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "CVENodes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CVEDownloadLogs");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "CVEDownloadLogs");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CPEs");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "CPEs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Vendors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedById",
                table: "Vendors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedById",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CVEs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedById",
                table: "CVEs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CVENodes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedById",
                table: "CVENodes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CVEDownloadLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedById",
                table: "CVEDownloadLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CPEs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedById",
                table: "CPEs",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VDB.MicroServices.CVEData.DB.Context.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CVEs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CVEID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVEs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CVENodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CVEId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Operator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentNodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVENodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVENodes_CVENodes_ParentNodeId",
                        column: x => x.ParentNodeId,
                        principalTable: "CVENodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CVENodes_CVEs_CVEId",
                        column: x => x.CVEId,
                        principalTable: "CVEs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CPEs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    URI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVulnerable = table.Column<bool>(type: "bit", nullable: false),
                    SpecificVersion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VersionStartIncluding = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VersionEndIncluding = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VersionStartExcluding = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VersionEndExcluding = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPEs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CPEs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CPECVENode",
                columns: table => new
                {
                    CPEsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CVENodesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPECVENode", x => new { x.CPEsId, x.CVENodesId });
                    table.ForeignKey(
                        name: "FK_CPECVENode_CPEs_CPEsId",
                        column: x => x.CPEsId,
                        principalTable: "CPEs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPECVENode_CVENodes_CVENodesId",
                        column: x => x.CVENodesId,
                        principalTable: "CVENodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CPECVENode_CVENodesId",
                table: "CPECVENode",
                column: "CVENodesId");

            migrationBuilder.CreateIndex(
                name: "IX_CPEs_DeletedAt_ProductId_SpecificVersion_VersionStartIncluding_VersionEndIncluding_VersionStartExcluding_VersionEndExcluding",
                table: "CPEs",
                columns: new[] { "DeletedAt", "ProductId", "SpecificVersion", "VersionStartIncluding", "VersionEndIncluding", "VersionStartExcluding", "VersionEndExcluding" });

            migrationBuilder.CreateIndex(
                name: "IX_CPEs_ProductId",
                table: "CPEs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CVENodes_CVEId",
                table: "CVENodes",
                column: "CVEId");

            migrationBuilder.CreateIndex(
                name: "IX_CVENodes_ParentNodeId",
                table: "CVENodes",
                column: "ParentNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CVEs_DeletedAt_CVEID",
                table: "CVEs",
                columns: new[] { "DeletedAt", "CVEID" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_DeletedAt_Name",
                table: "Products",
                columns: new[] { "DeletedAt", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorId",
                table: "Products",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_DeletedAt_Name",
                table: "Vendors",
                columns: new[] { "DeletedAt", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPECVENode");

            migrationBuilder.DropTable(
                name: "CPEs");

            migrationBuilder.DropTable(
                name: "CVENodes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "CVEs");

            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}

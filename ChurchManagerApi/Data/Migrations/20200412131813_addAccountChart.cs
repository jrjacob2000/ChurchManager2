using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManagerApi.Data.Migrations
{
    public partial class addAccountChart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountChart",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DateEntered = table.Column<DateTime>(nullable: false),
                    EnteredBy = table.Column<Guid>(nullable: false),
                    DateLastEdited = table.Column<DateTime>(nullable: true),
                    EditedBy = table.Column<Guid>(nullable: true),
                    ClientId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ShowInRegister = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountChart", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1804AE3F-A188-43A4-927E-1B6F756476BE",
                column: "ConcurrencyStamp",
                value: "ef11d218-8226-49b6-b3dc-4d8ba15c659c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "232EBE68-8783-4734-BAAE-6A1B1C7A4458",
                column: "ConcurrencyStamp",
                value: "e90a4053-3f64-46b4-aca7-5f5a75bdb8eb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28924E18-2381-4184-989C-71CFCFFF65B0",
                column: "ConcurrencyStamp",
                value: "d5999812-d1d3-46ea-a75f-3bd4efe3debd");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountChart");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1804AE3F-A188-43A4-927E-1B6F756476BE",
                column: "ConcurrencyStamp",
                value: "2a1a589e-0459-4960-928a-e98da152ff59");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "232EBE68-8783-4734-BAAE-6A1B1C7A4458",
                column: "ConcurrencyStamp",
                value: "a9a5ade9-9edf-4ed6-a610-365fd8803519");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28924E18-2381-4184-989C-71CFCFFF65B0",
                column: "ConcurrencyStamp",
                value: "e87902ba-e815-4904-aeda-d0d9963022e8");
        }
    }
}

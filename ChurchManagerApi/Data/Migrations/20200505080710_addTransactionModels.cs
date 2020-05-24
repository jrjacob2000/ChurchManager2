using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManagerApi.Data.Migrations
{
    public partial class addTransactionModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountChart",
                table: "AccountChart");

            migrationBuilder.RenameTable(
                name: "AccountChart",
                newName: "AccountCharts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountCharts",
                table: "AccountCharts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    Payee = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    AccountRegisterId = table.Column<Guid>(nullable: false),
                    DateLastEdited = table.Column<DateTime>(nullable: true),
                    Payment = table.Column<decimal>(nullable: true),
                    Deposit = table.Column<decimal>(nullable: true),
                    DateEntered = table.Column<DateTime>(nullable: false),
                    EnteredBy = table.Column<Guid>(nullable: false),
                    EditedBy = table.Column<Guid>(nullable: true),
                    OwnerGroupId = table.Column<Guid>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    FundId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionLines_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1804AE3F-A188-43A4-927E-1B6F756476BE",
                column: "ConcurrencyStamp",
                value: "4dd17a2b-fa0a-4441-adbd-8c4f45586144");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "232EBE68-8783-4734-BAAE-6A1B1C7A4458",
                column: "ConcurrencyStamp",
                value: "3e611f83-334b-42c2-9b1b-b32b47813f93");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28924E18-2381-4184-989C-71CFCFFF65B0",
                column: "ConcurrencyStamp",
                value: "974be51c-0a44-4ce1-892d-fbd0dafc04c8");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLines_TransactionId",
                table: "TransactionLines",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionLines");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountCharts",
                table: "AccountCharts");

            migrationBuilder.RenameTable(
                name: "AccountCharts",
                newName: "AccountChart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountChart",
                table: "AccountChart",
                column: "Id");

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
    }
}

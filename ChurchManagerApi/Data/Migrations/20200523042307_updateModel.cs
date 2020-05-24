using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManagerApi.Data.Migrations
{
    public partial class updateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1804AE3F-A188-43A4-927E-1B6F756476BE",
                column: "ConcurrencyStamp",
                value: "053f0675-4392-4d29-a2a9-856982cfadce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "232EBE68-8783-4734-BAAE-6A1B1C7A4458",
                column: "ConcurrencyStamp",
                value: "772f8b5f-1a40-4789-a6a9-1ca445240585");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28924E18-2381-4184-989C-71CFCFFF65B0",
                column: "ConcurrencyStamp",
                value: "aacab49d-a310-47fb-b513-756baf52f2a5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}

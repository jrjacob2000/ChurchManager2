using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManagerApi.Data.Migrations
{
    public partial class seedrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "IsDefaultRole", "Name", "NormalizedName" },
                values: new object[] { "232EBE68-8783-4734-BAAE-6A1B1C7A4458", "a9a5ade9-9edf-4ed6-a610-365fd8803519", null, false, "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "IsDefaultRole", "Name", "NormalizedName" },
                values: new object[] { "28924E18-2381-4184-989C-71CFCFFF65B0", "e87902ba-e815-4904-aeda-d0d9963022e8", null, false, "accountant", "ACCOUNTANT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "IsDefaultRole", "Name", "NormalizedName" },
                values: new object[] { "1804AE3F-A188-43A4-927E-1B6F756476BE", "2a1a589e-0459-4960-928a-e98da152ff59", null, false, "encoder", "ENCODER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1804AE3F-A188-43A4-927E-1B6F756476BE");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "232EBE68-8783-4734-BAAE-6A1B1C7A4458");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28924E18-2381-4184-989C-71CFCFFF65B0");
        }
    }
}

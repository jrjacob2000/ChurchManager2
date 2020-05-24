using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManagerApi.Data.Migrations
{
    public partial class renameOwnerGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerGroupId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerGroupId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

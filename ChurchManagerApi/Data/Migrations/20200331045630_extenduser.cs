using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManagerApi.Data.Migrations
{
    public partial class extenduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChangePasswordOnFirstLogin",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccountOwner",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerGroupId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangePasswordOnFirstLogin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsAccountOwner",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OwnerGroupId",
                table: "AspNetUsers");
        }
    }
}

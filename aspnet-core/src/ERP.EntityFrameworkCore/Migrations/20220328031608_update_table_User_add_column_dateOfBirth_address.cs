using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class update_table_User_add_column_dateOfBirth_address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AbpUsers",
                unicode: false,
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AbpUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AbpUsers");
        }
    }
}

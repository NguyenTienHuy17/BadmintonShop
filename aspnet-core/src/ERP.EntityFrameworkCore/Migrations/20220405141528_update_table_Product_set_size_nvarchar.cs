using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class update_table_Product_set_size_nvarchar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

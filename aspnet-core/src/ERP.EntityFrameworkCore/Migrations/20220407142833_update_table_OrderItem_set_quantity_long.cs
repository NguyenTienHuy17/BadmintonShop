using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class update_table_OrderItem_set_quantity_long : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Quantity",
                table: "OrderItems",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "OrderItems",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}

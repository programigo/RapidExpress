using Microsoft.EntityFrameworkCore.Migrations;

namespace RapidExpress.Data.Migrations
{
    public partial class GoodsValueColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoodsValue",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoodsValue",
                table: "Deliveries");
        }
    }
}

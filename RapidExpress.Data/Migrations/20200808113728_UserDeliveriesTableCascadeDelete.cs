using Microsoft.EntityFrameworkCore.Migrations;

namespace RapidExpress.Data.Migrations
{
    public partial class UserDeliveriesTableCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_AspNetUsers_UserId",
                table: "Deliveries");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_AspNetUsers_UserId",
                table: "Deliveries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_AspNetUsers_UserId",
                table: "Deliveries");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_AspNetUsers_UserId",
                table: "Deliveries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

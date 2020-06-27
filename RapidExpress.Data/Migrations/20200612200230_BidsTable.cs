using Microsoft.EntityFrameworkCore.Migrations;

namespace RapidExpress.Data.Migrations
{
    public partial class BidsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HeightFirstPart",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HeightSecondPart",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LengthFirstPart",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LengthSecondPart",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WidthFirstPart",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WidthSecondPart",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(nullable: false),
                    Currency = table.Column<int>(nullable: false),
                    DeliveryId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bids_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bids_DeliveryId",
                table: "Bids",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_UserId",
                table: "Bids",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropColumn(
                name: "HeightFirstPart",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "HeightSecondPart",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "LengthFirstPart",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "LengthSecondPart",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "WidthFirstPart",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "WidthSecondPart",
                table: "Deliveries");
        }
    }
}

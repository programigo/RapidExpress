using Microsoft.EntityFrameworkCore.Migrations;

namespace RapidExpress.Data.Migrations
{
    public partial class DeliveriesTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PickupLocation",
                table: "Deliveries",
                newName: "PickUpLocation");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryLocationZipCode",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryPropertyType",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryStreet",
                table: "Deliveries",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasInsurance",
                table: "Deliveries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PickUpLocationZipCode",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PickUpPropertyType",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PickUpStreet",
                table: "Deliveries",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientEmail",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientPhoneNumber",
                table: "Deliveries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderEmail",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderPhoneNumber",
                table: "Deliveries",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryLocationZipCode",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryPropertyType",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryStreet",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "HasInsurance",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PickUpLocationZipCode",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PickUpPropertyType",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PickUpStreet",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "RecipientEmail",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "RecipientPhoneNumber",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "SenderEmail",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "SenderPhoneNumber",
                table: "Deliveries");

            migrationBuilder.RenameColumn(
                name: "PickUpLocation",
                table: "Deliveries",
                newName: "PickupLocation");
        }
    }
}

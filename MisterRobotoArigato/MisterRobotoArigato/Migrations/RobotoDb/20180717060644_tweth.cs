using Microsoft.EntityFrameworkCore.Migrations;

namespace MisterRobotoArigato.Migrations.RobotoDb
{
    public partial class tweth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasketID",
                table: "BasketDetails");

            migrationBuilder.AddColumn<string>(
                name: "CustomerID",
                table: "Baskets",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "BasketDetails",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "CustomerID",
                table: "BasketDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "BasketDetails");

            migrationBuilder.AlterColumn<int>(
                name: "UnitPrice",
                table: "BasketDetails",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<int>(
                name: "BasketID",
                table: "BasketDetails",
                nullable: false,
                defaultValue: 0);
        }
    }
}

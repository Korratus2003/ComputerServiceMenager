using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceManager.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentStatusToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Services",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Services");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceManager.Migrations
{
    /// <inheritdoc />
    public partial class TechnicianImageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Technicians",
                type: "character varying(2083)",
                maxLength: 2083,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Technicians");
        }
    }
}

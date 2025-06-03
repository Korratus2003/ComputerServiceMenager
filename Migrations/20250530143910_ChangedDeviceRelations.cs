using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ComputerServiceManager.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDeviceRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Magazine_MagazineId",
                table: "Devices");

            migrationBuilder.DropTable(
                name: "Magazine");

            migrationBuilder.RenameColumn(
                name: "MagazineId",
                table: "Devices",
                newName: "SaleDeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_MagazineId",
                table: "Devices",
                newName: "IX_Devices_SaleDeviceId");

            migrationBuilder.CreateTable(
                name: "SaleDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DefaultPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDevices", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_SaleDevices_SaleDeviceId",
                table: "Devices",
                column: "SaleDeviceId",
                principalTable: "SaleDevices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_SaleDevices_SaleDeviceId",
                table: "Devices");

            migrationBuilder.DropTable(
                name: "SaleDevices");

            migrationBuilder.RenameColumn(
                name: "SaleDeviceId",
                table: "Devices",
                newName: "MagazineId");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_SaleDeviceId",
                table: "Devices",
                newName: "IX_Devices_MagazineId");

            migrationBuilder.CreateTable(
                name: "Magazine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DefaultPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magazine", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Magazine_MagazineId",
                table: "Devices",
                column: "MagazineId",
                principalTable: "Magazine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

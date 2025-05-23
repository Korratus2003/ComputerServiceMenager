using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceManager.Migrations
{
    /// <inheritdoc />
    public partial class FixedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Technicians_TechnicianId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TechnicianId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianId",
                table: "Users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TechnicianId",
                table: "Users",
                column: "TechnicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Technicians_TechnicianId",
                table: "Users",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Technicians_TechnicianId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TechnicianId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TechnicianId",
                table: "Users",
                column: "TechnicianId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Technicians_TechnicianId",
                table: "Users",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

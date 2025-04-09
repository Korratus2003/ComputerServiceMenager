using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ComputerServiceManager.Migrations
{

    public partial class InitialCreate : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    VisitsRegularly = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Clients", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "InventoryDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DefaultPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_InventoryDevices", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Technicians",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    EmploymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Technicians", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TechnicianId = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Technicians_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Technicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryDeviceId = table.Column<int>(type: "integer", nullable: false),
                    DeviceId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    SellDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_InventoryDevices_InventoryDeviceId",
                        column: x => x.InventoryDeviceId,
                        principalTable: "InventoryDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<int>(type: "integer", nullable: false),
                    TechnicianId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Technicians_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Technicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ClientId",
                table: "Devices",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_DeviceId",
                table: "Sales",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_InventoryDeviceId",
                table: "Sales",
                column: "InventoryDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_DeviceId",
                table: "Services",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_TechnicianId",
                table: "Services",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TechnicianId",
                table: "Users",
                column: "TechnicianId",
                unique: true);

            //dodawanie przykładowych danych
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name", "Surname", "PhoneNumber", "VisitsRegularly" },
                values: new object[,]
                {
                    { 1, "Jan", "Kowalski", "123456789", true },
                    { 2, "Anna", "Nowak", "987654321", false },
                    { 3, "Piotr", "Wiśniewski", "456123789", true },
                    { 4, "Kasia", "Zielińska", "789456123", false },
                    { 5, "Tomasz", "Majewski", "321987654", true },
                    { 6, "Ewa", "Nowakowska", "111222333", true },
                    { 7, "Marek", "Szymański", "999888777", false },
                    { 8, "Magda", "Jankowska", "333444555", true },
                    { 9, "Adam", "Krawczyk", "555666777", false },
                    { 10, "Aleksandra", "Dąbrowska", "123123123", true },
                    { 11, "Łukasz", "Pawlak", "321321321", true },
                    { 12, "Paweł", "Bąk", "111444555", false },
                    { 13, "Sylwia", "Wesołowska", "666555444", true },
                    { 14, "Daniel", "Lis", "222333444", false },
                    { 15, "Natalia", "Chmielewska", "777888999", true }
                });

            migrationBuilder.InsertData(
                table: "InventoryDevices",
                columns: new[] { "Id", "Name", "Type", "Description", "DefaultPrice" },
                values: new object[,]
                {
                    { 1, "Telefon", "Telefon", "Prosty smartfon do codziennego użytku", 1200 },
                    { 2, "Laptop", "Laptop", "Laptop do pracy biurowej", 3500 },
                    { 3, "Tablet", "Tablet", "Tablet z ekranem 10 cali", 1500 },
                    { 4, "Router", "Internet", "Router Wi-Fi", 500 },
                    { 5, "Monitor", "Monitor", "Monitor 24 cale", 900 },
                    { 6, "Klawiatura", "Akcesoria", "Klawiatura mechaniczna", 400 },
                    { 7, "Mysz", "Akcesoria", "Bezprzewodowa mysz", 300 },
                    { 8, "Słuchawki", "Akcesoria", "Słuchawki nauszne", 800 },
                    { 9, "Kamera", "Akcesoria", "Kamera internetowa", 600 },
                    { 10, "Powerbank", "Akcesoria", "Powerbank 10000mAh", 200 },
                    { 11, "Głośnik Bluetooth", "Audio", "Głośnik przenośny", 600 },
                    { 12, "Smartwatch", "Zegarek", "Smartwatch z GPS", 1200 },
                    { 13, "Projektor", "Multimedia", "Projektor HD", 2500 },
                    { 14, "Drukarka", "Urządzenie", "Drukarka laserowa", 1300 },
                    { 15, "Telewizor", "Multimedia", "Telewizor 4K", 4000 },
                    { 16, "Konsola", "Gry", "Konsola do gier", 2500 },
                    { 17, "Pendrive", "Akcesoria", "Pendrive 128GB", 150 },
                    { 18, "Dysk Zewnętrzny", "Akcesoria", "Dysk SSD 1TB", 500 },
                    { 19, "Mikrofon", "Audio", "Mikrofon USB", 700 },
                    { 20, "Drążek Selfie", "Akcesoria", "Rozkładany drążek", 100 }
                });

            migrationBuilder.InsertData(
                table: "Technicians",
                columns: new[] { "Id", "Name", "Surname", "PhoneNumber", "EmploymentDate" },
                values: new object[,]
                {
                    { 1, "Marek", "Nowak", "123123123", DateTime.UtcNow },
                    { 2, "Ewa", "Kowalska", "321321321", DateTime.UtcNow },
                    { 3, "Andrzej", "Wiśniewski", "987654321", DateTime.UtcNow }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "ClientId", "Name", "Type" },
                values: new object[,]
                {
                    { 1, 1, "Xiaomi Redmi Note 9", "Telefon" },
                    { 2, 2, "Dell XPS 13", "Laptop" },
                    { 3, 3, "iPad Pro", "Tablet" },
                    { 4, 4, "Samsung Galaxy S21", "Telefon" },
                    { 5, 5, "Lenovo ThinkPad", "Laptop" },
                    { 6, 6, "Huawei P30", "Telefon" },
                    { 7, 7, "MacBook Pro", "Laptop" },
                    { 8, 8, "Sony Xperia 5", "Telefon" },
                    { 9, 9, "Asus ROG", "Laptop" },
                    { 10, 10, "Google Pixel 6", "Telefon" },
                    { 11, 11, "HP EliteBook", "Laptop" },
                    { 12, 12, "OnePlus 9", "Telefon" },
                    { 13, 13, "Acer Aspire", "Laptop" },
                    { 14, 14, "Moto G Power", "Telefon" },
                    { 15, 15, "Microsoft Surface", "Laptop" },
                    { 16, 1, "Xiaomi Mi Band", "Smartwatch" },
                    { 17, 2, "Samsung Galaxy Tab", "Tablet" },
                    { 18, 3, "Canon EOS", "Kamera" },
                    { 19, 4, "LG Smart TV", "Telewizor" },
                    { 20, 5, "JBL Charge", "Głośnik Bluetooth" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "TechnicianId", "Role", "Password", "Login" },
                values: new object[,]
                {
                    { 1, 1, "Admin", "1234", "admin" },
                    { 2, 2, "Technician", "hashedpassword2", "tech2" }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "InventoryDeviceId", "DeviceId", "Price", "SellDate" },
                values: new object[,]
                {
                    { 1, 1, 1, 1200m, DateTime.UtcNow },
                    { 2, 2, 2, 3500m, DateTime.UtcNow },
                    { 3, 3, 3, 1500m, DateTime.UtcNow },
                    { 4, 4, 4, 500m, DateTime.UtcNow },
                    { 5, 5, 5, 900m, DateTime.UtcNow },
                    { 6, 6, 6, 400m, DateTime.UtcNow },
                    { 7, 7, 7, 300m, DateTime.UtcNow },
                    { 8, 8, 8, 800m, DateTime.UtcNow },
                    { 9, 9, 9, 600m, DateTime.UtcNow },
                    { 10, 10, 10, 200m, DateTime.UtcNow },
                    { 11, 11, 11, 600m, DateTime.UtcNow },
                    { 12, 12, 12, 1200m, DateTime.UtcNow },
                    { 13, 13, 13, 2500m, DateTime.UtcNow },
                    { 14, 14, 14, 1300m, DateTime.UtcNow },
                    { 15, 15, 15, 4000m, DateTime.UtcNow },
                    { 16, 16, 16, 2500m, DateTime.UtcNow },
                    { 17, 17, 17, 150m, DateTime.UtcNow },
                    { 18, 18, 18, 500m, DateTime.UtcNow },
                    { 19, 19, 19, 700m, DateTime.UtcNow },
                    { 20, 20, 20, 100m, DateTime.UtcNow }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "DeviceId", "TechnicianId", "Description", "Price", "Status", "ServiceDate" },
                values: new object[,]
                {
                    { 1, 1, 1, "Naprawa telefonu", 150m, "Zakończono", DateTime.UtcNow },
                    { 2, 2, 1, "Wymiana ekranu", 350m, "Zakończono", DateTime.UtcNow },
                    { 3, 3, 2, "Aktualizacja oprogramowania", 50m, "W toku", DateTime.UtcNow },
                    { 4, 4, 2, "Czyszczenie kamery", 100m, "Oczekuje", DateTime.UtcNow },
                    { 5, 5, 3, "Naprawa klawiatury", 120m, "Zakończono", DateTime.UtcNow },
                    { 6, 6, 3, "Konserwacja laptopa", 200m, "Zakończono", DateTime.UtcNow },
                    { 7, 7, 1, "Wymiana modułu Wi-Fi", 150m, "Oczekuje", DateTime.UtcNow },
                    { 8, 8, 1, "Kalibracja ekranu", 80m, "Zakończono", DateTime.UtcNow },
                    { 9, 9, 2, "Czyszczenie obiektywu", 40m, "Zakończono", DateTime.UtcNow },
                    { 10, 10, 3, "Naprawa mikrofonu", 90m, "W toku", DateTime.UtcNow },
                    { 11, 11, 1, "Diagnostyka komputera", 100m, "Zakończono", DateTime.UtcNow },
                    { 12, 12, 2, "Naprawa baterii", 60m, "Zakończono", DateTime.UtcNow },
                    { 13, 13, 3, "Instalacja sterowników", 80m, "Oczekuje", DateTime.UtcNow },
                    { 14, 14, 1, "Wymiana portu USB", 120m, "Zakończono", DateTime.UtcNow },
                    { 15, 15, 2, "Konfiguracja urządzenia", 90m, "Zakończono", DateTime.UtcNow },
                    { 16, 16, 3, "Naprawa karty graficznej", 200m, "Oczekuje", DateTime.UtcNow },
                    { 17, 17, 1, "Czyszczenie drukarki", 50m, "Zakończono", DateTime.UtcNow },
                    { 18, 18, 2, "Naprawa systemu operacyjnego", 100m, "W toku", DateTime.UtcNow },
                    { 19, 19, 3, "Wymiana głośnika", 75m, "Zakończono", DateTime.UtcNow },
                    { 20, 20, 1, "Naprawa telewizora", 400m, "Oczekuje", DateTime.UtcNow }
                });
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "InventoryDevices");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Technicians");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}

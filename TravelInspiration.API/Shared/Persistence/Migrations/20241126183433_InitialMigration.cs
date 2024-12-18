using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelInspiration.API.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItineraryId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stops_Itineraries_ItineraryId",
                        column: x => x.ItineraryId,
                        principalTable: "Itineraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Itineraries",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Description", "LastModifiedAt", "LastModifiedBy", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "DATASEED", new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified), "A wonderful journey through Europe", null, null, "European Adventure", "user1" },
                    { 2, "DATASEED", new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified), "Exploring the wonders of Asia", null, null, "Asia Exploration", "user2" }
                });

            migrationBuilder.InsertData(
                table: "Stops",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ImageUri", "ItineraryId", "LastModifiedAt", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, "DATASEED", new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified), "https://localhost:7120/paris.jpg", 1, null, null, "Paris" },
                    { 2, "DATASEED", new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified), "https://localhost:7120/rome.jpg", 1, null, null, "Rome" },
                    { 3, "DATASEED", new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified), "https://localhost:7120/bangkok.jpg", 2, null, null, "Bangkok" },
                    { 4, "DATASEED", new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified), "https://localhost:7120/tokyo.jpg", 2, null, null, "Tokyo" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stops_ItineraryId",
                table: "Stops",
                column: "ItineraryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "Itineraries");
        }
    }
}

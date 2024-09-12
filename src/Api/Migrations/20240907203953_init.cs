using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("a6d860c0-c3dd-4b3c-942a-fcc2ef040c33"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "deleted_at", "email", "first_name", "last_name", "status", "updated_at" },
                values: new object[] { new Guid("8f89cff9-8c7f-493f-af8c-a2fd68980ca9"), new DateTime(2021, 8, 30, 20, 30, 0, 0, DateTimeKind.Utc), null, "admin@admin.com", "Görkem Rıdvan", "ARIK", true, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("8f89cff9-8c7f-493f-af8c-a2fd68980ca9"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "deleted_at", "email", "first_name", "last_name", "status", "updated_at" },
                values: new object[] { new Guid("a6d860c0-c3dd-4b3c-942a-fcc2ef040c33"), new DateTime(2021, 8, 30, 20, 30, 0, 0, DateTimeKind.Utc), null, "admin@admin.com", "Görkem Rıdvan", "ARIK", true, null });
        }
    }
}

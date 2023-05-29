using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutApp.Migrations
{
    /// <inheritdoc />
    public partial class last3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "UserName" },
                values: new object[] { 1, "test@gmail.com", "$2a$11$4PF1nJ6x31nDyJVIb7okeOBtJAprv984a0w6RZx65sN.N4IttNUh2", "test" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynapredictAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 23, 1, 21, 9, 250, DateTimeKind.Utc).AddTicks(4983), "$2a$11$8FwFu.c3mBM7wySr7cQwBOIrKvJe5H3FrYgu1BujlTLgIJK4H5eh6" });
        }
    }
}

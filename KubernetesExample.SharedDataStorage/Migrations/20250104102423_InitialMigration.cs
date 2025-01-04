using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KubernetesExample.SharedDataStorage.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "std");

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "std",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "std",
                table: "Students",
                columns: new[] { "Id", "Age", "Name" },
                values: new object[] { new Guid("b8881257-533d-4cee-a037-bfd1cedb5fa4"), 18, "Perico" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students",
                schema: "std");
        }
    }
}

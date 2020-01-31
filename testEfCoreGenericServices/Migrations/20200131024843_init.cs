using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace testEfCoreGenericServices.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("cf70c77c-966f-4bda-b0f0-482a5d1062a1"), "abc" },
                    { new Guid("32feb0dd-6182-4dc0-9ad6-b147b6fb0cbb"), "cde" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user");
        }
    }
}

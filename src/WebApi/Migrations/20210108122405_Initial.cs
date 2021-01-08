using Microsoft.EntityFrameworkCore.Migrations;

namespace EfProblemSample.WebApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entity1s",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AUniqueProperty = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SomeProperty = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity1s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entity2s",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SomeOtherProperty = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity2s", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entity1s_AUniqueProperty",
                table: "Entity1s",
                column: "AUniqueProperty",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entity1s");

            migrationBuilder.DropTable(
                name: "Entity2s");
        }
    }
}

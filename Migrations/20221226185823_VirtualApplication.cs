using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace somiod.Migrations
{
    public partial class VirtualApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Applications_parentid",
                table: "Modules");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Modules",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_name",
                table: "Modules",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Applications_parentid",
                table: "Modules",
                column: "parentid",
                principalTable: "Applications",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Applications_parentid",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_name",
                table: "Modules");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Modules",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Applications_parentid",
                table: "Modules",
                column: "parentid",
                principalTable: "Applications",
                principalColumn: "id");
        }
    }
}

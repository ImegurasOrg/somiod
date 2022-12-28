using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace somiod.Migrations
{
    public partial class testVirtualAccessTipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Data_Modules_parentid",
                table: "Data");

            migrationBuilder.AddForeignKey(
                name: "FK_Data_Modules_parentid",
                table: "Data",
                column: "parentid",
                principalTable: "Modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Data_Modules_parentid",
                table: "Data");

            migrationBuilder.AddForeignKey(
                name: "FK_Data_Modules_parentid",
                table: "Data",
                column: "parentid",
                principalTable: "Modules",
                principalColumn: "id");
        }
    }
}

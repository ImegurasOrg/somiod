using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace somiod.Migrations
{
    public partial class AcurateApplicationRouting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Aplications_ParentId",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aplications",
                table: "Aplications");

            migrationBuilder.RenameTable(
                name: "Aplications",
                newName: "Applications");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Modules",
                newName: "parentid");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Modules",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Creation_DT",
                table: "Modules",
                newName: "creation_dt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Modules",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_ParentId",
                table: "Modules",
                newName: "IX_Modules_parentid");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Applications",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Creation_DT",
                table: "Applications",
                newName: "creation_dt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Applications",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "Aplications");

            migrationBuilder.RenameColumn(
                name: "parentid",
                table: "Modules",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Modules",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "creation_dt",
                table: "Modules",
                newName: "Creation_DT");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Modules",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_parentid",
                table: "Modules",
                newName: "IX_Modules_ParentId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Aplications",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "creation_dt",
                table: "Aplications",
                newName: "Creation_DT");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Aplications",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aplications",
                table: "Aplications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Aplications_ParentId",
                table: "Modules",
                column: "ParentId",
                principalTable: "Aplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

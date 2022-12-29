using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace somiod.Migrations
{
    public partial class dtoImplementations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Data_Modules_parentid",
                table: "Data");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Applications_parentid",
                table: "Modules");

            migrationBuilder.AlterColumn<int>(
                name: "parentid",
                table: "Modules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creation_dt",
                table: "Modules",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "parentid",
                table: "Data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Data_Modules_parentid",
                table: "Data",
                column: "parentid",
                principalTable: "Modules",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Applications_parentid",
                table: "Modules",
                column: "parentid",
                principalTable: "Applications",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Data_Modules_parentid",
                table: "Data");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Applications_parentid",
                table: "Modules");

            migrationBuilder.AlterColumn<int>(
                name: "parentid",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "creation_dt",
                table: "Modules",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<int>(
                name: "parentid",
                table: "Data",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Data_Modules_parentid",
                table: "Data",
                column: "parentid",
                principalTable: "Modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Applications_parentid",
                table: "Modules",
                column: "parentid",
                principalTable: "Applications",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

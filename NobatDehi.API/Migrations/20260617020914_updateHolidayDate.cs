using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NobatDehi.API.Migrations
{
    /// <inheritdoc />
    public partial class updateHolidayDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId1",
                table: "PlanDependencies",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Holidays",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDependencies_PlanId1",
                table: "PlanDependencies",
                column: "PlanId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanDependencies_Plans_PlanId1",
                table: "PlanDependencies",
                column: "PlanId1",
                principalTable: "Plans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanDependencies_Plans_PlanId1",
                table: "PlanDependencies");

            migrationBuilder.DropIndex(
                name: "IX_PlanDependencies_PlanId1",
                table: "PlanDependencies");

            migrationBuilder.DropColumn(
                name: "PlanId1",
                table: "PlanDependencies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Holidays",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NobatDehi.API.Migrations
{
    /// <inheritdoc />
    public partial class FixPlanDependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId1",
                table: "PlanDependencies",
                type: "int",
                nullable: true);

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
    }
}

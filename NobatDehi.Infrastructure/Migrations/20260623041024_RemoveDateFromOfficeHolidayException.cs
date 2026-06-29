using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NobatDehi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDateFromOfficeHolidayException : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "OfficeHolidayExceptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "OfficeHolidayExceptions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}

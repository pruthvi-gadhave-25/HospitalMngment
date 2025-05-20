using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class AvaliabiltySlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "AvailabilitySlots");

            migrationBuilder.DropColumn(
                name: "TimeRange",
                table: "AvailabilitySlots");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "DayofWeek",
                table: "AvailabilitySlots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "AvailabilitySlots",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "AvailabilitySlots",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilitySlots_DoctorId",
                table: "AvailabilitySlots",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilitySlots_Doctors_DoctorId",
                table: "AvailabilitySlots",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilitySlots_Doctors_DoctorId",
                table: "AvailabilitySlots");

            migrationBuilder.DropIndex(
                name: "IX_AvailabilitySlots_DoctorId",
                table: "AvailabilitySlots");

            migrationBuilder.DropColumn(
                name: "DayofWeek",
                table: "AvailabilitySlots");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "AvailabilitySlots");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "AvailabilitySlots");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "AvailabilitySlots",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeRange",
                table: "AvailabilitySlots",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatientService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seeddataadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppointmentSlots",
                columns: new[] { "Id", "CreatedAt", "DoctorId", "EndTime", "SlotDate", "StartTime", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, new TimeOnly(9, 30, 0), new DateOnly(2025, 1, 15), new TimeOnly(9, 0, 0), 0 },
                    { 2, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, new TimeOnly(10, 0, 0), new DateOnly(2025, 1, 15), new TimeOnly(9, 30, 0), 1 },
                    { 3, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, new TimeOnly(10, 30, 0), new DateOnly(2025, 1, 15), new TimeOnly(10, 0, 0), 0 },
                    { 4, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, new TimeOnly(11, 0, 0), new DateOnly(2025, 1, 15), new TimeOnly(10, 30, 0), 0 },
                    { 5, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, new TimeOnly(9, 30, 0), new DateOnly(2025, 1, 15), new TimeOnly(9, 0, 0), 1 },
                    { 6, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, new TimeOnly(10, 0, 0), new DateOnly(2025, 1, 15), new TimeOnly(9, 30, 0), 0 },
                    { 7, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, new TimeOnly(10, 30, 0), new DateOnly(2025, 1, 15), new TimeOnly(10, 0, 0), 0 },
                    { 8, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, new TimeOnly(14, 30, 0), new DateOnly(2025, 1, 15), new TimeOnly(14, 0, 0), 1 },
                    { 9, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, new TimeOnly(15, 0, 0), new DateOnly(2025, 1, 15), new TimeOnly(14, 30, 0), 0 },
                    { 10, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, new TimeOnly(15, 30, 0), new DateOnly(2025, 1, 15), new TimeOnly(15, 0, 0), 0 },
                    { 11, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 4, new TimeOnly(11, 30, 0), new DateOnly(2025, 1, 15), new TimeOnly(11, 0, 0), 0 },
                    { 12, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 4, new TimeOnly(12, 0, 0), new DateOnly(2025, 1, 15), new TimeOnly(11, 30, 0), 0 },
                    { 13, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 5, new TimeOnly(16, 30, 0), new DateOnly(2025, 1, 15), new TimeOnly(16, 0, 0), 1 },
                    { 14, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 5, new TimeOnly(17, 0, 0), new DateOnly(2025, 1, 15), new TimeOnly(16, 30, 0), 0 },
                    { 15, new DateTime(2024, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), 5, new TimeOnly(17, 30, 0), new DateOnly(2025, 1, 15), new TimeOnly(17, 0, 0), 0 }
                });

            migrationBuilder.InsertData(
                table: "Patient",
                columns: new[] { "Id", "BirthDate", "CreatedAt", "FirstName", "Gender", "IdentityNumber", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateOnly(1990, 5, 15), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Ahmet", "Erkek", "12345678901", "Yılmaz", 1 },
                    { 2, new DateOnly(1985, 8, 22), new DateTime(2024, 1, 2, 11, 0, 0, 0, DateTimeKind.Utc), "Ayşe", "Kadın", "12345678902", "Kaya", 2 },
                    { 3, new DateOnly(1978, 3, 10), new DateTime(2024, 1, 3, 9, 0, 0, 0, DateTimeKind.Utc), "Mehmet", "Erkek", "12345678903", "Demir", 3 },
                    { 4, new DateOnly(1995, 12, 1), new DateTime(2024, 1, 4, 14, 0, 0, 0, DateTimeKind.Utc), "Fatma", "Kadın", "12345678904", "Çelik", 4 },
                    { 5, new DateOnly(2000, 7, 25), new DateTime(2024, 1, 5, 16, 0, 0, 0, DateTimeKind.Utc), "Ali", "Erkek", "12345678905", "Öztürk", 5 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CreatedAt", "DoctorId", "PatientId", "SlotId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 10, 10, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1 },
                    { 2, new DateTime(2024, 12, 11, 11, 0, 0, 0, DateTimeKind.Utc), 2, 2, 5, 1 },
                    { 3, new DateTime(2024, 12, 12, 14, 0, 0, 0, DateTimeKind.Utc), 3, 3, 8, 0 },
                    { 4, new DateTime(2024, 12, 13, 16, 0, 0, 0, DateTimeKind.Utc), 5, 4, 13, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AppointmentSlots",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}

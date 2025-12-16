using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatientService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class doctordoctorTitleadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorTitles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaitlistEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    PreferredDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PreferredStartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    PreferredEndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    QueuePosition = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    NotifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NotifiedSlotId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitlistEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaitlistEntries_AppointmentSlots_NotifiedSlotId",
                        column: x => x.NotifiedSlotId,
                        principalTable: "AppointmentSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WaitlistEntries_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DoctorTitleId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_DoctorTitles_DoctorTitleId",
                        column: x => x.DoctorTitleId,
                        principalTable: "DoctorTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "DoctorTitles",
                columns: new[] { "Id", "TitleName" },
                values: new object[,]
                {
                    { 1, "Ortopedi" },
                    { 2, "Dahiliye" },
                    { 3, "Kulak Burun Boğaz" },
                    { 4, "Noroloji" },
                    { 5, "Diş" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "DoctorTitleId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, 1, "Kadir", "Turan" },
                    { 2, 2, "Haluk", "Özdemir" },
                    { 3, 3, "Neslihan", "Doğan" },
                    { 4, 4, "Emrullah", "Yıldırım" },
                    { 5, 5, "Mert", "Gökçe" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DoctorTitleId",
                table: "Doctors",
                column: "DoctorTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitlistEntries_DoctorId_PreferredDate_QueuePosition",
                table: "WaitlistEntries",
                columns: new[] { "DoctorId", "PreferredDate", "QueuePosition" });

            migrationBuilder.CreateIndex(
                name: "IX_WaitlistEntries_NotifiedSlotId",
                table: "WaitlistEntries",
                column: "NotifiedSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitlistEntries_PatientId_DoctorId_PreferredDate_Status",
                table: "WaitlistEntries",
                columns: new[] { "PatientId", "DoctorId", "PreferredDate", "Status" });

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSlots_Doctors_DoctorId",
                table: "AppointmentSlots",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSlots_Doctors_DoctorId",
                table: "AppointmentSlots");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "WaitlistEntries");

            migrationBuilder.DropTable(
                name: "DoctorTitles");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Api.infrastructure.migrations
{
    /// <inheritdoc />
    public partial class _0002_DatabaseSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    SensorType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegisteredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelemetryReadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetricType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MetricValue = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    EventTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IngestedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelemetryReadings_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ExternalId",
                table: "Devices",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryReadings_DeviceId",
                table: "TelemetryReadings",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelemetryReadings");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.Api.Infrastructure.Migrations;

public partial class _0001_Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Devices",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                ExternalId = table.Column<string>(maxLength: 120, nullable: false),
                Name = table.Column<string>(maxLength: 120, nullable: false),
                SensorType = table.Column<string>(maxLength: 50, nullable: false),
                RegisteredAtUtc = table.Column<DateTime>(nullable: false),
                IsEnabled = table.Column<bool>(nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Devices", x => x.Id); });

        migrationBuilder.CreateIndex(
            name: "IX_Devices_ExternalId",
            table: "Devices",
            column: "ExternalId",
            unique: true);

        migrationBuilder.CreateTable(
            name: "TelemetryReadings",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                DeviceId = table.Column<Guid>(nullable: false),
                MetricType = table.Column<string>(maxLength: 50, nullable: false),
                MetricValue = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                EventTimeUtc = table.Column<DateTime>(nullable: false),
                IngestedAtUtc = table.Column<DateTime>(nullable: false)
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
            name: "IX_TelemetryReadings_DeviceId_EventTimeUtc",
            table: "TelemetryReadings",
            columns: new[] { "DeviceId", "EventTimeUtc" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "TelemetryReadings");
        migrationBuilder.DropTable(name: "Devices");
    }
}

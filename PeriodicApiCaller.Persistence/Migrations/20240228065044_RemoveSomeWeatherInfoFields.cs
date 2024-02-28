using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeriodicApiCaller.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSomeWeatherInfoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Precipitation",
                table: "WeatherInfo");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "WeatherInfo");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "WeatherInfo");

            migrationBuilder.RenameColumn(
                name: "WindSpeed",
                table: "WeatherInfo",
                newName: "TemperatureC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TemperatureC",
                table: "WeatherInfo",
                newName: "WindSpeed");

            migrationBuilder.AddColumn<decimal>(
                name: "Precipitation",
                table: "WeatherInfo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "WeatherInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Temperature",
                table: "WeatherInfo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

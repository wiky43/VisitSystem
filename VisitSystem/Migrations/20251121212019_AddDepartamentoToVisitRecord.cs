using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartamentoToVisitRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Departamento",
                table: "VisitRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "VisitRecords");
        }
    }
}

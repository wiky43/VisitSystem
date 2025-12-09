using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitSystem.Migrations
{
    /// <inheritdoc />
    public partial class logicaDelshow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaHora",
                table: "VisitRecords",
                newName: "HoraEntrada");

            migrationBuilder.AddColumn<DateTime>(
                name: "HoraSalida",
                table: "VisitRecords",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraSalida",
                table: "VisitRecords");

            migrationBuilder.RenameColumn(
                name: "HoraEntrada",
                table: "VisitRecords",
                newName: "FechaHora");
        }
    }
}

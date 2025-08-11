using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class create_abonnemets_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AbonnementId",
                table: "abonnementPaiments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AbonnementId1",
                table: "abonnementPaiments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Abonnements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationValue = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_abonnementPaiments_AbonnementId1",
                table: "abonnementPaiments",
                column: "AbonnementId1");

            migrationBuilder.AddForeignKey(
                name: "FK_abonnementPaiments_Abonnements_AbonnementId1",
                table: "abonnementPaiments",
                column: "AbonnementId1",
                principalTable: "Abonnements",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_abonnementPaiments_Abonnements_AbonnementId1",
                table: "abonnementPaiments");

            migrationBuilder.DropTable(
                name: "Abonnements");

            migrationBuilder.DropIndex(
                name: "IX_abonnementPaiments_AbonnementId1",
                table: "abonnementPaiments");

            migrationBuilder.DropColumn(
                name: "AbonnementId",
                table: "abonnementPaiments");

            migrationBuilder.DropColumn(
                name: "AbonnementId1",
                table: "abonnementPaiments");
        }
    }
}

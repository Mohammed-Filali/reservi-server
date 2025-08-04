using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class create_Abonnement_Paiment_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "abonnementPaiments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProfetionnalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfetionnalId1 = table.Column<int>(type: "int", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_at = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abonnementPaiments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_abonnementPaiments_Profetionnals_ProfetionnalId1",
                        column: x => x.ProfetionnalId1,
                        principalTable: "Profetionnals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_abonnementPaiments_ProfetionnalId1",
                table: "abonnementPaiments",
                column: "ProfetionnalId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "abonnementPaiments");
        }
    }
}

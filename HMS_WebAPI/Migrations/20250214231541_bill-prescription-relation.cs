using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMS_Phase1.Migrations
{
    /// <inheritdoc />
    public partial class billprescriptionrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bills_PrescriptionId",
                table: "Bills",
                column: "PrescriptionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Prescriptions_PrescriptionId",
                table: "Bills",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Prescriptions_PrescriptionId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_PrescriptionId",
                table: "Bills");
        }
    }
}

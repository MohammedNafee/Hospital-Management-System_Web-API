using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMS_Phase1.Migrations
{
    /// <inheritdoc />
    public partial class Cascadedeletebehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Prescriptions_PrescriptionId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedications_Medications_MedicationId",
                table: "PrescriptionMedications");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedications_Prescriptions_PrescriptionId",
                table: "PrescriptionMedications");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Prescriptions_PrescriptionId",
                table: "Bills",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedications_Medications_MedicationId",
                table: "PrescriptionMedications",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "MedicationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedications_Prescriptions_PrescriptionId",
                table: "PrescriptionMedications",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Prescriptions_PrescriptionId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedications_Medications_MedicationId",
                table: "PrescriptionMedications");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedications_Prescriptions_PrescriptionId",
                table: "PrescriptionMedications");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Prescriptions_PrescriptionId",
                table: "Bills",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedications_Medications_MedicationId",
                table: "PrescriptionMedications",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "MedicationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedications_Prescriptions_PrescriptionId",
                table: "PrescriptionMedications",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

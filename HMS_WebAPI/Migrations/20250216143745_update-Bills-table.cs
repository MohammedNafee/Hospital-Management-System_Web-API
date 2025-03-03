using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMS_Phase1.Migrations
{
    /// <inheritdoc />
    public partial class updateBillstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Patients_PatientId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedication_Medications_MedicationId",
                table: "PrescriptionMedication");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedication_Prescriptions_PrescriptionId",
                table: "PrescriptionMedication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrescriptionMedication",
                table: "PrescriptionMedication");

            migrationBuilder.RenameTable(
                name: "PrescriptionMedication",
                newName: "PrescriptionMedications");

            migrationBuilder.RenameIndex(
                name: "IX_PrescriptionMedication_PrescriptionId",
                table: "PrescriptionMedications",
                newName: "IX_PrescriptionMedications_PrescriptionId");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Bills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrescriptionMedications",
                table: "PrescriptionMedications",
                columns: new[] { "MedicationId", "PrescriptionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Patients_PatientId",
                table: "Bills",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Patients_PatientId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedications_Medications_MedicationId",
                table: "PrescriptionMedications");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedications_Prescriptions_PrescriptionId",
                table: "PrescriptionMedications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrescriptionMedications",
                table: "PrescriptionMedications");

            migrationBuilder.RenameTable(
                name: "PrescriptionMedications",
                newName: "PrescriptionMedication");

            migrationBuilder.RenameIndex(
                name: "IX_PrescriptionMedications_PrescriptionId",
                table: "PrescriptionMedication",
                newName: "IX_PrescriptionMedication_PrescriptionId");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrescriptionMedication",
                table: "PrescriptionMedication",
                columns: new[] { "MedicationId", "PrescriptionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Patients_PatientId",
                table: "Bills",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedication_Medications_MedicationId",
                table: "PrescriptionMedication",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "MedicationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedication_Prescriptions_PrescriptionId",
                table: "PrescriptionMedication",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

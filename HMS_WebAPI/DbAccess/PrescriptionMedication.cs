
using Microsoft.EntityFrameworkCore;

namespace HMS_Phase1.Entities
{
    [PrimaryKey(nameof(MedicationId), nameof(PrescriptionId))]
    public class PrescriptionMedication
    {
        public int MedicationId { get; set; }
        public Medication Medication { get; set; }

        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }

        public PrescriptionMedication(int prescriptionId, int medicationId)
        {
            PrescriptionId = prescriptionId;
            MedicationId = medicationId;
        }

        public override string ToString()
        {
            return $"- Prescription Medication =>\n" +
                   $"   * Prescription ID: {PrescriptionId} \n" +
                   $"   * Medication ID: {MedicationId}";
        }
    }
}

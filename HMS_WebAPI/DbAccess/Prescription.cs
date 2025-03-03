
using Azure;
using System.Reflection;

namespace HMS_Phase1.Entities
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public DateTime PrescriptionDate { get; set; }

        public int PatientId { get; set; } // Foreign Key
        public Patient Patient { get; set; } // Navigation Property

        public int DoctorId { get; set; } // Foreign Key
        public Doctor Doctor { get; set; } // Navigation Property

        public List<PrescriptionMedication> Medications { get; set; } = new List<PrescriptionMedication>();
        public Bill Bill { get; set; }

        public Prescription(DateTime prescriptionDate, int patientId, int doctorId)
        {
            PrescriptionDate = prescriptionDate;
            PatientId = patientId;
            DoctorId = doctorId;
        }

        public override string ToString()
        {
           return $"- Prescription {PrescriptionId} => \n" +
                  $"   * Prescription Date: {PrescriptionDate} \n" +
                  $"   * Patient ID: {PatientId} \n" +
                  $"   * Doctor ID: {DoctorId}";
        }
    }
}

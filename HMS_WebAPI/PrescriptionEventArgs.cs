
namespace HMS_Phase1
{
    public class PrescriptionEventArgs : EventArgs
    {
        public int PrescriptionId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int MedicationId { get; set; }

        public PrescriptionEventArgs(int prescriptionId, DateTime prescriptionDate, int patientId, int doctorId, int medicationId)
        {
            PrescriptionId = prescriptionId;
            PrescriptionDate = prescriptionDate;
            PatientId = patientId;
            DoctorId = doctorId;
            MedicationId = medicationId;
        }
    }
}

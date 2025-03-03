namespace HMS_WebAPI.DTOs
{
    public class PrescriptionDTO
    {
        public DateTime PrescriptionDate { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int MedicationId { get; set; }
    }
}

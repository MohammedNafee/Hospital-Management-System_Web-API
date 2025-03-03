
namespace HMS_Phase1.Entities
{
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Canceled
    }
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }

        public int PatientId { get; set; } // Foreign Key
        public Patient Patient { get; set; } // Navigation Property

        public int DoctorId { get; set; }  // Foreign Key
        public Doctor Doctor { get; set; } // Navigation Property

        public Appointment(DateTime appointmentDate, int patientId, int doctorId)
        {
            AppointmentDate = appointmentDate;
            Status = AppointmentStatus.Scheduled;
            PatientId = patientId;
            DoctorId = doctorId;
        }

        public override string ToString()
        {
            return $"- Appointment {AppointmentId} => \n" +
                    $"   * Appointment Date: {AppointmentDate} \n" +
                    $"   * Status: {Status} \n" +
                    $"   * Patient ID: {PatientId} \n" +
                    $"   * Doctor ID: {DoctorId}";
        }
    }
}

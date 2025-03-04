using HMS_Phase1.Entities;
using HMS_Phase1;

namespace HMS_WebAPI.DbAccess
{
    public class AppointmentRepository
    {
        private readonly HMSContext _context;

        public AppointmentRepository(HMSContext context)
        {
            _context = context;
        }

        public void AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public Appointment? GetAppointmentById(int appointmentId)
        {
            return _context.Appointments.SingleOrDefault(a => a.AppointmentId == appointmentId);
        }

        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return _context.Appointments.Where(a => a.PatientId == patientId).ToList();
        }

        public List<Appointment> GetAppointmentsByDoctorId(int doctorId)
        {
            return _context.Appointments.Where(a => a.DoctorId == doctorId).ToList();
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            _context.SaveChanges();
        }

        public bool DeleteAppointment(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            return _context.SaveChanges() > 0;
        }
    }
}

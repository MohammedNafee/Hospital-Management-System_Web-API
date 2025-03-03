using HMS_Phase1.Entities;
using HMS_WebAPI.DTOs;

namespace HMS_Phase1.Management_Classes
{
    public class AppointmentManager
    {
        private readonly HMSContext _context;

        public AppointmentManager(HMSContext context)
        {
            _context = context;
        }

        public void ScheduleAppointment(Appointment appointment)
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

        public bool CancelAppointment(int appointmentId)
        {
            var appointment = _context.Appointments.SingleOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null) return false;

            appointment.Status = AppointmentStatus.Canceled;
            _context.SaveChanges();
            return true;
        }

        public Appointment? UpdateAppointment(int appointmentId, AppointmentDTO appointmentDTO)
        {
            var appointment = _context.Appointments.SingleOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null)
                return null;

            appointment.PatientId = appointmentDTO.PatientId;
            appointment.DoctorId = appointmentDTO.DoctorId;
            appointment.AppointmentDate = appointmentDTO.AppointmentDate;

            _context.SaveChanges();
            return appointment;
        }
    }
}

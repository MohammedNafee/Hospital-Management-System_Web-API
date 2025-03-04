using HMS_Phase1.Entities;
using HMS_WebAPI.DbAccess;
using HMS_WebAPI.DTOs;

namespace HMS_Phase1.Management_Classes
{
    public class AppointmentManager
    {
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentManager(AppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public void ScheduleAppointment(AppointmentDTO appointmentDto, int loggedInUserId, string role)
        {
            if (appointmentDto == null)
                throw new ArgumentException("Invalid appointment data");

            if (role == "Patient" && loggedInUserId != appointmentDto.PatientId)
                throw new UnauthorizedAccessException("Patients can only schedule their own appointments.");

            if (role == "Doctor" && loggedInUserId != appointmentDto.DoctorId)
                throw new UnauthorizedAccessException("Doctors can only schedule their own appointments.");

            var appointment = new Appointment(
                appointmentDto.AppointmentDate,
                appointmentDto.PatientId,
                appointmentDto.DoctorId
            );

            _appointmentRepository.AddAppointment(appointment);
        }

        public Appointment? GetAppointmentById(int appointmentId)
        {
            return _appointmentRepository.GetAppointmentById(appointmentId);    
        }

        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return _appointmentRepository.GetAppointmentsByPatientId(patientId);
        }

        public List<Appointment> GetAppointmentsByDoctorId(int doctorId)
        {
            return _appointmentRepository.GetAppointmentsByDoctorId(doctorId);
        }

        public void CancelAppointment(int appointmentId, int loggedInUserId, string role)
        {
            var appointment = _appointmentRepository.GetAppointmentById(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found.");

            if (role == "Patient" && loggedInUserId != appointment.PatientId)
                throw new UnauthorizedAccessException("Patients can only cancel their own appointments.");

            if (role == "Doctor" && loggedInUserId != appointment.DoctorId)
                throw new UnauthorizedAccessException("Doctors can only cancel their own appointments.");

            appointment.Status = AppointmentStatus.Canceled;
            _appointmentRepository.UpdateAppointment(appointment);
        }

        public Appointment? UpdateAppointment(int appointmentId, AppointmentDTO appointmentDto, int loggedInUserId, string role)
        {
            var appointment = _appointmentRepository.GetAppointmentById(appointmentId);
            if (appointment == null)
                return null;

            if (role == "Doctor" && loggedInUserId != appointment.DoctorId)
                throw new UnauthorizedAccessException("Doctors can only update their own appointments.");

            appointment.PatientId = appointmentDto.PatientId;
            appointment.DoctorId = appointmentDto.DoctorId;
            appointment.AppointmentDate = appointmentDto.AppointmentDate;

            _appointmentRepository.UpdateAppointment(appointment);
            return appointment;
        }
    }
}

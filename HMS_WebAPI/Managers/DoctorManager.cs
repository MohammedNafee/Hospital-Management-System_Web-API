using HMS_Phase1.Entities;

namespace HMS_Phase1.Management_Classes
{
    public class DoctorManager
    {
        private readonly HMSContext _context;

        public DoctorManager(HMSContext context)
        {
            _context = context;
        }

        public void AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }

        public List<Doctor> GetAllDoctors()
        {
            return _context.Doctors.ToList();
        }

        public Doctor? GetDoctorById(int doctorId)
        {
            return _context.Doctors.SingleOrDefault(d => d.DoctorId == doctorId);
        }

        public Doctor? UpdateDoctor(int doctorId, Doctor updatedDoctor)
        {
            var doctor = _context.Doctors.SingleOrDefault(d => d.DoctorId == doctorId);
            if (doctor == null) return null;

            doctor.Name = updatedDoctor.Name;
            doctor.Age = updatedDoctor.Age;
            doctor.Gender = updatedDoctor.Gender;
            doctor.ContactNumber = updatedDoctor.ContactNumber;
            doctor.Email = updatedDoctor.Email;
            doctor.Specialty = updatedDoctor.Specialty;

            _context.SaveChanges();
            return doctor;
        }

        public bool DeleteDoctor(int doctorId)
        {
            var doctor = _context.Doctors.SingleOrDefault(d => d.DoctorId == doctorId);
            if (doctor == null) return false;

            // Check for dependencies
            bool hasAppointments = _context.Appointments.Any(a => a.DoctorId == doctorId);
            bool hasPrescriptions = _context.Prescriptions.Any(p => p.DoctorId == doctorId);

            if (hasAppointments || hasPrescriptions)
            {
                throw new InvalidOperationException("Doctor has dependent records (Appointments/Prescriptions) and cannot be deleted.");
            }

            _context.Doctors.Remove(doctor);
            _context.SaveChanges();
            return true;
        }
    }
}

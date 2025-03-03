using HMS_Phase1.Entities;

namespace HMS_Phase1.Management_Classes
{
    public class PatientManager
    {
        private readonly HMSContext _context;

        public PatientManager(HMSContext context)
        {
            _context = context;
        }

        public void AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();  
        }

        public Patient? UpdatePatient(int patientId, Patient updatedPatient)
        {
            var patient = _context.Patients.SingleOrDefault(p => p.PatientId == patientId);
            if (patient == null) return null;

            patient.Name = updatedPatient.Name;
            patient.Age = updatedPatient.Age;
            patient.Gender = updatedPatient.Gender;
            patient.ContactNumber = updatedPatient.ContactNumber;
            patient.Address = updatedPatient.Address;

            _context.SaveChanges();
            return patient;
        }

        public bool DeletePatient(int patientId)
        {
            var patient = _context.Patients.SingleOrDefault(p => p.PatientId == patientId);
            if (patient == null) return false;

            // Check for dependencies
            bool hasAppointments = _context.Appointments.Any(a => a.PatientId == patientId);
            bool hasPrescriptions = _context.Prescriptions.Any(p => p.PatientId == patientId);
            bool hasBills = _context.Bills.Any(b => b.Prescription.PatientId == patientId);

            if (hasAppointments || hasPrescriptions || hasBills)
            {
                throw new InvalidOperationException("Patient has dependent records (Appointments, Prescriptions, Bills) and cannot be deleted.");
            }

            _context.Patients.Remove(patient);
            _context.SaveChanges();
            return true;
        }


        public List<Patient> GetAllPatients()
        {
            return _context.Patients.ToList();
        }

        public Patient? GetPatientById(int id)
        {
            return _context.Patients.SingleOrDefault(p => p.PatientId == id);
        }


    }
}

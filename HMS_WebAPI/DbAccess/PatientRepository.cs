using HMS_Phase1.Entities;
using HMS_Phase1;

namespace HMS_WebAPI.DbAccess
{
    public class PatientRepository
    {
        private readonly HMSContext _context;

        public PatientRepository(HMSContext context)
        {
            _context = context;
        }

        public void AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public Patient? GetPatientById(int patientId)
        {
            return _context.Patients.SingleOrDefault(p => p.PatientId == patientId);
        }

        public List<Patient> GetAllPatients()
        {
            return _context.Patients.ToList();
        }

        public void UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

        public bool DeletePatient(Patient patient)
        {
            _context.Patients.Remove(patient);
            return _context.SaveChanges() > 0;
        }

        public bool HasDependencies(int patientId)
        {
            return _context.Appointments.Any(a => a.PatientId == patientId) ||
                   _context.Prescriptions.Any(p => p.PatientId == patientId) ||
                   _context.Bills.Any(b => b.Prescription.PatientId == patientId);
        }
    }
}

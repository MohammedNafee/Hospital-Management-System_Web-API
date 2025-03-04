using HMS_Phase1;
using HMS_Phase1.Entities;

namespace HMS_WebAPI.DbAccess
{
    public class MedicationRepository
    {
        private readonly HMSContext _context;

        public MedicationRepository(HMSContext context)
        {
            _context = context;
        }

        public void AddMedication(Medication medication)
        {
            _context.Medications.Add(medication);
            _context.SaveChanges();
        }

        public List<Medication> GetAllMedications()
        {
            return _context.Medications.ToList();
        }

        public Medication? GetMedicationById(int medicationId)
        {
            return _context.Medications.SingleOrDefault(med => med.MedicationId == medicationId);
        }

        public void UpdateMedication(Medication medication)
        {
            _context.Medications.Update(medication);
            _context.SaveChanges();
        }

        public bool DeleteMedication(Medication medication)
        {
            _context.Medications.Remove(medication);
            return _context.SaveChanges() > 0;
        }

        public bool IsMedicationUsedInPrescriptions(int medicationId)
        {
            return _context.PrescriptionMedications.Any(pm => pm.MedicationId == medicationId);
        }
    }
}

using HMS_Phase1.Entities;
using HMS_WebAPI.DTOs;

namespace HMS_Phase1.Management_Classes
{
    public class MedicationManager
    {
        private readonly HMSContext _context;

        public MedicationManager(HMSContext context)
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

        public Medication? UpdateMedication(int medicationId, MedicationDTO updatedMedication)
        {
            var medication = _context.Medications.SingleOrDefault(med => med.MedicationId == medicationId);
            if (medication == null)
                return null; // Medication not found

            medication.Name = updatedMedication.Name;
            medication.Quantity = updatedMedication.Quantity;
            medication.Price = updatedMedication.Price;

            _context.SaveChanges();
            return medication;
        }

        public bool DeleteMedication(int medicationId)
        {
            var medication = _context.Medications.SingleOrDefault(med => med.MedicationId == medicationId);
            if (medication == null)
                return false; // Medication not found

            // Check if the medication is linked to any prescriptions
            bool isUsedInPrescriptions = _context.PrescriptionMedications.Any(pm => pm.MedicationId == medicationId);
            if (isUsedInPrescriptions)
                throw new InvalidOperationException("Cannot delete medication because it is linked to prescriptions.");

            _context.Medications.Remove(medication);
            _context.SaveChanges();
            return true;
        }
    }
}

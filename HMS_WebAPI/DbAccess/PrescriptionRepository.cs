using HMS_Phase1;
using HMS_Phase1.Entities;

namespace HMS_WebAPI.DbAccess
{
    public class PrescriptionRepository
    {
        private readonly HMSContext _context;

        public PrescriptionRepository(HMSContext context)
        {
            _context = context;
        }

        public void AddPrescription(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();
        }

        public void AddPrescriptionMedication(PrescriptionMedication prescriptionMedication)
        {
            _context.PrescriptionMedications.Add(prescriptionMedication);
            _context.SaveChanges();
        }

        public Prescription? GetPrescriptionById(int id)
        {
            return _context.Prescriptions.SingleOrDefault(pre => pre.PrescriptionId == id);
        }

        public List<Prescription> GetAllPrescriptions()
        {
            return _context.Prescriptions.ToList();
        }

        public void UpdatePrescription(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            _context.SaveChanges();
        }

        public Medication? GetMedicationById(int medicationId)
        {
            return _context.Medications.SingleOrDefault(m => m.MedicationId == medicationId);
        }

        public void UpdateMedication(Medication medication)
        {
            _context.Medications.Update(medication);
            _context.SaveChanges();
        }
    }
}

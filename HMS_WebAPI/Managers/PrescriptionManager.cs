using HMS_Phase1.Entities;
using HMS_WebAPI.DTOs;

namespace HMS_Phase1.Management_Classes
{
    public class PrescriptionManager
    {
        private readonly HMSContext _context;
        private readonly BillingManager _billingManager;

        public PrescriptionManager(HMSContext context, BillingManager billingManager)
        {
            _context = context;
            _billingManager = billingManager;
        }

        public void IssuePrescription(PrescriptionDTO prescriptionDTO)
        {
            if (prescriptionDTO == null)
                throw new ArgumentNullException(nameof(prescriptionDTO), "Invalid prescription data");

            var medication = _context.Medications.SingleOrDefault(m => m.MedicationId == prescriptionDTO.MedicationId);
            
            if (medication == null)
                throw new InvalidOperationException("Medication not found");

            if (medication.Quantity <= 0)
                throw new InvalidOperationException("Medication is out of stock");

            // Reduce stock quantity
            medication.Quantity -= 1;

            var prescription = new Prescription(
                prescriptionDTO.PrescriptionDate,
                prescriptionDTO.PatientId,
                prescriptionDTO.DoctorId
            );

            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();

            var prescriptionMedication = new PrescriptionMedication(
                prescription.PrescriptionId, 
                prescriptionDTO.MedicationId
            );
            
            _context.PrescriptionMedications.Add(prescriptionMedication);
            _context.SaveChanges();

            // Generate bill after issuing a prescription
            var eventArgs = new PrescriptionEventArgs(
                prescription.PrescriptionId, 
                prescriptionDTO.PrescriptionDate, 
                prescriptionDTO.PatientId, 
                prescriptionDTO.DoctorId, 
                prescriptionDTO.MedicationId
            );

            _billingManager.GenerateBill(eventArgs);
        }

        public Prescription? GetPrescriptionById(int id)
        {
            return _context.Prescriptions.SingleOrDefault(pre => pre.PrescriptionId == id);
        }

        public List<Prescription> GetAllPrescriptions()
        {
            return _context.Prescriptions.ToList();
        }

        public Prescription? UpdatePrescription(int id, PrescriptionDTO updatedPrescription)
        {
            var prescription = _context.Prescriptions.SingleOrDefault(pre => pre.PrescriptionId == id);
            if (prescription == null) return null;

            prescription.PrescriptionDate = updatedPrescription.PrescriptionDate;
            prescription.PatientId = updatedPrescription.PatientId; 
            prescription.DoctorId = updatedPrescription.DoctorId;
            
            _context.SaveChanges(); 
            return prescription;
        }
    }
}

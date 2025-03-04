using HMS_Phase1.Entities;
using HMS_WebAPI.DbAccess;
using HMS_WebAPI.DTOs;

namespace HMS_Phase1.Management_Classes
{
    public class PrescriptionManager
    {
        private readonly PrescriptionRepository _prescriptionRepository;
        private readonly BillingManager _billingManager;

        public PrescriptionManager(PrescriptionRepository prescriptionRepository, BillingManager billingManager)
        {
            _prescriptionRepository = prescriptionRepository;   
            _billingManager = billingManager;
        }

        public void IssuePrescription(PrescriptionDTO prescriptionDTO)
        {
            if (prescriptionDTO == null)
                throw new ArgumentNullException(nameof(prescriptionDTO), "Invalid prescription data");

            var medication = _prescriptionRepository.GetMedicationById(prescriptionDTO.MedicationId);
            
            if (medication == null)
                throw new InvalidOperationException("Medication not found");

            if (medication.Quantity <= 0)
                throw new InvalidOperationException("Medication is out of stock");

            // Reduce stock quantity
            medication.Quantity -= 1;
            _prescriptionRepository.UpdateMedication(medication);

            var prescription = new Prescription(
                prescriptionDTO.PrescriptionDate,
                prescriptionDTO.PatientId,
                prescriptionDTO.DoctorId
            );

            _prescriptionRepository.AddPrescription(prescription);

            var prescriptionMedication = new PrescriptionMedication(
                prescription.PrescriptionId, 
                prescriptionDTO.MedicationId
            );
            
            _prescriptionRepository.AddPrescriptionMedication(prescriptionMedication);

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
            return _prescriptionRepository.GetPrescriptionById(id);
        }

        public List<Prescription> GetAllPrescriptions()
        {
            return _prescriptionRepository.GetAllPrescriptions();
        }

        public Prescription? UpdatePrescription(int id, PrescriptionDTO updatedPrescription)
        {
            if (updatedPrescription == null)
                throw new ArgumentException("Invalid Prescription data");

            var prescription = _prescriptionRepository.GetPrescriptionById(id);
            if (prescription == null) return null;

            prescription.PrescriptionDate = updatedPrescription.PrescriptionDate;
            prescription.PatientId = updatedPrescription.PatientId; 
            prescription.DoctorId = updatedPrescription.DoctorId;
            
            _prescriptionRepository.UpdatePrescription(prescription);
            return prescription;
        }
    }
}

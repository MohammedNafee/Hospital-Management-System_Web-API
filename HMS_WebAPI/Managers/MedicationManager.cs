using HMS_Phase1.Entities;
using HMS_WebAPI.DbAccess;
using HMS_WebAPI.DTOs;

namespace HMS_Phase1.Management_Classes
{
    public class MedicationManager
    {
        private readonly MedicationRepository _medicationRepository;

        public MedicationManager(MedicationRepository medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        public void AddMedication(MedicationDTO medicationDTO)
        {
            if (medicationDTO == null)
                throw new ArgumentException("Invalid medication data");

            var medication = new Medication
            (
                medicationDTO.Name,
                medicationDTO.Quantity,
                medicationDTO.Price
            );

            _medicationRepository.AddMedication(medication);
        }

        public List<Medication> GetAllMedications()
        {
            return _medicationRepository.GetAllMedications();
        }

        public Medication? UpdateMedication(int medicationId, MedicationDTO medicationDTO)
        {
            if (medicationDTO == null)
                throw new ArgumentException("Invalid medication data");

            var medication = _medicationRepository.GetMedicationById(medicationId);
            if (medication == null)
                return null; // Medication not found

            medication.Name = medicationDTO.Name;
            medication.Quantity = medicationDTO.Quantity;
            medication.Price = medicationDTO.Price;

            _medicationRepository.UpdateMedication(medication);
            return medication;
        }

        public bool DeleteMedication(int medicationId)
        {
            var medication = _medicationRepository.GetMedicationById(medicationId);
            if (medication == null)
                return false; // Medication not found

            // Check if the medication is linked to any prescription
            if (_medicationRepository.IsMedicationUsedInPrescriptions(medicationId))
                throw new InvalidOperationException("Cannot delete medication because it is linked to prescriptions.");

            return _medicationRepository.DeleteMedication(medication);
        }
    }
}

using HMS_Phase1.Entities;
using HMS_WebAPI.DbAccess;
using HMS_WebAPI.DTOs;

namespace HMS_Phase1.Management_Classes
{
    public class PatientManager
    {
        private readonly PatientRepository _patientRepository;
        public PatientManager(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public void AddPatient(PatientDTO patientDto)
        {
            if (patientDto == null) 
                throw new ArgumentException("Invalid patient data");

            var patient = new Patient
                (
                    patientDto.Name,
                    patientDto.Age,
                    patientDto.Gender,
                    patientDto.ContactNumber,
                    patientDto.Address
                );

            _patientRepository.AddPatient( patient );
        }

        public Patient? UpdatePatient(int patientId, PatientDTO patientDTO, int loggedInUserId, string role)
        {
            var patient = _patientRepository.GetPatientById(patientId);
            if (patient == null) return null;

            // Patients can only update their own profile
            if (role == "Patient" && loggedInUserId != patientId)
                throw new UnauthorizedAccessException("Patients can only update their own profile.");

            if (patientDTO == null)
                throw new ArgumentException("Invalid patient data");

            var updatedPatient = new Patient
                (
                    patientDTO.Name,
                    patientDTO.Age,
                    patientDTO.Gender,
                    patientDTO.ContactNumber,
                    patientDTO.Address
                );

            patient.Name = updatedPatient.Name;
            patient.Age = updatedPatient.Age;
            patient.Gender = updatedPatient.Gender;
            patient.ContactNumber = updatedPatient.ContactNumber;
            patient.Address = updatedPatient.Address;

            _patientRepository.UpdatePatient( patient );
            return patient;
        }

        public bool DeletePatient(int patientId)
        {
            var patient = _patientRepository.GetPatientById(patientId);
            if (patient == null) return false;

            // Check for dependencies
            if (_patientRepository.HasDependencies(patientId))
            {
                throw new InvalidOperationException("Patient has dependent records (Appointments, Prescriptions, Bills) and cannot be deleted.");
            }

            return _patientRepository.DeletePatient( patient );
        }


        public List<Patient> GetAllPatients(int loggedInUserId, string role)
        {
            if (role == "Patient")
            {
                var patient = _patientRepository.GetPatientById(loggedInUserId);    
                return patient != null ? new List<Patient> { patient } : new List<Patient>();
            }
            return _patientRepository.GetAllPatients();
        }

        public Patient? GetPatientById(int id, int loggedInUserId, string role)
        {
            if (role == "Patient" && id != loggedInUserId)
            {
                throw new UnauthorizedAccessException("Patients can only view their own profile.");
            }
            return _patientRepository.GetPatientById(id);
        }


    }
}

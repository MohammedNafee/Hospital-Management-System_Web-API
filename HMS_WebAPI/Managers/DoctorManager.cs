using HMS_Phase1.Entities;
using HMS_WebAPI.DbAccess;
using HMS_WebAPI.DTOs;

namespace HMS_Phase1.Management_Classes
{
    public class DoctorManager
    {
        private readonly DoctorRepository _doctorRepository;

        public DoctorManager(DoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public void AddDoctor(DoctorDTO doctorDTO)
        {
            if (doctorDTO == null)
                throw new ArgumentException("Invalid doctor data");

            var doctor = new Doctor(
                doctorDTO.Name,
                doctorDTO.Age,
                doctorDTO.Gender,
                doctorDTO.ContactNumber,
                doctorDTO.Email,
                doctorDTO.Specialty
            );

            _doctorRepository.AddDoctor(doctor);
        }


        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAllDoctors();
        }

        public Doctor? GetDoctorById(int doctorId)
        {
            return _doctorRepository.GetDoctorById(doctorId);
        }

        public Doctor? UpdateDoctor(int doctorId, DoctorDTO doctorDTO)
        {
            if (doctorDTO == null)
                throw new ArgumentException("Invalid doctor data");

            var doctor = _doctorRepository.GetDoctorById(doctorId);
            if (doctor == null) return null;

            var updatedDoctor = new Doctor
                (
                    doctorDTO.Name,
                    doctorDTO.Age,
                    doctorDTO.Gender,
                    doctorDTO.ContactNumber,
                    doctorDTO.Email,
                    doctorDTO.Specialty
                );

            doctor.Name = updatedDoctor.Name;
            doctor.Age = updatedDoctor.Age;
            doctor.Gender = updatedDoctor.Gender;
            doctor.ContactNumber = updatedDoctor.ContactNumber;
            doctor.Email = updatedDoctor.Email;
            doctor.Specialty = updatedDoctor.Specialty;

            _doctorRepository.UpdateDoctor(doctor);
           return doctor;
        }

        public bool DeleteDoctor(int doctorId)
        {
            var doctor = _doctorRepository.GetDoctorById(doctorId);
            if (doctor == null) return false;

            if (_doctorRepository.HasDependencies(doctorId))
            {
                throw new InvalidOperationException("Doctor has dependent records (Appointments/Prescriptions) and cannot be deleted.");
            }

            return _doctorRepository.DeleteDoctor(doctor);
        }
    }
}

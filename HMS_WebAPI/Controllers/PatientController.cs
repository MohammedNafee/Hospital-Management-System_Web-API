using System.Security.Claims;
using HMS_Phase1.Entities;
using HMS_Phase1.Management_Classes;
using HMS_WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PatientController : ControllerBase
    {
        private readonly PatientManager _patientManager;
        public PatientController(PatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult AddPatient([FromBody] PatientDTO patientDto)
        {
            if (patientDto == null)
                return BadRequest("Invalid patient data");

            var patient = new Patient
                (
                    patientDto.Name,
                    patientDto.Age,
                    patientDto.Gender,
                    patientDto.ContactNumber,
                    patientDto.Address
                );

            _patientManager.AddPatient(patient);
            return Created();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult UpdatePatient(int id, [FromBody] PatientDTO patientDTO)
        {
            if (patientDTO == null)
                return BadRequest("Invalid patient data");

            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                return Forbid();

            // Patients can only update their own profile
            if (User.IsInRole("Patient") && userId != id)
                return Forbid("Patients can only update their own profile.");

            var updatedPatient = new Patient
            (
                patientDTO.Name,
                patientDTO.Age,
                patientDTO.Gender,
                patientDTO.ContactNumber,
                patientDTO.Address
            );

            var patient = _patientManager.UpdatePatient(id, updatedPatient);
            if (patient == null)
                return NotFound("Patient not found");

            return Ok(patient);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult DeletePatient(int id)
        {
            try
            {
                bool deleted = _patientManager.DeletePatient(id);
                if (!deleted)
                    return NotFound("Patient not found");

                return Ok("Patient deleted successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetAllPatients()
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                return Forbid();

            // Patients can only see their own data
            if (User.IsInRole("Patient"))
            {
                var patient = _patientManager.GetPatientById(userId);
                if (patient == null)
                    return NotFound("Patient not found");

                return Ok(new List<Patient> { patient });
            }

            var patients = _patientManager.GetAllPatients();
            if (!patients.Any())
                return NotFound("No patients available");

            return Ok(patients);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetPatientById(int id)
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                return Forbid();

            if (User.IsInRole("Patient") && userId != id)
                return Forbid("Patients can only view their own profile.");

            var patient = _patientManager.GetPatientById(id);
            if (patient == null)
                return NotFound("Patient not found");

            return Ok(patient);
        }
    }
}

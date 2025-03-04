using System.Security.Claims;
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

        private (int userId, string role) GetUserInfo()
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var loggedInUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                throw new  UnauthorizedAccessException();

            return (userId, loggedInUserRole);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult AddPatient([FromBody] PatientDTO patientDto)
        {
            try
            {
                _patientManager.AddPatient(patientDto);
                return Created();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult UpdatePatient(int id, [FromBody] PatientDTO patientDTO)
        {

            try
            {
                var (userId, role) = GetUserInfo();

                var patient = _patientManager.UpdatePatient(id, patientDTO, userId, role);

                if (patient == null)
                    return NotFound("Patient not found");

                return Ok(patient);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }  
          
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
            var (userId, role) = GetUserInfo();
            
            var patients = _patientManager.GetAllPatients(userId, role);
            if (!patients.Any())
                return NotFound("No patients available");

            return Ok(patients);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetPatientById(int id)
        {
            try
            {

                var (userId, role) = GetUserInfo();

                var patient = _patientManager.GetPatientById(id, userId, role);

                if (patient == null)
                    return NotFound("Patient not found");

                return Ok(patient);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();            
            }
        }
    }
}

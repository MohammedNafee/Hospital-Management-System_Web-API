using System.Security.Claims;
using HMS_Phase1.Management_Classes;
using HMS_WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_WebAPI.Controllers
{
    [Route("api/[Controller]/[action]")]
    public class PrescriptionController : ControllerBase
    {
        private readonly PrescriptionManager _prescriptionManager;

        public PrescriptionController(PrescriptionManager prescriptionManager)
        {
            _prescriptionManager = prescriptionManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult IssuePrescription([FromBody] PrescriptionDTO prescriptionDTO)
        {
            try
            {
                _prescriptionManager.IssuePrescription(prescriptionDTO);
                return Created();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetPrescriptionById(int id)
        {
            var prescription = _prescriptionManager.GetPrescriptionById(id);
            if (prescription == null)
                return NotFound("Prescription not found");

            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                return Forbid();

            if (User.IsInRole("Patient") && prescription.PatientId != userId)
                return Forbid("Patient can only view their own prescriptions");

            return Ok(prescription);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetAllPrescriptions()
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                return Forbid();

            if (User.IsInRole("Patient"))
            {
                var prescriptions = _prescriptionManager.GetAllPrescriptions()
                    .Where(pre => pre.PatientId == userId).ToList();

                if(!prescriptions.Any())
                    return NotFound("No prescriptions found");

                return Ok(prescriptions);

            }

            var allPrescriptions = _prescriptionManager.GetAllPrescriptions();
            if (!allPrescriptions.Any())
                return NotFound("No prescriptions found");

            return Ok(allPrescriptions);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult UpdatePrescription(int id, [FromBody] PrescriptionDTO prescriptionDTO)
        {
            if (prescriptionDTO == null)
                return BadRequest("Invalid prescription data");

            var updatedPrescription = _prescriptionManager.UpdatePrescription(id, prescriptionDTO);

            if (updatedPrescription == null)
                return NotFound("Prescription not found");

            return Ok(updatedPrescription); 
        }
    }
}

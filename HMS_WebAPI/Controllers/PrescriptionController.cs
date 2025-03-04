using System.Security.Claims;
using HMS_Phase1.Management_Classes;
using HMS_WebAPI.DTOs;
using HMS_WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_WebAPI.Controllers
{
    [Route("api/[Controller]/[action]")]
    public class PrescriptionController : ControllerBase
    {
        private readonly PrescriptionManager _prescriptionManager;
        private readonly UserInfoService _userInfoService;

        public PrescriptionController(PrescriptionManager prescriptionManager, UserInfoService userInfoService)
        {
            _prescriptionManager = prescriptionManager;
            _userInfoService = userInfoService; 
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

            var (userId, role) = _userInfoService.GetUserInfo();

            if (role == "Patient" && prescription.PatientId != userId)
                return Forbid("Patients can only view their own prescriptions");

            return Ok(prescription);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetAllPrescriptions()
        {
            var (userId, role) = _userInfoService.GetUserInfo();

            if (role == "Patient")
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

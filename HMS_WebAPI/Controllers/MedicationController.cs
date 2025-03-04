using HMS_Phase1.Management_Classes;
using HMS_WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_WebAPI.Controllers
{
    [Route("api/[Controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class MedicationController : ControllerBase
    {
        private readonly MedicationManager _medicationManager;

        public MedicationController(MedicationManager medicationManager)
        {
            _medicationManager = medicationManager;
        }

        [HttpPost]
        public IActionResult AddMedication([FromBody] MedicationDTO medicationDTO)
        {
            try
            {
                _medicationManager.AddMedication(medicationDTO);
                return Created();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAllMedications()
        {
            var medications = _medicationManager.GetAllMedications();
            if (!medications.Any())
                return NotFound("No medications available");

            return Ok(medications);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateMedication(int id, [FromBody] MedicationDTO medicationDTO)
        {
            try
            {
                var updatedMedication = _medicationManager.UpdateMedication(id, medicationDTO);
                if (updatedMedication == null)
                    return NotFound("Medication not found");

                return Ok(updatedMedication);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMedication(int id)
        {
            try
            {
                bool deleted = _medicationManager.DeleteMedication(id);
                if (!deleted)
                    return NotFound("Medication not found");

                return Ok("Medication deleted successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

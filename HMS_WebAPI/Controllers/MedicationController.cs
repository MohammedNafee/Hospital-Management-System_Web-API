using HMS_Phase1.Entities;
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
            if (medicationDTO == null)
                return BadRequest("Invalid medication data");

            var medication = new Medication
            (
                medicationDTO.Name,
                medicationDTO.Quantity,
                medicationDTO.Price
            );

            _medicationManager.AddMedication(medication);
            return Created();
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
            if (medicationDTO == null)
                return BadRequest("Invalid medication data");

            var updatedMedication = _medicationManager.UpdateMedication(id, medicationDTO);
            if (updatedMedication == null)
                return NotFound("Medication not found");

            return Ok(updatedMedication);
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

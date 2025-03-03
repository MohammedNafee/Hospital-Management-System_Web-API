using HMS_Phase1.Entities;
using HMS_Phase1.Management_Classes;
using HMS_WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_WebAPI.Controllers
{
    [Route("api/[Controller]/[action]")]
    [Authorize(Roles = "Admin")] 
    public class DoctorController : ControllerBase
    {
        private readonly DoctorManager _doctorManager;

        public DoctorController(DoctorManager doctorManager)
        {
            _doctorManager = doctorManager;
        }

        [HttpPost]
        public IActionResult AddDoctor([FromBody] DoctorDTO doctorDTO)
        {
            if (doctorDTO == null)
                return BadRequest("Invalid doctor data");

            var doctor = new Doctor
            (
                doctorDTO.Name,
                doctorDTO.Age,
                doctorDTO.Gender,
                doctorDTO.ContactNumber,
                doctorDTO.Email,
                doctorDTO.Specialty
            );

            _doctorManager.AddDoctor(doctor);
            return Created();
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var doctors = _doctorManager.GetAllDoctors();
            if (!doctors.Any())
                return NotFound("No doctors available");

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public IActionResult GetDoctorById(int id)
        {
            var doctor = _doctorManager.GetDoctorById(id);
            if (doctor == null)
                return NotFound("Doctor not found");

            return Ok(doctor);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(int id, [FromBody] DoctorDTO doctorDTO)
        {
            if (doctorDTO == null)
                return BadRequest("Invalid doctor data");

            var updatedDoctor = new Doctor
            (
                doctorDTO.Name,
                doctorDTO.Age,
                doctorDTO.Gender,
                doctorDTO.ContactNumber,
                doctorDTO.Email,
                doctorDTO.Specialty
            );

            var doctor = _doctorManager.UpdateDoctor(id, updatedDoctor);
            if (doctor == null)
                return NotFound("Doctor not found");

            return Ok(doctor);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            try
            {
                bool deleted = _doctorManager.DeleteDoctor(id);
                if (!deleted)
                    return NotFound("Doctor not found");

                return Ok("Doctor deleted successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

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
            try
            {
                _doctorManager.AddDoctor(doctorDTO);
                return Created();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var doctor = _doctorManager.UpdateDoctor(id, doctorDTO);
                if (doctor == null)
                    return NotFound("Doctor not found");

                return Ok(doctor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            } 

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

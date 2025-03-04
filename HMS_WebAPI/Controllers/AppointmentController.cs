using System.Security.Claims;
using HMS_Phase1.Entities;
using HMS_Phase1.Management_Classes;
using HMS_WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_WebAPI.Controllers
{
    [Route("api/[Controller]/[action]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentManager _appointmentManager;

        public AppointmentController(AppointmentManager appointmentManager)
        {
            _appointmentManager = appointmentManager;
        }

        private (int userId, string role) GetUserInfo()
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var loggedInUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                throw new UnauthorizedAccessException();

            return (userId, loggedInUserRole);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult ScheduleAppointment([FromBody] AppointmentDTO appointmentDTO)
        {
            try
            {
                var (userId, role) = GetUserInfo();
                _appointmentManager.ScheduleAppointment(appointmentDTO, userId, role);
                return Created();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAppointmets([FromQuery] int? patientId, [FromQuery] int? doctorId)
        {
            if (patientId.HasValue)
                return Ok(_appointmentManager.GetAppointmentsByPatientId(patientId.Value));

            if (doctorId.HasValue)
                return Ok(_appointmentManager.GetAppointmentsByDoctorId(doctorId.Value));

            return BadRequest("Please provide either a patientId or doctorId.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult CancelAppointment(int id)
        {
            try
            {
                var (userId, role) = GetUserInfo();
                _appointmentManager.CancelAppointment(id, userId, role);
                return Ok($"Appointment {id} canceled successfully.");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult UpdateAppointment(int id, [FromBody] AppointmentDTO appointmentDTO)
        {
            try
            {
                var (userId, role) = GetUserInfo();
                var updatedAppointment = _appointmentManager.UpdateAppointment(id, appointmentDTO, userId, role);
                
                if (updatedAppointment == null)
                    return NotFound("Appointment not found.");

                return Ok(updatedAppointment);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}

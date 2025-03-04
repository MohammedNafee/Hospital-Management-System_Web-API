using HMS_Phase1.Management_Classes;
using HMS_WebAPI.DTOs;
using HMS_WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_WebAPI.Controllers
{
    [Route("api/[Controller]/[action]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentManager _appointmentManager;
        private readonly UserInfoService _userInfoService;

        public AppointmentController(AppointmentManager appointmentManager, UserInfoService userInfoService)
        {
            _appointmentManager = appointmentManager;
            _userInfoService = userInfoService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult ScheduleAppointment([FromBody] AppointmentDTO appointmentDTO)
        {
            try
            {
                var (userId, role) = _userInfoService.GetUserInfo();
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
                var (userId, role) = _userInfoService.GetUserInfo();
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
                var (userId, role) = _userInfoService.GetUserInfo();
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

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

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult ScheduleAppointment([FromBody] AppointmentDTO appointmentDTO)
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (!int.TryParse(loggedInUserId, out int userId)) 
                return Forbid();
            
            if (appointmentDTO == null)
                return BadRequest("Invalid appointment data");

            if (User.IsInRole("Patient") && appointmentDTO.PatientId != userId)
                return Forbid("Patients can only Schedule their own Appointments.");

            if (User.IsInRole("Doctor") && appointmentDTO.DoctorId != userId)
                return Forbid("Doctors can only Schedule their own Appointments.");

            var appointment = new Appointment(
                appointmentDTO.AppointmentDate,
                appointmentDTO.PatientId,
                appointmentDTO.DoctorId
            );
         

            _appointmentManager.ScheduleAppointment(appointment);
            return Created();
        }

        [HttpGet]
        public IActionResult GetAppointmets([FromQuery] int? patientId, [FromQuery] int? doctorId)
        {
            if (patientId.HasValue)
            {
                var appointments = _appointmentManager.GetAppointmentsByPatientId(patientId.Value);
                if (appointments.Count == 0)
                    return NotFound("No appointments found for this patient.");
                
                return Ok(appointments);
            }

            if (doctorId.HasValue)
            {
                var appointments = _appointmentManager.GetAppointmentsByDoctorId(doctorId.Value);
                if (appointments.Count == 0)
                    return NotFound("No appointments found for this doctor.");
                
                return Ok(appointments);
            }

            return BadRequest("Please provide either a patientId or doctorId.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult CancelAppointment(int id)
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                return Forbid();

            var appointment = _appointmentManager.GetAppointmentById(id);
            if (appointment == null)
                return NotFound("Appointment Not Found!");

            if (User.IsInRole("Patient") && userId != appointment.PatientId)
                return Forbid("Patient can only cancel their own Appointments");

            if (User.IsInRole("Doctor") && userId != appointment.DoctorId)
                return Forbid("Doctor can only cancel their own Appointments");

            bool canceled = _appointmentManager.CancelAppointment(id);
            if (!canceled)
                return NotFound("Appointment does not exist");

            return Ok($"Appointment {id} canceled successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult UpdateAppointment(int id, [FromBody] AppointmentDTO appointmentDTO)
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(loggedInUserId, out int userId))
                return Forbid();

            var appointment = _appointmentManager.GetAppointmentById(id);
            if (appointment == null)
                return NotFound("Appointment Not Found!");

            if (User.IsInRole("Doctor") && userId != appointment.DoctorId)
                return Forbid("Doctor can only update their own Appointments.");

            var updatedAppointment = _appointmentManager.UpdateAppointment(id, appointmentDTO);
            if (updatedAppointment == null)
                return NotFound("Appointment not found.");
            return Ok(updatedAppointment);
        }
    }
}

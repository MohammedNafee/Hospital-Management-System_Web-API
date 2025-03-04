using HMS_Phase1;
using HMS_Phase1.Management_Classes;
using HMS_WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_WebAPI.Controllers
{
    [Route("api/[Controller]/[action]")]
    public class BillingController : ControllerBase
    {
        private readonly BillingManager _billingManager;

        private readonly UserInfoService _userInfoService;

        public BillingController(BillingManager billingManager, UserInfoService userInfoService)
        {
            _billingManager = billingManager;
            _userInfoService = userInfoService;
        }

        [HttpPost]
        public IActionResult GenerateBill([FromBody] PrescriptionEventArgs e)
        {
            try
            {
                _billingManager.GenerateBill(e);
                return Created();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while generating the bill");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetBillsByPatientId([FromQuery] int? patientId)
        {
            var (userId, role) = _userInfoService.GetUserInfo();

            if (role == "Patient")
            {
                patientId = userId;
            }

            if (patientId == null)
                return BadRequest("Patient ID is required.");

            var bills = _billingManager.GetBillsByPatientId(patientId.Value);
            if (!bills.Any())
                return NotFound($"No bills found for patient: {patientId}.");

            return Ok(bills);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetAllBills()
        {
            var (userId, role) = _userInfoService.GetUserInfo();

            if (role == "Patient")
            {
                var bills = _billingManager.GetBillsByPatientId(userId);
                if (!bills.Any())
                    return NotFound($"No bills available for patient: {userId}");
                
                return Ok(bills);
            }
  
            var allBills = _billingManager.GetAllBills();
            if (!allBills.Any())
                return NotFound("No bills available");

            return Ok(allBills);
        }

        [HttpPut("{billId}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult UpdateBillStatus(int billId)
        {
            var bill = _billingManager.UpdateBillStatus(billId);

            if (bill == null)
                return NotFound("Bill not found.");

            return Ok(bill);    
        }

    }
}

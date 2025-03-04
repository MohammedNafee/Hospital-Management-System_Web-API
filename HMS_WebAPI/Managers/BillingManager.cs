using HMS_Phase1.Entities;
using HMS_WebAPI.DbAccess;

namespace HMS_Phase1.Management_Classes
{
    public class BillingManager
    {
        private readonly BillingRepository _billingRepository;

        public BillingManager(BillingRepository billingRepository)
        {
           _billingRepository = billingRepository;
        }

        public void GenerateBill(PrescriptionEventArgs e)
        {
            if (e == null)
                throw new ArgumentException("Invalid prescription data");

            var medication = _billingRepository.GetMedicationById(e.MedicationId);
            if (medication == null)
                throw new InvalidOperationException("Medication not found");

            decimal totalAmount = medication.Price;

            var bill = new Bill(e.PrescriptionId, totalAmount, DateTime.Now, BillStatus.Unpaid);

            _billingRepository.AddBill(bill);
        }

        public List<Bill> GetBillsByPatientId(int patientId)
        {
            return _billingRepository.GetBillsByPatientId(patientId);
        }

        public List<Bill> GetAllBills()
        {
            return _billingRepository.GetAllBills();
        }

        public Bill? UpdateBillStatus(int billId)
        {
            var bill = _billingRepository.GetBillById(billId);
            if (bill == null) return null;

            if (bill.Status == BillStatus.Unpaid)
                bill.Status = BillStatus.Paid;
            else
                bill.Status = BillStatus.Unpaid;
            
          _billingRepository.UpdateBill(bill);
            return bill;
        }
    }
}

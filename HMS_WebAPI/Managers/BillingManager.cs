using HMS_Phase1.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Phase1.Management_Classes
{
    public class BillingManager
    {
        private readonly HMSContext _context;

        public BillingManager(HMSContext context)
        {
            _context = context;
        }

        public void GenerateBill(PrescriptionEventArgs e)
        {
            var medication = _context.Medications.FirstOrDefault(med => med.MedicationId == e.MedicationId);
            if (medication == null)
                throw new InvalidOperationException("Medication not found");

            decimal totalAmount = medication.Price;

            var bill = new Bill(e.PrescriptionId, totalAmount, DateTime.Now, BillStatus.Unpaid);
            _context.Bills.Add(bill);
            _context.SaveChanges();
        }

        public List<Bill> GetBillsByPatientId(int patientId)
        {
            return _context.Bills.Where(b => b.Prescription.PatientId == patientId).ToList();
        }

        public List<Bill> GetAllBills()
        {
            return _context.Bills.ToList();
        }

        public Bill? UpdateBillStatus(int billId)
        {
            var bill = _context.Bills.SingleOrDefault(b => b.BillId == billId);
            if (bill == null) return null;

            if (bill.Status == BillStatus.Unpaid)
                bill.Status = BillStatus.Paid;
            else
                bill.Status = BillStatus.Unpaid;
            
            _context.SaveChanges();
            return bill;
        }
    }
}

using HMS_Phase1;
using HMS_Phase1.Entities;

namespace HMS_WebAPI.DbAccess
{
    public class BillingRepository
    {
        private readonly HMSContext _context;

        public BillingRepository(HMSContext context)
        {
            _context = context;
        }

        public Medication? GetMedicationById(int medicationId)
        {
            return _context.Medications.FirstOrDefault(m => m.MedicationId == medicationId);
        }

        public void AddBill(Bill bill)
        {
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

        public Bill? GetBillById(int billId)
        {
            return _context.Bills.SingleOrDefault(b => b.BillId == billId);
        }

        public void UpdateBill(Bill bill)
        {
            _context.Bills.Update(bill);
            _context.SaveChanges();
        }
    }
}

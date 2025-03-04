using HMS_Phase1;
using HMS_Phase1.Entities;

namespace HMS_WebAPI.DbAccess
{
     public class DoctorRepository
     {
         private readonly HMSContext _context;

         public DoctorRepository(HMSContext context)
         {
             _context = context;
         }

         public void AddDoctor(Doctor doctor)
         {
             _context.Doctors.Add(doctor);
             _context.SaveChanges();
         }

         public List<Doctor> GetAllDoctors()
         {
             return _context.Doctors.ToList();
         }

         public Doctor? GetDoctorById(int doctorId)
         {
             return _context.Doctors.SingleOrDefault(d => d.DoctorId == doctorId);
         }

         public void UpdateDoctor(Doctor doctor)
         {
             _context.Doctors.Update(doctor);
             _context.SaveChanges();
         }

         public bool DeleteDoctor(Doctor doctor)
         {
             _context.Doctors.Remove(doctor);
             return _context.SaveChanges() > 0;
         }

         public bool HasDependencies(int doctorId)
         {
             return _context.Appointments.Any(a => a.DoctorId == doctorId) ||
                    _context.Prescriptions.Any(p => p.DoctorId == doctorId);
         }
     }
}

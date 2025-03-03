using Microsoft.EntityFrameworkCore;
using HMS_Phase1.Entities;
using Microsoft.Extensions.Options;
using HMS_WebAPI.DbAccess;

namespace HMS_Phase1
{
    public class HMSContext : DbContext 
    {
        public HMSContext(DbContextOptions<HMSContext> dbContext) 
        : base (dbContext)
        {
            
        }

        public DbSet<Patient> Patients {  get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<PrescriptionMedication> PrescriptionMedications { get; set; }
        public DbSet<Bill> Bills { get; set; }  
        public DbSet<SysUser> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=DESKTOP-7E8QNOQ;Initial Catalog=HMS;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region patient-relations
            
            // A patient can have many appointments.
            modelBuilder.Entity<Appointment>()
                .HasOne(p => p.Patient)
                .WithMany(x => x.Appointments)
                .HasForeignKey(f => f.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // A patient can have many prescriptions.
            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany(x =>x.Prescriptions)
                .HasForeignKey(f => f.PatientId)
                .OnDelete(DeleteBehavior.Cascade);


            #endregion

            ////////////////////////////////////////

            #region doctor-relations

            // A doctor can have many appointments.
            modelBuilder.Entity<Appointment>()
                .HasOne(d => d.Doctor)
                .WithMany(a => a.Appointments)
                .HasForeignKey(f => f.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // A doctor can issue many prescriptions.
            modelBuilder.Entity<Prescription>()
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(f => f.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            ////////////////////////////////////////

            #region medication-prescription-relations

            // A prescription can have many medications.
            // A medication can have many prescriptions.

            /**
             * The relationship between Prescription and Medication results in a many-to-many relationship 
             * because a single prescription can include multiple medications, 
             * and a single medication can be part of multiple prescriptions. 
             * This arises from the fact that both Prescription and Medication have a one-to-many relationship with each other from opposite sides.
             * 
             */

            modelBuilder.Entity<PrescriptionMedication>()
                .HasOne(pre => pre.Prescription)
                .WithMany(med => med.Medications)
                .HasForeignKey(f => f.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrescriptionMedication>()
                .HasOne(med => med.Medication)
                .WithMany(pre => pre.Prescriptions)
                .HasForeignKey(f => f.MedicationId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            ////////////////////////////////////////

            #region bill-prescription-relation

            // A bill is associated with one prescription.
            modelBuilder.Entity<Bill>()
                .HasOne(pre => pre.Prescription)
                .WithOne(b => b.Bill)
                .HasForeignKey<Bill>(f => f.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade); 
            #endregion

            ///////////////////////////////////////
 
        }
    }
}

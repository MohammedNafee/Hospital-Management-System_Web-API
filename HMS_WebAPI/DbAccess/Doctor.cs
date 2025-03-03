
namespace HMS_Phase1.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        public Doctor(string name, int age, string gender, string contactNumber, string email, string specialty)
        {
            Name = name;
            Age = age;
            Gender = gender;
            ContactNumber = contactNumber;
            Email = email;
            Specialty = specialty;
        }

        public override string ToString()
        {
            return $"- Doctor {DoctorId} => \n" +
                   $"   * Name: {Name} \n" +
                   $"   * Age: {Age} \n" +
                   $"   * Gender: {Gender} \n" +
                   $"   * Contact Number: {ContactNumber} \n" +
                   $"   * Email: {Email} \n" +
                   $"   * Specialty: {Specialty}";
        }
    }
}

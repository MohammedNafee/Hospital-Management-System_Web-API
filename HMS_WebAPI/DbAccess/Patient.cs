namespace HMS_Phase1.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public List<Bill> Bills { get; set; } = new List<Bill>();

        public Patient(string name, int age, string gender, string contactNumber, string address)
        {
            Name = name;
            Age = age;
            Gender = gender;
            ContactNumber = contactNumber;
            Address = address;
        }

        public override string ToString()
        {
            return $"- Patient {PatientId} => \n" +
                   $"   * Name: {Name} \n" +
                   $"   * Age: {Age} \n" +
                   $"   * Gender: {Gender} \n" +
                   $"   * Contact Number: {ContactNumber} \n" +
                   $"   * Address: {Address}";  
        }
    }
}

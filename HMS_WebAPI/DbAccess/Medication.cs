
using Azure;
using System.Reflection;

namespace HMS_Phase1.Entities
{
    public class Medication
    {
        public int MedicationId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public List<PrescriptionMedication> Prescriptions { get; set; } = new List<PrescriptionMedication>();

        public Medication(string name, int quantity, decimal price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }

        public override string ToString()
        {
            return $"- Medication {MedicationId} => \n" +
                   $"   * Name: {Name} \n" +
                   $"   * Quantity: {Quantity} \n" +
                   $"   * Price: {Price}";
        }
    }
}

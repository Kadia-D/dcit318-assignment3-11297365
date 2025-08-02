using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareSystem
{
    // a. Generic repository
    public class Repository<T>
    {
        private List<T> items = new();

        public void Add(T item) => items.Add(item);

        public List<T> GetAll() => new(items);

        public T? GetById(Func<T, bool> predicate) =>
            items.FirstOrDefault(predicate);

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // b. Patient class
    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString() =>
            $"[Patient] ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
    }

    // c. Prescription class
    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString() =>
            $"[Prescription] ID: {Id}, Medication: {MedicationName}, Date: {DateIssued.ToShortDateString()}, Patient ID: {PatientId}";
    }

    // d-g. Main app
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo = new();
        private Repository<Prescription> _prescriptionRepo = new();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new();

        // Seed 2–3 patients and 4–5 prescriptions
        public void SeedData()
        {
            _patientRepo.Add(new Patient(1, "Alicia Jones", 20, "Female"));
            _patientRepo.Add(new Patient(2, "Damson Idris", 29, "Male"));
            _patientRepo.Add(new Patient(3, "Max Emilian Verstappen ", 27, "Male"));

            _prescriptionRepo.Add(new Prescription(1, 1, "Amoxicillin", DateTime.Now.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(2, 2, "Ibuprofen", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(3, 1, "Nexium", DateTime.Now.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Nugel -O", DateTime.Now.AddDays(-2)));
            _prescriptionRepo.Add(new Prescription(5, 1, "Cetirizine", DateTime.Now.AddDays(-1)));
        }

        // Build the prescription map
        public void BuildPrescriptionMap()
        {
            var prescriptions = _prescriptionRepo.GetAll();

            _prescriptionMap = prescriptions
                .GroupBy(p => p.PatientId)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList()
                );
        }

        // Print all patients
        public void PrintAllPatients()
        {
            Console.WriteLine("----- All Patients -----");
            foreach (var patient in _patientRepo.GetAll())
                Console.WriteLine(patient);
        }

        // f. Get prescriptions by patient ID
        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            return _prescriptionMap.ContainsKey(patientId)
                ? _prescriptionMap[patientId]
                : new List<Prescription>();
        }

        // g. Print prescriptions for a patient
       public void PrintPrescriptionsForPatient(int id)
{
    if (_prescriptionMap.ContainsKey(id))
    {
        Console.WriteLine($"\nPrescriptions for Patient ID {id}:");
        foreach (var prescription in _prescriptionMap[id])
        {
            Console.WriteLine($"Prescription ID: {prescription.Id}, Medication: {prescription.MedicationName}, Date: {prescription.DateIssued}");
        }
    }
    else
    {
        Console.WriteLine($"No prescriptions found for Patient ID {id}.");
    }
}

    }

    class Program
    {
        static void Main(string[] args)
        {
            var app = new HealthSystemApp();
            app.SeedData();
            app.BuildPrescriptionMap();
            app.PrintAllPatients();

            Console.Write("\nEnter a patient ID to view prescriptions: ");
            if (int.TryParse(Console.ReadLine(), out int selectedId))
            {
                app.PrintPrescriptionsForPatient(selectedId);
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }
}

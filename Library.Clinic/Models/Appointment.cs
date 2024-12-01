using System;

namespace Library.Clinic.Models
{
    public class Appointment
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }
        public int Id { get; set; }
        public Patient? Patient { get; set; }
        public Physician? Physician { get; set; }
        public List<Patient>? AllPatients { get; set; }
        public List<Physician>? AllPhysicians { get; set; }
        public List<Treatment> AllTreatments { get; set; } 
        public int TotalWithout { get; set; }
        public int TotalWith {get; set; }


        public Appointment()
        {
            StartTime = DateTime.Now;
            EndTime = StartTime.AddMinutes(30);
            PatientId = 0;
            PhysicianId = 0;
            Id = 0;
            TotalWithout = 0;
            TotalWith = 0;
        }
    }
}

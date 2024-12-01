using System;

namespace Library.Clinic.Models
{
    public class Physician
    {
        public string Name { get; set; }
        public string LicenseNum { get; set; }
        public DateTime GradDate { get; set; }
        public string Specializations { get; set; }
        public bool Available { get; set; }
        public int Id { get; set; }

        public Physician()
        {
            Name = string.Empty;
            LicenseNum = string.Empty;
            GradDate = DateTime.MinValue;
            Specializations = string.Empty;
            Available = true;
            Id = 0;
        }
    }
}
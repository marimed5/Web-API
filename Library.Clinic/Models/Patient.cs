using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Clinic.Models
{
    public class Patient
    {
        public int Id { get; set; }
        private string? name;
        public string Name { get; set;}
        public DateTime Birthday {  get; set; }
        public string Address { get; set; }

        public string SSN { get; set; }

        public string InsuranceProvider { get; set; }
        public int MemberId { get; set; }
        public string PlanType { get; set; }
        public int CoPay { get; set; }
        public int Coverage { get; set; }


        public Patient()
        {
            Name = string.Empty;
            Address = string.Empty;
            Birthday = DateTime.MinValue;
            SSN = string.Empty;
            InsuranceProvider = string.Empty;
            MemberId = 0;
            PlanType = string.Empty;
        }
    }
}

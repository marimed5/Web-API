using Library.Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Clinic.Services
{
    public class PatientServiceProxy
    {
        private static object _lock = new object();
        private static PatientServiceProxy? instance;
        
        public static PatientServiceProxy Current
        {
            get
            {
                lock(_lock)
                {
                    if (instance == null)
                    {
                        instance = new PatientServiceProxy();
                    }
                }
                return instance;
            }
        }

        
        private PatientServiceProxy()
        {
            instance = null;


            Patients = new List<Patient>
            {  
                new Patient{Id = 1, Name = "John Doe", InsuranceProvider = "United", CoPay = 20, Coverage=80}
                , new Patient{Id = 2, Name = "Jane Doe", InsuranceProvider = "Blue Cross", CoPay = 15, Coverage=60}
            };
        }
        public int LastKey
        {
            get
            {
                if(Patients.Any())
                {
                    return Patients.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        private List<Patient>? patients; 
        public List<Patient> Patients { 
            get {
                return patients;
            }

            private set
            {
                if (patients != value)
                {
                    patients = value;
                }
            }
        }

        public void AddOrUpdatePatient(Patient patient)
        {
            bool isAdd = false;
            if (patient.Id <= 0)
            {
                patient.Id = LastKey + 1;
                isAdd = true;
            }

            if(isAdd)
            {
                Patients.Add(patient);
            }

        }

        public void DeletePatient(int id) {
            var patientToRemove = Patients.FirstOrDefault(p => p.Id == id);

            if (patientToRemove != null)
            {
                Patients.Remove(patientToRemove);
            }
        }
    }
}

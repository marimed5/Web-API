using System;
using Library.Clinic.Models;

namespace Api.Clinic.Database
{
    static public class FakeDatabase
    {
        private static List<Physician> physicians = new List<Physician>
        {
            new Physician{Name = "James Smith", LicenseNum = "099120", 
            GradDate = new DateTime(2018, 09, 30), Specializations = "Dermatology", 
            Available = true, Id = 1}
        };

        public static List<Physician> Physicians
        {
            get {
                return physicians;
            }
        }

        public static int LastKey
        {
            get
            {
                if(Physicians.Any())
                {
                    return Physicians.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public static Physician? AddOrUpdatePhysician(Physician physician)
        {
            if (physician == null)
            {
                return null;
            }
            bool isAdd = false;
            if (physician.Id <= 0)
            {
                physician.Id = LastKey + 1;
                isAdd = true;
            }

            if(isAdd)
            {
                Physicians.Add(physician);
            }

            return physician;
        }
    }    
}



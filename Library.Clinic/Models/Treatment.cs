using System;

namespace Library.Clinic.Models
{
    public class Treatment
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsSelected { get; set; }

        public Treatment(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}
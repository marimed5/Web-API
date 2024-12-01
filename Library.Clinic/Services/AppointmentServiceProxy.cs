using Library.Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Library.Clinic.Services
{
    public class AppointmentServiceProxy
    {
        private static object _lock = new object();
        private static AppointmentServiceProxy? instance;

        public static AppointmentServiceProxy Current
        {
            get
            {
                lock(_lock)
                {
                    if (instance == null)
                    {
                        instance = new AppointmentServiceProxy();
                    }
                }
                return instance;
            }
        }

        
        private AppointmentServiceProxy()
        {
            instance = null;
            Appointments = new List<Appointment>();
        }

        public List<Appointment> appointments = new List<Appointment>();
        public List<Appointment> Appointments { 
            get {
                return appointments;
            }

            private set
            {
                if (appointments != value)
                {
                    appointments = value;
                }
            }
        }


        public int LastKey
        {
            get
            {
                if(Appointments.Any())
                {
                    return Appointments.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public List<Treatment> AllTreatments = new List<Treatment>
        {
            new Treatment("Checkup", 100),
            new Treatment("Diagnostic Tests", 300),
            new Treatment("Therapy", 150),
            new Treatment("Surgery", 1000)
        };

        public string AddOrUpdateAppointment(Appointment appointment)
        {
            bool isAdd = false;
            if (appointment.Id <= 0)
            {
                isAdd = true;
            }
            

            if(isAdd)
            {
                if (IsPhysicianAvailable(appointment.PhysicianId, appointment.StartTime, appointment.EndTime, appointment.Id))
                {
                    appointment.Id = LastKey + 1;
                    appointments.Add(appointment);
                    return "Appointment Booked!";
                }
                else
                {
                    return "The physician is not available at chosen time, try again.";
                }
            }
            else
            {
                if (IsPhysicianAvailable(appointment.PhysicianId, appointment.StartTime, appointment.EndTime, appointment.Id))
                {
                    return "Appointment Booked!";
                }
                else
                {
                    return "The physician is not available at chosen time, try again.";
                }
            }

        }

        public void DeleteAppointment(int Id)
        {
            var appointmentToRemove = Appointments.FirstOrDefault(a => a.Id == Id);
            if (appointmentToRemove != null)
            {
                Appointments.Remove(appointmentToRemove);
            }
        }

        public bool IsPhysicianAvailable(int physicianId, DateTime proposedStartTime, DateTime proposedEndTime, int appointmentId)
        {
            if (!WorkingHours(proposedStartTime, proposedEndTime))
            {
                return false;
            }

            foreach (var appointment in appointments)
            {
                if (appointment.Id == appointmentId)
                {
                    continue;
                }
                if (appointment.PhysicianId == physicianId)
                {
                    if (HasBooking(appointment, proposedStartTime, proposedEndTime))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool WorkingHours(DateTime startTime, DateTime endTime)
        {
            if (startTime.DayOfWeek == DayOfWeek.Saturday || startTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            TimeSpan startOfBusiness = new TimeSpan(8, 0, 0);
            TimeSpan endOfBusiness = new TimeSpan(17, 0, 0);

            return startTime.TimeOfDay >= startOfBusiness && endTime.TimeOfDay <= endOfBusiness;
        }

        private bool HasBooking(Appointment existingAppointment, DateTime newStartTime, DateTime newEndTime)
        {
            return newStartTime < existingAppointment.EndTime && newEndTime > existingAppointment.StartTime;
        }
    }
}


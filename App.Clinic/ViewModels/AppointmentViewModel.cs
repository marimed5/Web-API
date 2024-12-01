using Library.Clinic.Models;
using Library.Clinic.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace App.Clinic.ViewModels 
{
    public class AppointmentViewModel
    {
        private Appointment? Model { get; set; }
        public ICommand? DeleteCommand { get; set; }
        public ICommand? EditCommand { get; set; }

        public int Id
        {
            get
            {
                if(Model == null)
                {
                    return -1;
                }

                return Model.Id;
            }

            set
            {
                if(Model != null && Model.Id != value) {
                    Model.Id = value;
                }
            }
        }

        public int PatientId
        {
            get => Model?.PatientId ?? -1;
            set
            {
                if(Model != null)
                {
                    Model.PatientId= value;
                    Model.Patient = PatientServiceProxy.Current.Patients.FirstOrDefault(p => p.Id == value);
                }
            }
        }

        public string PatientName
        {
            get
            {
                if (Model != null && Model.PatientId > 0)
                {
                    if (Model.Patient == null)
                    {
                        Model.Patient = PatientServiceProxy
                            .Current
                            .Patients
                            .FirstOrDefault(p => p.Id == Model.PatientId);
                    } 
                }
                return Model?.Patient?.Name ?? string.Empty;
            }
        }

        public List<Patient> AllPatients
        {
            get
            {
                if (PatientServiceProxy.Current?.Patients != null)
                {
                    return new List<Patient>(PatientServiceProxy.Current.Patients);
                }
                return new List<Patient>();
            }
            set
            {
                Model.AllPatients = value;
            }
        }

        public int PhysicianId
        {
            get => Model?.PhysicianId?? -1;
            set
            {
                if (Model != null)
                {
                    Model.PhysicianId= value;
                    Model.Physician = PhysicianServiceProxy.Current.Physicians.FirstOrDefault(p => p.Id == value);
                }
            }
        }

        public string PhysicianName
        {
            get
            {
                return Model?.Physician?.Name ?? string.Empty;
            }
        }

        public List<Physician> AllPhysicians
        {
            get
            {
                if (PhysicianServiceProxy.Current?.Physicians != null)
                {
                    return new List<Physician>(PhysicianServiceProxy.Current.Physicians);
                }
                return new List<Physician>();
            }
            set
            {
                Model.AllPhysicians = value;
            }
        }

        public DateTime StartTime
        {
            get => Model?.StartTime ?? DateTime.MinValue;
            set
            {
                if (Model!= null)
                {
                    Model.StartTime = value;
                }
            }
        }

        public DateTime EndTime
        {
            get => Model?.EndTime ?? DateTime.MinValue;
            set
            {
                if (Model!= null)
                {
                    Model.EndTime = Model.StartTime.AddMinutes(30);
                }
            }
        }

        public string InsuranceProvider
        {
            get
            {
                return Model?.Patient?.InsuranceProvider ?? string.Empty;
            }
        }

        public List<Treatment> AllTreatments
        {
            get
            {
                return new List<Treatment>(AppointmentServiceProxy.Current.AllTreatments);
            }
        }

        public void OnCheckBoxCheckedChanged(Treatment treatment, bool isChecked)
        {
            treatment.IsSelected = isChecked;
            if (treatment.IsSelected == true)
            {
               Model.TotalWithout += treatment.Price; 
            }
            
        }

        public int TotalWithout
        {
            get => Model.TotalWithout;
        }

        public int TotalWith
        {
            get => Model.TotalWithout - (TotalWithout * Model.Patient.Coverage / 100) + Model.Patient.CoPay ;
        }

        public void SetupCommands()
        {
            DeleteCommand = new Command(DoDelete);
            EditCommand = new Command((p) => DoEdit(p as AppointmentViewModel));
        }

        private void DoDelete()
        {
            if (Id > 0)
            {
                AppointmentServiceProxy.Current.DeleteAppointment(Id);
                Shell.Current.GoToAsync("//Appointments");
            }
        }

        private void DoEdit(AppointmentViewModel? avm)
        {
            if (avm == null)
            {
                return;
            }

            var selectedAppointmentId = avm?.Id ?? 0;
            Shell.Current.GoToAsync($"//AppointmentDetails?appointmentId={selectedAppointmentId}");
        }

        public AppointmentViewModel()
        {
            Model = new Appointment();
            SetupCommands();
        }

        public AppointmentViewModel(Appointment? model)
        {
            Model = model;
            if (Model != null)
            {
                PatientId = Model.PatientId;
                PhysicianId = Model.PhysicianId;
            }
            SetupCommands();
        }

        public void ExecuteAdd()
        {
            if (Model != null)
            {
                Model.EndTime = Model.StartTime.AddMinutes(30);
                string result = AppointmentServiceProxy.Current.AddOrUpdateAppointment(Model);
                Shell.Current.DisplayAlert("Appointment Status", result, "OK");
                if (result == "The physician is not available at chosen time, try again.")
                {
                    return;
                }
                if (result == "Appointment Booked!")
                {
                    Shell.Current.GoToAsync("//Appointments");
                }
                Shell.Current.GoToAsync("//Appointments");
            }
            
        }
    }
}

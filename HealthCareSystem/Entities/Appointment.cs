using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareSystem.Entities
{
    // Join entity with payload (AppointmentDate)
    public class Appointment
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public DateTime AppointmentDate { get; set; }
    }
}

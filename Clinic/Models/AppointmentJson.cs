using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class AppointmentJson
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Name { get; set; }

        public Boolean Status { get; set; }
        public string Prescription { get; set; }
        
        public int PatientID { get; set; }
    }
}

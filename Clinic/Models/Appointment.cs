using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class Appointment
    {
        public enum type
        {
            Examination,
            consultation
        }
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public Boolean Status { get; set; }
        public type Type { get; set; }
        public string Prescription { get; set; }
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
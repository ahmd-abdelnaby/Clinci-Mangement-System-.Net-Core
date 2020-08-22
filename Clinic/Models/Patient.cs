using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class Patient
    {
        public enum gender
        {
            Male,
            Female
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public gender Gender { get; set; }
        public int Age { get; set; }
        public string Image { get; set; }
        public string Mobile { get; set; }
        public string TobaccoUsage { get; set; }
        public string Alchol { get; set; }
        public string History { get; set; }

    }
}
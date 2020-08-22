using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class Disease
    {
        public int ID { get; set; }
        [Display(Name ="Disease")]
        public string diseaseName { get; set; }

        public string Symptoms { get; set; }
        public string Reference { get; set; }

    }
}
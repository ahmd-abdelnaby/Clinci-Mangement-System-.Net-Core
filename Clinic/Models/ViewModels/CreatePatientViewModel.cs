using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models.ViewModels
{
    public class CreatePatientViewModel
    {
        public enum gender
        {
            Male,
            Female
        }
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public gender Gender { get; set; }
        public int Age { get; set; }
        public IFormFile Image { get; set; }
        [Required]
        public string Mobile { get; set; }
        public string TobaccoUsage { get; set; }
        public string Alchol { get; set; }
        public string History { get; set; }

    }
}


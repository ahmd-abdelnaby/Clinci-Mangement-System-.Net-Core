using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models.ViewModels
{
    public class CreateRecordViewModel
    {
        public int PatientID { get; set; }
        public IFormFile Document { get; set; }
        public string Description { get; set; }

    }
}

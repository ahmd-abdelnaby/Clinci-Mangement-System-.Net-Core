using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinic.Data;
using Clinic.Models;
using Clinic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Clinic.Models.ViewModels;
using System.IO;
using static Clinic.Models.Patient;

namespace Clinic.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IService<Patient> patientservice;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly RecordsServices recordeService;
        private readonly AppointmentServices appointmentsServices;

        public PatientsController(ApplicationDbContext context, IService<Patient> Patientservice
            , IHostingEnvironment hostingEnvironment,RecordsServices RecordeService
            ,AppointmentServices appointmentsServices)
        {
            _context = context;
            patientservice = Patientservice;
            this.hostingEnvironment = hostingEnvironment;
            recordeService = RecordeService;
            this.appointmentsServices = appointmentsServices;
        }

        // GET: Patients
        public IActionResult Index()
        {
            return View(patientservice.GetAll());
        }

        // GET: Patients/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = patientservice.GetOne(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePatientViewModel model)
        {

            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Image != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                   
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Patient patient = new Patient
                {
                    Name = model.Name,
                    Age = model.Age,
                    Alchol = model.Alchol,
                    Gender = (gender)model.Gender,
                    History = model.History,
                    Mobile=model.Mobile,
                    TobaccoUsage=model.TobaccoUsage,
                    Image = uniqueFileName
                };

                patientservice.insert(patient);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }




        //if (ModelState.IsValid)
        //{
        //    patientservice.insert(patient);
        //    return RedirectToAction(nameof(Index));
        //}
        //return View(patient);
    

        // GET: Patients/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = patientservice.GetOne(id);
            CreatePatientViewModel model = new CreatePatientViewModel
            {
                ID=patient.ID,
                Name = patient.Name,
                Age = patient.Age,
                Alchol = patient.Alchol,
                History = patient.History,
                Mobile = patient.Mobile,
                TobaccoUsage = patient.TobaccoUsage,
            };
            ViewBag.gender = patient.Gender;
            ViewBag.image = patient.Image;
            if (patient == null)
            {
                return NotFound();
            }
            ViewBag.records = recordeService.GetPatientRecords(patient.ID);
            ViewBag.appointments = appointmentsServices.GetPatientAppointments(patient.ID);
            return View(model);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CreatePatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Image != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Patient patient = new Patient
                {
                    Name = model.Name,
                    Age = model.Age,
                    Alchol = model.Alchol,
                    Gender = (gender)model.Gender,
                    History = model.History,
                    Mobile = model.Mobile,
                    TobaccoUsage = model.TobaccoUsage,
                    Image = uniqueFileName
                };

                patientservice.Update(patient,id);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Patients/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = patientservice.GetOne(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            patientservice.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }
        
    }
}

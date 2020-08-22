using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Clinic.Models;
using Clinic.Models.ViewModels;
using Clinic.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class RecordsController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly RecordsServices recordeService;
        private readonly IService<Patient> patientservice;

        public RecordsController( IHostingEnvironment hostingEnvironment, RecordsServices RecordeService
            , IService<Patient> Patientservice)
        {
            this.hostingEnvironment = hostingEnvironment;
            recordeService = RecordeService;
            patientservice = Patientservice;
        }
        public IActionResult Index()
        {
            return View(recordeService.GetAll());
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            ViewBag.ID = id;
            ViewBag.PatientName = patientservice.GetOne(id).Name;
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Document != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Document.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    model.Document.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Record record = new Record
                {
                    PatientID = model.PatientID,
                    Description = model.Description,
                    Document = uniqueFileName
                };

                recordeService.insert(record);
                return RedirectToAction("Edit", "Patients", new { id = record.PatientID });
            }

            return View();
        }
        [HttpGet]
        public IActionResult AddRecord()
        {
            ViewBag.Patients = patientservice.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult AddRecord(CreateRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Document != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Document.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    model.Document.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Record record = new Record
                {
                    PatientID = model.PatientID,
                    Description = model.Description,
                    Document = uniqueFileName
                };

                recordeService.insert(record);
                return RedirectToAction("Edit", "Patients", new { id = record.PatientID });
            }

            return View();
        }

        public void DownloadDocument()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri("https://localhost:44352/images/59c6c6e4-1c12-408a-9b9b-d518127780a8_m1.jpg??%27noimage.png%27&v=-OjQeJYyHj-R8B83iILDnZ-FGtnmUQ42SmBzTY9CtCY"), @"c:\temp\image35.png");   
            }
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = recordeService.GetOne(id);
            if (record == null)
            {
                return NotFound();
            }
            CreateRecordViewModel model = new CreateRecordViewModel
            {
                Description = record.Description,
                PatientID = record.PatientID
            };
            ViewBag.Document = record.Document;
            return View(model);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Document != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Document.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    model.Document.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Record record = new Record()
                {
                    Description = model.Description,
                    PatientID = model.PatientID,
                    Document = uniqueFileName
                };
                try
                {
                    recordeService.Update(record,id);
                    
                }
                catch 
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
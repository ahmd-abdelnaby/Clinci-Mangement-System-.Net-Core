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

namespace Clinic.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IService<Patient> patientservice;
        private readonly IService<Appointment> appointmetsServices;

        public AppointmentsController(ApplicationDbContext context, IService<Patient> Patientservice
            , IService<Appointment> appointmetsServices)
        {
            _context = context;
            patientservice = Patientservice;
            this.appointmetsServices = appointmetsServices;
        }

        // GET: Appointments
        public  IActionResult Index()
        {
            return View(appointmetsServices.GetAll());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewBag.Patients = patientservice.GetAll();
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.Status = true;
                appointmetsServices.insert(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Patients = patientservice.GetAll();
            return View(appointment);
        }
        [HttpPost]
        public async Task<IActionResult> CreateJson(int PatientID,int Type,string Prescription,int year,int month,int day)
        {
            Appointment appointment = new Appointment
            {
                PatientID = PatientID,
                Status = true,
                Date= new DateTime(year, month, day),
                Prescription=Prescription
            };
            if (Type == 0)
                appointment.Type = Appointment.type.Examination;
            else
                appointment.Type = Appointment.type.consultation;

            appointmetsServices.insert(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Date,Status,Prescription")] Appointment appointment)
        {
            if (id != appointment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.ID == id);
        }

        [HttpGet]
        public IActionResult AddAppointment(int? id)
        {
            ViewBag.ID = id;
            ViewBag.PatientName = patientservice.GetOne(id).Name;
            return View();
        }
        [HttpPost]
        public IActionResult AddAppointment(Appointment model)
        {
            if (ModelState.IsValid)
            {
                Appointment app = new Appointment
                {
                    PatientID = model.PatientID,
                    Date = model.Date,
                    Prescription = model.Prescription,
                    Status =true,
                    Type = model.Type,
                    Patient = model.Patient
                };
                model.Status = true;
                appointmetsServices.insert(app);
                return RedirectToAction("Edit", "Patients", new { id = app.PatientID });
            }

            return View();
        }
        public List<AppointmentJson> AllAppointments()
        {
            List<AppointmentJson> appsJson = new List<AppointmentJson>();
            AppointmentJson json;
            var appos= _context.Appointments.Include(a=>a.Patient).ToList();
            foreach (var item in appos)
            {
                json = new AppointmentJson
                {
                    Date = item.Date,
                    Day = item.Date.Day,
                    Month = item.Date.Month,
                    Year = item.Date.Year,
                    PatientID = item.PatientID,
                    ID = item.ID,
                    Prescription = item.Prescription,
                    Status = item.Status,
                    Name = item.Patient.Name
                };
                appsJson.Add(json);
            }
            return appsJson;
        }
        public List<AppointmentJson> appointmentsByDate(int month, int day)
        {
            List<AppointmentJson> appsJson = new List<AppointmentJson>();
            AppointmentJson json;
            var appos = _context.Appointments.Where(r => r.Date.Month == month && r.Date.Day == day).Include(a => a.Patient).ToList();
            foreach (var item in appos)
            {
                json = new AppointmentJson
                {
                    Date = item.Date,
                    Day = item.Date.Day,
                    Month = item.Date.Month,
                    Year = item.Date.Year,
                    PatientID = item.PatientID,
                    ID = item.ID,
                    Prescription = item.Prescription,
                    Status = item.Status,
                    Name=item.Patient.Name
                };
                appsJson.Add(json);
            }
            return appsJson;
        }
        public IActionResult Calendar()
        {
            ViewBag.Patients = patientservice.GetAll();
            return View();
        }
    }
}


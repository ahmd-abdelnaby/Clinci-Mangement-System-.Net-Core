using Clinic.Data;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Services
{
    public class AppointmentServices : IService<Appointment>
    {
        private readonly ApplicationDbContext _context;
        public AppointmentServices(ApplicationDbContext context)
        {
            this._context = context;
        }
        public void Delete(int id)
        {
            _context.Appointments.Remove(_context.Appointments.FirstOrDefault(p => p.ID == id));
            _context.SaveChanges();
        }

        public List<Appointment> GetAll()
        {
            return _context.Appointments.Include(a => a.Patient).ToList();
        }

        public Appointment GetOne(int? id)
        {
            return _context.Appointments.FirstOrDefault(p => p.ID == id);
        }

        public void insert(Appointment model)
        {
            _context.Appointments.Add(model);
            _context.SaveChanges();
        }

        public void Update(Appointment model, int id)
        {
            var Appointment = _context.Appointments.FirstOrDefault(p => p.ID == id);
            Appointment.Patient = model.Patient;
            Appointment.Prescription = model.Prescription;
            Appointment.Status = model.Status;
            Appointment.Date = model.Date;
            _context.SaveChanges();
        }
        public List<Appointment> GetPatientAppointments(int? id)
        {
            return _context.Appointments.Where(r => r.Patient.ID == id).ToList();
        }

        public List<Appointment> GettAppointmentsByDate(DateTime dateTime)
        {
            return _context.Appointments.Where(r => r.Date==dateTime).ToList();
        }
    }
}

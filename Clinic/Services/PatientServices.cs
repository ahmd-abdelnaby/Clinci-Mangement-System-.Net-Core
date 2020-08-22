using Clinic.Data;
using Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Services
{
    public class PatientServices : IService<Patient>
    {
        private readonly ApplicationDbContext _context;

        public PatientServices(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Delete(int id)
        {
            _context.Patients.Remove(_context.Patients.FirstOrDefault(p => p.ID == id));
            _context.SaveChanges();
        }

        public List<Patient> GetAll()
        {
            return _context.Patients.ToList();
        }

        public Patient GetOne(int? id)
        {

            var patient =  _context.Patients
                .FirstOrDefault(m => m.ID == id);
            return patient;
        }

        private Patient NotFound()
        {
            throw new NotImplementedException();
        }

        public void insert(Patient model)
        {
            _context.Patients.Add(model);
            _context.SaveChanges();
        }

        public void Update(Patient model, int id)
        {
            var pateint = _context.Patients.FirstOrDefault(p => p.ID == id);
            pateint.Name = model.Name;
            pateint.Mobile = model.Mobile;
            pateint.Image = model.Image;
            pateint.TobaccoUsage = model.TobaccoUsage;
            pateint.Age = model.Age;
            pateint.Alchol = model.Alchol;
            pateint.Gender = model.Gender;
            _context.SaveChanges();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }
    }
}

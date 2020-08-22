using Clinic.Data;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Services
{
    public class RecordsServices : IService<Record>
    {
        private readonly ApplicationDbContext _context;

        public RecordsServices(ApplicationDbContext context)
        {
            this._context = context;
        }
        public void Delete(int id)
        {
            _context.Records.Remove(_context.Records.FirstOrDefault(p => p.ID == id));
            _context.SaveChanges();
        }

        public List<Record> GetAll()
        {
            return _context.Records.Include(r=>r.Patient).ToList();
        }
        public List<Record> GetPatientRecords(int?id)
        {
            return _context.Records.Where(r=>r.Patient.ID==id).ToList();
        }

        public Record GetOne(int? id)
        {
            return _context.Records.FirstOrDefault(m => m.ID == id);
        }

        public void insert(Record model)
        {
            _context.Records.Add(model);
            _context.SaveChanges();
        }

        public void Update(Record model, int id)
        {
            var record = _context.Records.FirstOrDefault(p => p.ID == id);
            record.Document = model.Document;
            record.Patient = model.Patient;
            record.Document = model.Document;
            _context.SaveChanges();
        }
    }
}

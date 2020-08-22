using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Services
{
    public interface IService<Model>
    {
        List<Model> GetAll();
        Model GetOne(int? id);
        public void Update(Model model, int id);
        public void Delete(int id);
        public void insert(Model model);
    }
}

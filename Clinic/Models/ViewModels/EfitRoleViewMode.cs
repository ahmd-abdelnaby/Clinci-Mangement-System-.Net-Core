using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Views.Admonstrator
{
    public class EfitRoleViewMode
    {
        public EfitRoleViewMode()
        {
            Users = new List<string>();
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public List<string> Users  { get; set; }
    }
}

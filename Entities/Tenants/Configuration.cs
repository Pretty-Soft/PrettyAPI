using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Tenants
{
    public  class Configuration
    {
        public string DbProvider { get; set; } 
        public string Name { get; set; } 
        public string VendorId { get; set; } 
        public string ConnectionString { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Tenants
{
    public class Tenant
    {
        public string Name { get; set; } = default!;
        public string DbProvider { get; set; } = "MSSQL";  
        public long VendorId { get; set; }
        public string ConnectionString { get; set; } = "";
    }
}

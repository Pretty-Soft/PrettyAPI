using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Tenants
{
    public class Tenant: BaseEntity
    {
        public string Name { get; set; } = default!;
        public string DbProvider { get; set; } = "MSSQL";  
        public string ConnectionString { get; set; } = "";
        public string Host { get; set; } = "";
        public string SubDomain { get; set; } = "";
        public string Logo { get; set; } = "";
        public string ThemeColor { get; set; } = "";
       
    }
}

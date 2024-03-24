using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Tenants
{
    public class TenantSettings
    {
        public required Configuration Defaults { get; set; }
        public List<Tenant> Tenants { get; set; } = default!;
    }
}

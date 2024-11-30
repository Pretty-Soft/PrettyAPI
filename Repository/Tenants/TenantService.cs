using Entities.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace Repository.Tenants
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private HttpContext _httpContext;
        private Tenant _currentTenant;
        private Configuration configuration { get; set; }
        public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = contextAccessor.HttpContext;
            
            if (_httpContext != null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetTenant(Convert.ToInt64(tenantId));
                }
                else
                {
                    SetTenant(1);
                }
            }
        }

        private void SetTenant(long tenantId=1)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(a => a.TenantId == tenantId);

            if (_currentTenant == null || string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                _currentTenant = new Tenant();
                SetDefaultConnectionStringToCurrentTenant();
            }
        }

        private void SetDefaultConnectionStringToCurrentTenant()
        {
           _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
        }

        public string? GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Defaults?.DbProvider;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}

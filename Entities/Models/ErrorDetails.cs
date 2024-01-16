using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; } = default!;
        public string Message { get; set; } = default!;
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

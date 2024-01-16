using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Helper
{
    public class ApiResponse
    {
        public string status { get; set; } = "";
        public int statusCode { get; set; }
        public string message { get; set; } = "";
    }
    public class ApiResponseSuccess<T> : ApiResponse
    {
        public T data { get; set; }
    }
}

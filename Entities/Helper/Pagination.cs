using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Helper
{
    public class Pagination
    {
        [Range(0, 100)]
        public int Limit { get; set; } = 100;
        public int Skip { get; set; } = 0;
    }
}

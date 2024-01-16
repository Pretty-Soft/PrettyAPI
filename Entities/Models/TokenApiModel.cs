using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class TokenApiModel
    {
        public string? AccessToken { get; set; }= default!;
        public string? RefreshToken { get; set; }= default!;
    }
}

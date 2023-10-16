using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatablegenerator.Models
{
    public class DBCheckModel
    {
        public bool IsMSSQL { get; set; } = true;

        public bool IsMySQL { get; set; }

    }
}

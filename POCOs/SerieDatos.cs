using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCOs
{
    public class SerieDatos
    {
        public string NombreSerie { get; set; }
        public Dictionary<DateTime, decimal> Datos { get; set; }
    }
}

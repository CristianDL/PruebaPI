using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCOs
{
    public class ParametrosConsulta
    {
        public string[] CodigosBarras { get; set; }
        public DateTimeOffset FechaInicio { get; set; }
        public DateTimeOffset FechaFin { get; set; }
    }
}

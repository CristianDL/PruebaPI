using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCOs
{
    public class ActivoElectrico
    {
        public string CodigoMID { get; set; }
        public string Tag { get; set; }
        public List<SerieDatos> SeriesDatos { get; set; }
    }
}

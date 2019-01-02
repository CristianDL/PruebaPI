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
        public string WebId { get; set; }
        public List<SerieDatos> SeriesDatos { get; set; }

        public override string ToString()
        {
            StringBuilder datos = new StringBuilder();

            foreach (SerieDatos serie in SeriesDatos)
            {
                foreach (KeyValuePair<DateTime, double> item in serie.Datos)
                {
                    if (item.Key.Minute == 0 && item.Key.Second == 0 && item.Key.Millisecond == 0)
                    {
                        datos.Append(CodigoMID).Append(";").Append(serie.NombreSerie).Append(";").Append(item.Key.Date).Append(";").Append(item.Key.Hour + 1).Append(";").Append(item.Value).Append(Environment.NewLine);
                    }
                    else
                    {
                        datos.Append(CodigoMID).Append(";").Append(serie.NombreSerie).Append(";").Append(item.Key.Date).Append(";").Append(item.Key.TimeOfDay.ToString("g")).Append(";").Append(item.Value).Append(Environment.NewLine);
                    }
                }
            }

            return datos.ToString();
        }
    }
}

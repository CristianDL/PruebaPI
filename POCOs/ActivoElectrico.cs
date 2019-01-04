using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace POCOs
{
    [DataContract]
    public class ActivoElectrico
    {
        [DataMember]
        public string CodigoMID { get; set; }
        public string Tag { get; set; }
        public string WebId { get; set; }
        [DataMember]
        public List<SerieDatos> SeriesDatos { get; set; }

        public override string ToString()
        {
            StringBuilder datos = new StringBuilder();

            foreach (SerieDatos serie in SeriesDatos)
            {
                List<DateTime> fechas = serie.Datos.Keys.ToList();
                fechas.Sort();

                foreach (DateTime fecha in fechas)
                {
                    if (fecha.Minute == 0 && fecha.Second == 0 && fecha.Millisecond == 0 && !serie.NombreSerie.Equals(Variables.P.ToString()))
                    {
                        datos.Append(CodigoMID).Append(";").Append(serie.NombreSerie).Append(";").Append(fecha.Date).Append(";").Append(fecha.Hour + 1).Append(";").Append(serie.Datos[fecha]).Append(Environment.NewLine);
                    }
                    else
                    {
                        datos.Append(CodigoMID).Append(";").Append(serie.NombreSerie).Append(";").Append(fecha.Date).Append(";").Append(fecha.TimeOfDay.ToString("g")).Append(";").Append(serie.Datos[fecha]).Append(Environment.NewLine);
                    }
                }
            }

            return datos.ToString();
        }
    }
}

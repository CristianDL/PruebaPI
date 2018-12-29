using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json.Linq;
using POCOs;

namespace PIWebAPI
{
    public static class PIRequests
    {
        public static string GetAttributeWebId(string tag)
        {
            string webId = string.Empty;
            StringBuilder url = new StringBuilder();

            url.Append(ConfigurationManager.AppSettings["baseUrl"]);
            url.Append(Constants.AttributeController);
            url.Append(Constants.QueryStringStart);
            url.Append(Constants.PathParameter);
            url.Append(ConfigurationManager.AppSettings["baseTagPath"]);
            url.Append(tag);

            //using (PIWebAPIClient client = new PIWebAPIClient(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]))
            using (PIWebAPIClient client = new PIWebAPIClient())
            {
                Task<JObject> t = client.GetAsync(url.ToString());
                t.Wait();
                webId = t.Result[Constants.WebIdField].ToString();
            }

            return webId;
        }

        public static List<ActivoElectrico> GetRecordedDataAdHoc(List<ActivoElectrico> activos, DateTime startTime, DateTime endTime)
        {
            JArray items = null;
            StringBuilder url = new StringBuilder();

            url.Append(ConfigurationManager.AppSettings["baseUrl"]);
            url.Append(Constants.StreamSetController);
            url.Append(Constants.Slash);
            url.Append(Constants.RecordedDataLink);
            url.Append(Constants.QueryStringStart);
            for (int i = 0; i < activos.Count; i++)
            {
                ActivoElectrico temp = activos.ElementAt(i);
                url.Append(Constants.WebIdParameter);
                url.Append(temp.WebId);
                url.Append(Constants.QueryStringParameterDelimiter);
            }
            url.Append(Constants.StartTimeParameter);
            url.Append(startTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"));
            url.Append(Constants.QueryStringParameterDelimiter);
            url.Append(Constants.EndTimeParameter);
            url.Append(endTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"));

            //using (PIWebAPIClient client = new PIWebAPIClient(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]))
            using (PIWebAPIClient client = new PIWebAPIClient())
            {
                Task<JObject> t = client.GetAsync(url.ToString());
                t.Wait();
                items = (JArray) t.Result[Constants.ItemsField];
            }

            foreach (ActivoElectrico activo in activos)
            {
                JObject serieDatos = (JObject)items.Select(p => p[Constants.NameField].ToString().Equals(activo.Tag));
                JArray datos = (JArray) serieDatos[Constants.ItemsField];
                if (activo.SeriesDatos == null)
                {
                    activo.SeriesDatos = new List<SerieDatos>();
                }
                SerieDatos serie = new SerieDatos();
                serie.NombreSerie = Variables.P.ToString();
                serie.Datos = new Dictionary<DateTime, double>();
                foreach (var dato in datos)
                {
                    if (bool.Parse(dato[Constants.GoodField].ToString()))
                    {
                        serie.Datos.Add((DateTime)dato[Constants.TimestampField], (double)dato[Constants.ValueField]);
                    }
                }
                activo.SeriesDatos.Add(serie);
            }

            return activos;
        }
    }
}

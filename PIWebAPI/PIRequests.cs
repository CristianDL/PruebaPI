using POCOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
            using (PIWebAPIClient client = new PIWebAPIClient(url.ToString()))
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
            url.Append(startTime.ToString("u"));
            url.Append(Constants.QueryStringParameterDelimiter);
            url.Append(Constants.EndTimeParameter);
            url.Append(endTime.ToString("u"));

            //using (PIWebAPIClient client = new PIWebAPIClient(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]))
            using (PIWebAPIClient client = new PIWebAPIClient(url.ToString()))
            {
                Task<JObject> t = client.GetAsync(url.ToString());
                t.Wait();
                items = (JArray) t.Result[Constants.ItemsField];
            }

            foreach (ActivoElectrico activo in activos)
            {
                JObject serieDatos = (JObject)items.Where(p => p[Constants.NameField].ToString().Equals(activo.Tag)).First();
                JArray datos = (JArray) serieDatos[Constants.ItemsField];
                if (activo.SeriesDatos == null)
                {
                    activo.SeriesDatos = new List<SerieDatos>();
                }
                SerieDatos serie = new SerieDatos
                {
                    NombreSerie = Variables.P.ToString(),
                    Datos = new Dictionary<DateTime, double>()
                };
                foreach (var dato in datos)
                {
                    if (bool.Parse(dato[Constants.GoodField].ToString()) && !serie.Datos.ContainsKey((DateTime)dato[Constants.TimestampField]))
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

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
        public static async Task<string> GetAttributeWebIdAsync(string tag)
        {
            string webId = string.Empty;
            StringBuilder url = new StringBuilder();

            url.Append(ConfigurationManager.AppSettings["baseUrl"]);
            url.Append(Constants.AttributeController);
            url.Append(Constants.QueryStringStart);
            url.Append(Constants.PathParameter);
            url.Append(ConfigurationManager.AppSettings["baseTagPath"]);
            url.Append(tag);

            using (PIWebAPIClient client = new PIWebAPIClient(url.ToString()))
            {
                JObject obj = await client.GetAsync(url.ToString()).ConfigureAwait(false);
                webId = obj[Constants.WebIdField].ToString();
            }

            return webId;
        }

        public static async Task<List<ActivoElectrico>> GetRecordedDataAdHocAsync(List<ActivoElectrico> activos, DateTimeOffset startTime, DateTimeOffset endTime)
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
            url.Append(startTime.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            url.Append(Constants.QueryStringParameterDelimiter);
            url.Append(Constants.EndTimeParameter);
            url.Append(endTime.ToString("yyyy-MM-ddTHH:mm:sszzz"));

            using (PIWebAPIClient client = new PIWebAPIClient(url.ToString()))
            {
                JObject obj = await client.GetAsync(url.ToString()).ConfigureAwait(false);
                items = (JArray) obj[Constants.ItemsField];
            }

            foreach (ActivoElectrico activo in activos)
            {
                JObject serieDatos = (JObject)items.Where(p => p[Constants.NameField].ToString().Equals(activo.Tag)).First();
                JArray datos = (JArray)serieDatos[Constants.ItemsField];

                SerieDatos serie;
                if (activo.SeriesDatos.Count > 0 && activo.SeriesDatos.Exists(s => s.NombreSerie.Equals(Variables.P.ToString())))
                {
                    serie = activo.SeriesDatos.Find(s => s.NombreSerie.Equals(Variables.P.ToString()));
                }
                else
                {
                    serie = new SerieDatos
                    {
                        NombreSerie = Variables.P.ToString(),
                        Datos = new Dictionary<DateTimeOffset, double>()
                    };
                    activo.SeriesDatos.Add(serie);
                }

                foreach (var dato in datos)
                {
                    DateTimeOffset fecha = (DateTimeOffset)dato[Constants.TimestampField];
                    if (bool.Parse(dato[Constants.GoodField].ToString()) && !serie.Datos.ContainsKey(fecha.ToOffset(startTime.Offset)))
                    {
                        serie.Datos.Add(fecha.ToOffset(startTime.Offset), (double)dato[Constants.ValueField]);
                    }
                }
            }

            return activos;
        }

        public static async Task<List<ActivoElectrico>> GetPlotDataAdHocAsync(List<ActivoElectrico> activos, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            JArray items = null;
            StringBuilder url = new StringBuilder();

            url.Append(ConfigurationManager.AppSettings["baseUrl"]);
            url.Append(Constants.StreamSetController);
            url.Append(Constants.Slash);
            url.Append(Constants.PlotDataLink);
            url.Append(Constants.QueryStringStart);
            for (int i = 0; i < activos.Count; i++)
            {
                ActivoElectrico temp = activos.ElementAt(i);
                url.Append(Constants.WebIdParameter);
                url.Append(temp.WebId);
                url.Append(Constants.QueryStringParameterDelimiter);
            }
            url.Append(Constants.StartTimeParameter);
            url.Append(startTime.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            url.Append(Constants.QueryStringParameterDelimiter);
            url.Append(Constants.EndTimeParameter);
            url.Append(endTime.ToString("yyyy-MM-ddTHH:mm:sszzz"));

            using (PIWebAPIClient client = new PIWebAPIClient(url.ToString()))
            {
                JObject obj = await client.GetAsync(url.ToString()).ConfigureAwait(false);
                items = (JArray)obj[Constants.ItemsField];
            }

            foreach (ActivoElectrico activo in activos)
            {
                JObject serieDatos = (JObject)items.Where(p => p[Constants.NameField].ToString().Equals(activo.Tag)).First();
                JArray datos = (JArray)serieDatos[Constants.ItemsField];

                SerieDatos serie;
                if (activo.SeriesDatos.Count > 0 && activo.SeriesDatos.Exists(s => s.NombreSerie.Equals(Variables.P.ToString())))
                {
                    serie = activo.SeriesDatos.Find(s => s.NombreSerie.Equals(Variables.P.ToString()));
                }
                else
                {
                    serie = new SerieDatos
                    {
                        NombreSerie = Variables.P.ToString(),
                        Datos = new Dictionary<DateTimeOffset, double>()
                    };
                    activo.SeriesDatos.Add(serie);
                }

                foreach (var dato in datos)
                {
                    DateTimeOffset fecha = (DateTimeOffset)dato[Constants.TimestampField];
                    if (bool.Parse(dato[Constants.GoodField].ToString()) && !serie.Datos.ContainsKey(fecha.ToOffset(startTime.Offset)))
                    {
                        serie.Datos.Add(fecha.ToOffset(startTime.Offset), (double)dato[Constants.ValueField]);
                    }
                }
                activo.SeriesDatos.Add(serie);
            }

            return activos;
        }
    }
}

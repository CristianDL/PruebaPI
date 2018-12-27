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

            using (PIWebAPIClient client = new PIWebAPIClient(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]))
            {
                Task<JObject> result = client.GetAsync(url.ToString());
                result.Wait();
                webId = result.Result[Constants.WebIdField].ToString();
            }

            return webId;
        }

        public static Dictionary<DateTime, decimal> GetRecordedDataAdHoc(Dictionary<string, string> webIds, DateTime startTime, DateTime endTime)
        {
            Dictionary<DateTime, decimal> serie = new Dictionary<DateTime, decimal>();



            return serie;
        }
    }
}

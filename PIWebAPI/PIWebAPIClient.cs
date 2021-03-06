﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Configuration;

namespace PIWebAPI
{
    public class PIWebAPIClient : IDisposable
    {
        private HttpClient client;
        private bool disposed = false;

        public PIWebAPIClient(string url)
        {
            CredentialCache credentialCache = new CredentialCache
            {
                {
                    new Uri(url),
                    "Negotiate",
                    new NetworkCredential(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["domain"])
                }
            };

            HttpClientHandler handler = new HttpClientHandler
            {
                Credentials = credentialCache
            };
            client = new HttpClient(handler)
            {
                Timeout = new TimeSpan(0, 0, 10)
            };
        }

        public async Task<JObject> GetAsync(string uri)
        {
            int intentoActual = 0;
            HttpResponseMessage response =  new HttpResponseMessage(HttpStatusCode.BadRequest);
            string content = string.Empty;
            int maximoIntentos = int.Parse(ConfigurationManager.AppSettings["maxIntentosConexion"]);
            while (intentoActual < maximoIntentos && !response.IsSuccessStatusCode)
            {
                try
                {
                    response = await client.GetAsync(uri).ConfigureAwait(false);
                    content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        intentoActual++;
                    }
                }
                catch (Exception e)
                {
                    intentoActual++;
                    if (intentoActual == maximoIntentos)
                    {
                        throw e;
                    }
                }
            }
            if (!response.IsSuccessStatusCode)
            {
                string responseMessage = string.Format("Error: {0}", (int)response.StatusCode);
                throw new HttpRequestException(response + Environment.NewLine + content);
            }
            return JObject.Parse(content);
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    client.Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

    }
}

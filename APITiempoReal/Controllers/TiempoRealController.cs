using CalculosEnPot;
using Newtonsoft.Json;
using POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace APITiempoReal.Controllers
{
    /// <summary>
    /// Controlador creado para acceder a las operaciones
    /// </summary>
    [RoutePrefix("api/tiemporeal")]
    public class TiempoRealController : ApiController
    {
        /// <summary>
        /// Calcula la energía por hora para las barras con códigos provistos por parámetro, entre dos fechas determinadas.
        /// </summary>
        /// <param name="parametros">POCO con los parámetros de consulta (codigos de las barras en el MID, fecha inicio y fecha fin).</param>
        /// <returns>Serie de energía por hora para las barras.</returns>
        [HttpGet]
        [Route("energia")]
        [ActionName("energia")]
        public IHttpActionResult CalcularEnergia([FromBody] ParametrosConsulta parametros)
        {
            Task<List<ActivoElectrico>> t = CalculosSobreActivos.CalcularVariableElectricaAsync(Variables.E, parametros.CodigosBarras, parametros.FechaInicio, parametros.FechaFin);
            t.Wait();
            List<ActivoElectrico> activos = t.Result;

            if (activos.Count > 0)
            {
                activos.RemoveAll(a => a.SeriesDatos.Where(s => s.NombreSerie.Equals(Variables.E.ToString())).ToList().Count() <= 0);

                foreach (ActivoElectrico item in activos)
                {
                    item.SeriesDatos.RemoveAll(s => !s.NombreSerie.Equals(Variables.E.ToString()));
                }

                var json = JsonConvert.SerializeObject(ActivoElectrico.Desglosar(activos));
                return Ok(json);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Calcula la potencia máxima por hora para las barras con códigos provistos por parámetro, entre dos fechas determinadas.
        /// </summary>
        /// <param name="parametros">POCO con los parámetros de consulta (codigos de las barras en el MID, fecha inicio y fecha fin).</param>
        /// <returns>Serie de potencia máxima por hora para las barras.</returns>
        [HttpGet]
        [Route("potenciaMaxima")]
        [ActionName("potenciaMaxima")]
        public IHttpActionResult CalcularPotenciaMaxima([FromBody] ParametrosConsulta parametros)
        {
            Task<List<ActivoElectrico>> t = CalculosSobreActivos.CalcularVariableElectricaAsync(Variables.Pmax, parametros.CodigosBarras, parametros.FechaInicio, parametros.FechaFin);
            t.Wait();
            List<ActivoElectrico> activos = t.Result;

            if (activos.Count > 0)
            {
                activos.RemoveAll(a => a.SeriesDatos.Where(s => s.NombreSerie.Equals(Variables.Pmax.ToString())).ToList().Count() <= 0);

                foreach (ActivoElectrico item in activos)
                {
                    item.SeriesDatos.RemoveAll(s => !s.NombreSerie.Equals(Variables.Pmax.ToString()));
                }

                var json = JsonConvert.SerializeObject(ActivoElectrico.Desglosar(activos));
                return Ok(json);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

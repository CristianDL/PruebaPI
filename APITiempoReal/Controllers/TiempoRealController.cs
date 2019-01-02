using CalculosEnPot;
using POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace APITiempoReal.Controllers
{
    public class TiempoRealController : ApiController
    {
        [HttpGet]
        public IHttpActionResult CalcularEnergia(string[] codigosBarras, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ActivoElectrico> activos = CalculosSobreActivos.CalcularVariableElectrica(Variables.E, codigosBarras, fechaInicio, fechaFin);

            if (activos.Count > 0)
            {
                activos.RemoveAll(a => a.SeriesDatos.Where(s => s.NombreSerie.Equals(Variables.E.ToString())).ToList().Count() <= 0);

                foreach (ActivoElectrico item in activos)
                {
                    item.SeriesDatos.RemoveAll(s => !s.NombreSerie.Equals(Variables.E.ToString()));
                }

                return Ok(activos);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult CalcularPotenciaMaxima(string[] codigosBarras, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ActivoElectrico> activos = CalculosSobreActivos.CalcularVariableElectrica(Variables.Pmax, codigosBarras, fechaInicio, fechaFin);

            if (activos.Count > 0)
            {
                activos.RemoveAll(a => a.SeriesDatos.Where(s => s.NombreSerie.Equals(Variables.Pmax.ToString())).ToList().Count() <= 0);

                foreach (ActivoElectrico item in activos)
                {
                    item.SeriesDatos.RemoveAll(s => !s.NombreSerie.Equals(Variables.Pmax.ToString()));
                }

                return Ok(activos);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

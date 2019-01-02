using AccesoBaseDatos;
using PIWebAPI;
using POCOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculosEnPot
{
    public static class CalculosSobreActivos
    {
        public static List<ActivoElectrico> CalcularVariableElectrica(Variables variable, string[] codigosBarras, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ActivoElectrico> activos = new List<ActivoElectrico>();

            foreach (string codigo in codigosBarras)
            {
                ActivoElectrico activo = new ActivoElectrico
                {
                    CodigoMID = codigo
                };
                activos.Add(activo);
            }

            foreach (ActivoElectrico activo in activos)
            {
                ConsultasBDMID consulta = new ConsultasBDMID();
                activo.Tag = consulta.ObtenerTagMapeo(activo.CodigoMID);
                if (!string.IsNullOrEmpty(activo.Tag))
                {
                    activo.WebId = PIRequests.GetAttributeWebId(activo.Tag);
                }
            }

            activos = activos.Where(a => a.WebId != null).ToList();

            activos = PIRequests.GetRecordedDataAdHoc(activos, fechaInicio.AddDays(-1), fechaFin.AddDays(1));
            activos = activos.Where(a => a.SeriesDatos.Count > 0 && a.SeriesDatos[0].Datos.Where(x => x.Key >= fechaInicio && x.Key <= fechaFin).Count() > 0).ToList();

            foreach (ActivoElectrico activo in activos)
            {
                SerieDatos serie = new SerieDatos
                {
                    NombreSerie = variable.ToString()
                };

                DateTime fechaCalculo = fechaInicio;
                while (fechaCalculo < fechaFin.AddDays(1))
                {
                    KeyValuePair<DateTime, double> dato;

                    try
                    {
                        switch (variable)
                        {
                            case Variables.Pmax:
                                dato = CalculosEnergiaPotencia.CalcularPotenciaMaxima(activo.SeriesDatos.Where(x => x.NombreSerie.Equals(Variables.P.ToString())).First().Datos, fechaCalculo, fechaCalculo.AddHours(1));
                                break;
                            case Variables.E:
                                dato = CalculosEnergiaPotencia.CalcularEnergia(activo.SeriesDatos.Where(x => x.NombreSerie.Equals(Variables.P.ToString())).First().Datos, fechaCalculo, fechaCalculo.AddHours(1));
                                break;
                            default:
                                dato = new KeyValuePair<DateTime, double>(fechaCalculo, 0);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        dato = new KeyValuePair<DateTime, double>(fechaCalculo, 0);
                    }

                    serie.Datos.Add(dato.Key, dato.Value);
                    fechaCalculo = fechaCalculo.AddHours(1);
                }

                activo.SeriesDatos.Add(serie);
            }

            return activos;
        }
    }
}

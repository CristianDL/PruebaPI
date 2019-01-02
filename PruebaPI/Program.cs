using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIWebAPI;
using POCOs;
using CalculosEnPot;
using System.IO;
using AccesoBaseDatos;

namespace PruebaPI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<ActivoElectrico> activos = new List<ActivoElectrico>();
                List<string> codigos = args[0].Split(new char[] { ';' }).ToList();
                DateTime fechaInicio = DateTime.Parse(args[1]);
                DateTime fechaFin = DateTime.Parse(args[2]);

                foreach (string codigo in codigos)
                {
                    ActivoElectrico activo = new ActivoElectrico
                    {
                        CodigoMID = codigo
                    };
                    activos.Add(activo);
                }

                Console.WriteLine("Buscando mapeos al MID...");

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

                Console.WriteLine("Consultando datos de PI...");

                activos = PIRequests.GetRecordedDataAdHoc(activos, fechaInicio.AddDays(-1), fechaFin.AddDays(1));
                activos = activos.Where(a => a.SeriesDatos.Count > 0 && a.SeriesDatos[0].Datos.Count > 0).ToList();

                Console.WriteLine("Calculando energía y potencia máxima...");

                foreach (ActivoElectrico activo in activos)
                {
                    SerieDatos energia = new SerieDatos();
                    SerieDatos potenciaMaxima = new SerieDatos();

                    energia.NombreSerie = Variables.E.ToString();
                    potenciaMaxima.NombreSerie = Variables.Pmax.ToString();

                    DateTime fechaCalculo = fechaInicio;
                    while (fechaCalculo < fechaFin.AddDays(1))
                    {
                        KeyValuePair<DateTime, double> datoEnergia;
                        KeyValuePair<DateTime, double> datoPotenciaMaxima;

                        try
                        {
                            datoEnergia = CalculosEnergiaPotencia.CalcularEnergia(activo.SeriesDatos.Where(x => x.NombreSerie.Equals(Variables.P.ToString())).First().Datos, fechaCalculo, fechaCalculo.AddHours(1));
                        }
                        catch (Exception)
                        {
                            datoEnergia = new KeyValuePair<DateTime, double>(fechaCalculo, 0);
                        }

                        try
                        {
                            datoPotenciaMaxima = CalculosEnergiaPotencia.CalcularPotenciaMaxima(activo.SeriesDatos.Where(x => x.NombreSerie.Equals(Variables.P.ToString())).First().Datos, fechaCalculo, fechaCalculo.AddHours(1));
                        }
                        catch (Exception)
                        {
                            datoPotenciaMaxima = new KeyValuePair<DateTime, double>(fechaCalculo, 0);
                        }

                        energia.Datos.Add(datoEnergia.Key, datoEnergia.Value);
                        potenciaMaxima.Datos.Add(datoPotenciaMaxima.Key, datoPotenciaMaxima.Value);

                        fechaCalculo = fechaCalculo.AddHours(1);
                    }

                    activo.SeriesDatos.Add(energia);
                    activo.SeriesDatos.Add(potenciaMaxima);
                }

                StringBuilder resultado = new StringBuilder();
                resultado.Append("CodigoMID").Append(";").Append("Variable").Append(";").Append("Fecha").Append(";").Append("Hora").Append(";").Append("Valor").Append(Environment.NewLine);

                Console.WriteLine("Generando archivo de resultados...");

                foreach (ActivoElectrico item in activos)
                {
                    //SerieDatos borrar = item.SeriesDatos.Where(x => x.NombreSerie.Equals(Variables.P.ToString())).First();
                    //item.SeriesDatos.Remove(borrar);
                    resultado.Append(item.ToString());
                }

                File.WriteAllText("calculos.csv", resultado.ToString());
            }
            //catch (Exception e)
            //{
            //    StringBuilder error = new StringBuilder();
            //    error.Append("Error: ").Append(e.Message).Append(Environment.NewLine).Append(e.StackTrace).Append(Environment.NewLine);
            //    var ex = e.InnerException;
            //    while (ex != null)
            //    {
            //        error.Append(ex.Message).Append(Environment.NewLine).Append(ex.StackTrace).Append(Environment.NewLine);
            //        ex = ex.InnerException;
            //    }
            //    Console.WriteLine(error.ToString());

            //}
            finally
            {
                Console.WriteLine("Finalizado. Presione una tecla para salir...");
                Console.ReadKey();
            }
        }
    }
}
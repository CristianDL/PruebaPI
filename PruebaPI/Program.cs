using AccesoBaseDatos;
using CalculosEnPot;
using PIWebAPI;
using POCOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaPI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Task t = ProbarPI(args);
                t.Wait();
            }
            catch (Exception e)
            {
                StringBuilder error = new StringBuilder();
                error.Append("Error: ").Append(e.Message).Append(Environment.NewLine).Append(e.StackTrace).Append(Environment.NewLine);
                var ex = e.InnerException;
                while (ex != null)
                {
                    error.Append(ex.Message).Append(Environment.NewLine).Append(ex.StackTrace).Append(Environment.NewLine);
                    ex = ex.InnerException;
                }
                Console.WriteLine(error.ToString());
            }
            finally
            {
                Console.WriteLine("Finalizado. Presione una tecla para salir...");
                Console.ReadKey();
            }
        }

        static async Task ProbarPI(string[] args)
        {
            List<ActivoElectrico> activos = new List<ActivoElectrico>();
            List<string> codigos = args[0].Split(new char[] { ';' }).ToList();
            DateTimeOffset fechaInicio = DateTimeOffset.Parse(args[1]);
            DateTimeOffset fechaFin = DateTimeOffset.Parse(args[2]);

            foreach (string codigo in codigos)
            {
                ActivoElectrico activo = new ActivoElectrico
                {
                    CodigoMID = codigo
                };
                activos.Add(activo);
            }

            Console.WriteLine("Barras iniciales: " + activos.Count());

            Console.WriteLine("Buscando mapeos al MID...");

            ConsultasBDMID consulta = new ConsultasBDMID();

            foreach (ActivoElectrico activo in activos)
            {
                activo.Tag = consulta.ObtenerTagMapeo(activo.CodigoMID);
                if (!string.IsNullOrEmpty(activo.Tag))
                {
                    activo.WebId = await PIRequests.GetAttributeWebIdAsync(activo.Tag);
                }
            }

            List<ActivoElectrico> borrados = activos.Where(a => a.WebId == null).ToList();

            activos = activos.Where(a => a.WebId != null).ToList();

            Console.WriteLine("Barras con WebId: " + activos.Count);
            if (borrados.Count > 0)
            {
                Console.WriteLine("Borradas:");
                foreach (var item in borrados)
                {
                    Console.WriteLine(item.CodigoMID);
                } 
            }

            Console.WriteLine("Consultando datos de PI...");

            activos = await PIRequests.GetPlotDataAdHocAsync(activos, fechaInicio.AddDays(-1), fechaFin.AddDays(2)).ConfigureAwait(false);
            activos = await PIRequests.GetRecordedDataAdHocAsync(activos, fechaInicio.AddDays(-1), fechaFin.AddDays(2)).ConfigureAwait(false);

            borrados = activos.Where(a => a.SeriesDatos.Count <= 0 || a.SeriesDatos[0].Datos.Where(x => x.Key >= fechaInicio && x.Key <= fechaFin).Count() <= 0).ToList();

            activos = activos.Where(a => a.SeriesDatos.Count > 0 && a.SeriesDatos[0].Datos.Where(x => x.Key >= fechaInicio && x.Key <= fechaFin).Count() > 0).ToList();

            Console.WriteLine("Barras con datos: " + activos.Count);
            if (borrados.Count > 0)
            {
                Console.WriteLine("Borradas:");
                foreach (var item in borrados)
                {
                    Console.WriteLine(item.CodigoMID);
                } 
            }

            Console.WriteLine("Calculando energía y potencia máxima...");

            foreach (ActivoElectrico activo in activos)
            {
                SerieDatos energia = new SerieDatos();
                SerieDatos potenciaMaxima = new SerieDatos();

                energia.NombreSerie = Variables.E.ToString();
                potenciaMaxima.NombreSerie = Variables.Pmax.ToString();

                DateTimeOffset fechaCalculo = fechaInicio;
                while (fechaCalculo < fechaFin.AddDays(1))
                {
                    KeyValuePair<DateTimeOffset, double> datoEnergia;
                    KeyValuePair<DateTimeOffset, double> datoPotenciaMaxima;

                    try
                    {
                        datoEnergia = CalculosEnergiaPotencia.CalcularEnergia(activo.SeriesDatos.Where(x => x.NombreSerie.Equals(Variables.P.ToString())).First().Datos, fechaCalculo, fechaCalculo.AddHours(1));
                    }
                    catch (Exception)
                    {
                        datoEnergia = new KeyValuePair<DateTimeOffset, double>(fechaCalculo, 0);
                    }

                    try
                    {
                        datoPotenciaMaxima = CalculosEnergiaPotencia.CalcularPotenciaMaxima(activo.SeriesDatos.Where(x => x.NombreSerie.Equals(Variables.P.ToString())).First().Datos, fechaCalculo, fechaCalculo.AddHours(1));
                    }
                    catch (Exception)
                    {
                        datoPotenciaMaxima = new KeyValuePair<DateTimeOffset, double>(fechaCalculo, 0);
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
    }
}
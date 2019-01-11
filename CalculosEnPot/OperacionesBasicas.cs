using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculosEnPot
{
    public class OperacionesBasicas
    {
        public static KeyValuePair<DateTimeOffset, double> InterpolacionLineal(Dictionary<DateTimeOffset, double> serie, DateTimeOffset fechaInter)
        {
            List<DateTimeOffset> fechas = serie.Keys.ToList();
            DateTimeOffset fechaInicio = fechas.Where(x => x < fechaInter).OrderByDescending(x => x).First();
            DateTimeOffset fechaFin = fechas.Where(x => x > fechaInter).OrderBy(x => x).First();
            double datoInicio = serie[fechaInicio];
            double datoFin = serie[fechaFin];
            TimeSpan hastaInter = new TimeSpan(fechaInter.Ticks - fechaInicio.Ticks);
            TimeSpan hastaFin = new TimeSpan(fechaFin.Ticks - fechaInicio.Ticks);

            return new KeyValuePair<DateTimeOffset, double>(fechaInter, datoInicio + ((datoFin - datoInicio) * hastaInter.TotalSeconds / hastaFin.TotalSeconds) );
        }
        
        public static double IntegracionTrapezoidal(Dictionary<DateTimeOffset, double> serieDatos)
        {
            List<DateTimeOffset> fechas = serieDatos.Keys.ToList();
            fechas.Sort();
            double integral = 0;
    
            for (int i = 0; i < fechas.Count - 1; i++)
            {
                TimeSpan intervalo = new TimeSpan(fechas.ElementAt(i + 1).Ticks - fechas.ElementAt(i).Ticks);
                integral += (serieDatos[fechas.ElementAt(i + 1)] + serieDatos[fechas.ElementAt(i)]) * (intervalo.TotalHours) / 2;
            }

            return integral;
        } 
    }
}
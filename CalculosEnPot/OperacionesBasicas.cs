using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculosEnPot
{
    public class OperacionesEnergia
    {
        public static double InterpolacionLineal(double potInf, double potSup, DateTime fechaInicio, DateTime fechaFin, DateTime fechaInter)
        {
            TimeSpan hastaInter = new TimeSpan(fechaInter.Ticks - fechaInicio.Ticks);
            TimeSpan hastaFin = new TimeSpan(fechaFin.Ticks - fechaInicio.Ticks);

            return potInf + ((potSup - potInf)*(hastaInter.TotalSeconds)/(hastaFin.TotalSeconds));
        }
        
        public static double IntegracionTrapezoidal(Dictionary<DateTime, double> serieDatos)
        {
            double integral = 0;
    
            for (int i = 0; i < serieDatos.Keys.Count - 1; i++)
            {
                TimeSpan intervalo = new TimeSpan(serieDatos.Keys.ElementAt(i + 1).Ticks - serieDatos.Keys.ElementAt(i).Ticks);
                integral += (serieDatos[serieDatos.Keys.ElementAt(i + 1)] + serieDatos[serieDatos.Keys.ElementAt(i)]) * (intervalo.TotalSeconds) / 2;
            }

            return integral;
        } 
    }
}
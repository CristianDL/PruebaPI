using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculosEnPot
{
    public static class CalculosEnergiaPotencia
    {
        public static KeyValuePair<DateTimeOffset, double> CalcularEnergia(Dictionary<DateTimeOffset, double> seriePotencia, DateTimeOffset fechaInicio, DateTimeOffset fechaFin)
        {
            if (!seriePotencia.ContainsKey(fechaInicio))
            {
                KeyValuePair<DateTimeOffset, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia, fechaInicio);
                seriePotencia.Add(datoEst.Key, datoEst.Value);
            }
            if (!seriePotencia.ContainsKey(fechaFin))
            {
                KeyValuePair<DateTimeOffset, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia, fechaFin);
                seriePotencia.Add(datoEst.Key, datoEst.Value);
            }

            List<KeyValuePair<DateTimeOffset, double>> datosRecortados = seriePotencia.Where(x => x.Key >= fechaInicio && x.Key <= fechaFin).ToList();
            Dictionary<DateTimeOffset, double> serieRecortada = datosRecortados.ToDictionary(pair => pair.Key, pair => pair.Value);

            return new KeyValuePair<DateTimeOffset, double>(fechaInicio, OperacionesBasicas.IntegracionTrapezoidal(serieRecortada));
        }

        public static KeyValuePair<DateTimeOffset, double> CalcularPotenciaMaxima(Dictionary<DateTimeOffset, double> seriePotencia, DateTimeOffset fechaInicio, DateTimeOffset fechaFin)
        {
            if (!seriePotencia.ContainsKey(fechaInicio))
            {
                KeyValuePair<DateTimeOffset, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia, fechaInicio);
                seriePotencia.Add(datoEst.Key, datoEst.Value);
            }
            if (!seriePotencia.ContainsKey(fechaFin))
            {
                KeyValuePair<DateTimeOffset, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia, fechaFin);
                seriePotencia.Add(datoEst.Key, datoEst.Value);
            }

            List<KeyValuePair<DateTimeOffset, double>> datosRecortados = seriePotencia.Where(x => x.Key >= fechaInicio && x.Key <= fechaFin).ToList();
            Dictionary<DateTimeOffset, double> serieRecortada = datosRecortados.ToDictionary(pair => pair.Key, pair => pair.Value);

            return new KeyValuePair<DateTimeOffset, double>(fechaInicio, serieRecortada.Values.ToList().Max());
        }
    }
}

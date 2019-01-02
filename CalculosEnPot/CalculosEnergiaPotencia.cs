using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculosEnPot
{
    public static class CalculosEnergiaPotencia
    {
        public static KeyValuePair<DateTime, double> CalcularEnergia(Dictionary<DateTime, double> seriePotencia, DateTime fechaInicio, DateTime fechaFin)
        {
            if (!seriePotencia.ContainsKey(fechaInicio))
            {
                KeyValuePair<DateTime, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia, fechaInicio);
                seriePotencia.Add(datoEst.Key, datoEst.Value);
            }
            if (!seriePotencia.ContainsKey(fechaFin))
            {
                KeyValuePair<DateTime, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia, fechaFin);
                seriePotencia.Add(datoEst.Key, datoEst.Value);
            }

            List<KeyValuePair<DateTime, double>> datosRecortados = seriePotencia.Where(x => x.Key >= fechaInicio && x.Key <= fechaFin).ToList();
            Dictionary<DateTime, double> serieRecortada = datosRecortados.ToDictionary(pair => pair.Key, pair => pair.Value);

            return new KeyValuePair<DateTime, double>(fechaInicio, OperacionesBasicas.IntegracionTrapezoidal(serieRecortada));
        }

        public static KeyValuePair<DateTime, double> CalcularPotenciaMaxima(Dictionary<DateTime, double> seriePotencia, DateTime fechaInicio, DateTime fechaFin)
        {
            if (!seriePotencia.ContainsKey(fechaInicio))
            {
                KeyValuePair<DateTime, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia, fechaInicio);
                seriePotencia.Add(datoEst.Key, datoEst.Value);
            }
            if (!seriePotencia.ContainsKey(fechaFin))
            {
                KeyValuePair<DateTime, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia, fechaFin);
                seriePotencia.Add(datoEst.Key, datoEst.Value);
            }

            List<KeyValuePair<DateTime, double>> datosRecortados = seriePotencia.Where(x => x.Key >= fechaInicio && x.Key <= fechaFin).ToList();
            Dictionary<DateTime, double> serieRecortada = datosRecortados.ToDictionary(pair => pair.Key, pair => pair.Value);

            return new KeyValuePair<DateTime, double>(fechaInicio, serieRecortada.Values.ToList().Max());
        }
    }
}

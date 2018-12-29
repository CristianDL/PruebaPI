using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POCOs;

namespace CalculosEnPot
{
    public static class CalculosEnergiaPotencia
    {
        public static KeyValuePair<DateTime, double> CalcularEnergia(SerieDatos seriePotencia, DateTime fechaInicio, DateTime fechaFin)
        {
            if (!seriePotencia.Datos.ContainsKey(fechaInicio))
            {
                KeyValuePair<DateTime, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia.Datos, fechaInicio);
                seriePotencia.Datos.Add(datoEst.Key, datoEst.Value);
            }
            if (!seriePotencia.Datos.ContainsKey(fechaFin))
            {
                KeyValuePair<DateTime, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia.Datos, fechaFin);
                seriePotencia.Datos.Add(datoEst.Key, datoEst.Value);
            }

            List<KeyValuePair<DateTime, double>> datosRecortados = seriePotencia.Datos.Where(x => x.Key >= fechaInicio && x.Key <= fechaFin).ToList();
            Dictionary<DateTime, double> serieRecortada = datosRecortados.ToDictionary(pair => pair.Key, pair => pair.Value);

            return new KeyValuePair<DateTime, double>(fechaInicio, OperacionesBasicas.IntegracionTrapezoidal(serieRecortada));
        }

        public static KeyValuePair<DateTime, double> CalcularPotenciaMaxima(SerieDatos seriePotencia, DateTime fechaInicio, DateTime fechaFin)
        {
            if (!seriePotencia.Datos.ContainsKey(fechaInicio))
            {
                KeyValuePair<DateTime, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia.Datos, fechaInicio);
                seriePotencia.Datos.Add(datoEst.Key, datoEst.Value);
            }
            if (!seriePotencia.Datos.ContainsKey(fechaFin))
            {
                KeyValuePair<DateTime, double> datoEst = OperacionesBasicas.InterpolacionLineal(seriePotencia.Datos, fechaFin);
                seriePotencia.Datos.Add(datoEst.Key, datoEst.Value);
            }

            List<KeyValuePair<DateTime, double>> datosRecortados = seriePotencia.Datos.Where(x => x.Key >= fechaInicio && x.Key <= fechaFin).ToList();
            Dictionary<DateTime, double> serieRecortada = datosRecortados.ToDictionary(pair => pair.Key, pair => pair.Value);

            return new KeyValuePair<DateTime, double>(fechaInicio, serieRecortada.Values.ToList().Max());
        }
    }
}

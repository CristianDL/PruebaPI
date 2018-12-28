using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIWebAPI;
using System.Configuration;
using POCOs;

namespace PruebaPI
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ActivoElectrico> activos = new List<ActivoElectrico>();

            foreach (string codigo in args)
            {
                ActivoElectrico activo = new ActivoElectrico();
                activo.CodigoMID = codigo;
                activos.Add(activo);
            }

            foreach (ActivoElectrico activo in activos)
            {
                //TODO: Consulta al MID por los tags
                activo.Tag = string.Empty;
                activo.WebId = PIRequests.GetAttributeWebId(activo.Tag);
            }

            activos = PIRequests.GetRecordedDataAdHoc(activos, new DateTime(2018, 10, 22), new DateTime(2018, 10, 28));

            //Calcular potencia y energía de cada activo (barra)

            //Escribir la información a un csv.

            Console.WriteLine("Presione una tecla para salir...");
            Console.ReadKey();
        }
    }
}

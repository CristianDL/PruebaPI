using System;
using PIWebAPI;
using System.Configuration;

namespace PruebaPI
{
    class Program
    {
        static void Main(string[] args)
        {
            using (PIWebAPIClient client = new PIWebAPIClient(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]))
            {
                //aqui va el código
            }

            Console.WriteLine("Presione una tecla para salir...");
            Console.ReadKey();
        }
    }
}

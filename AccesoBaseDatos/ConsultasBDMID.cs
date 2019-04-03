using System.Configuration;
using System.Data;

namespace AccesoBaseDatos
{
    public class ConsultasBDMID
    {
        private AccesoBD conexion;
        private ParametrosSql listaParametros;

        public ConsultasBDMID()
        {
            conexion = new AccesoBD(ConfigurationManager.AppSettings["conexionMID"]);
        }

        public string ObtenerTagMapeo(string codigoMID)
        {
            listaParametros = new ParametrosSql();

            listaParametros.AdicionarParametro("@codigoMID", SqlDbType.VarChar, ParameterDirection.Input, codigoMID);

            return (string)conexion.EjecutarQueryEscalar(Queries.TagMapeo, listaParametros.ListaParametros.ToArray());
        }
    }
}

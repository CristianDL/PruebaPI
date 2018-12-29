using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AccesoBaseDatos
{
    public class ConsultasBDMID
    {
        private AccesoBD conexion;
        private ParametrosSql listaParametros;

        public ConsultasBDMID()
        {
            conexion = new AccesoBD();
        }

        public string ObtenerTagMapeo(string codigoMID)
        {
            listaParametros = new ParametrosSql();

            listaParametros.AdicionarParametro("@codigoMID", SqlDbType.VarChar, ParameterDirection.Input, codigoMID);

            return (string)conexion.EjecutarEscalar(Queries.TagMapeo, listaParametros.ListaParametros.ToArray());
        }
    }
}

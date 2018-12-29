using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoBaseDatos
{
    public class ParametrosSql
    {
        private SqlParameter parametro;
        private List<SqlParameter> parametros;

        public List<SqlParameter> ListaParametros { get { return parametros; } }

        public ParametrosSql()
        {
            parametros = new List<SqlParameter>();
        }

        /// <summary>
        /// Agrega un parámetro a la lista
        /// </summary>
        /// <param name="nombre">nombre del parámetro inlcuido el @</param>
        /// <param name="tipoSql">el tipo sql del parámetro</param>
        /// <param name="valor">valor</param>
        public void AdicionarParametro(string nombre, SqlDbType tipoSql, ParameterDirection direccion, object valor)
        {
            parametro = new SqlParameter(nombre, tipoSql);
            parametro.Direction = direccion;
            if (valor == null)
                parametro.Value = DBNull.Value;
            else
                parametro.Value = valor;
            parametros.Add(parametro);
        }
    }
}
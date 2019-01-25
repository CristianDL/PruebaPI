using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace AccesoBaseDatos
{
    public class AccesoBD
    {
        private string cadenaConexion;

        public AccesoBD()
        {
            cadenaConexion = ConfigurationManager.AppSettings["conexionMID"];
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y retorna un DataTable con los datos
        /// </summary>
        /// <param name="nombreSP">Nombre del procedimiento almacenado</param>
        /// <param name="parametros">Parámetros del procedimiento almacenado, null si no tiene</param>
        /// <returns>DataTable con los datos</returns>
        public DataTable ObtenerDataTable(string nombreSP, SqlParameter[] parametros)
        {
            DataTable dt = new DataTable();
            int intentoActual = 0;
            int maximoIntentos = int.Parse(ConfigurationManager.AppSettings["maxIntentosConexion"]);
            while (intentoActual < maximoIntentos)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(cadenaConexion))
                    using (SqlCommand cmd = new SqlCommand(nombreSP, conn))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parametros != null)
                        {
                            cmd.Parameters.AddRange(parametros);
                        }
                        da.Fill(dt);
                        intentoActual = maximoIntentos;
                    }
                }
                catch (Exception e)
                {
                    intentoActual++;
                    if (intentoActual == maximoIntentos)
                    {
                        throw e;
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y retorna un DataRow con los datos en dado caso que no existan datos
        /// se retorna null
        /// </summary>
        /// <param name="nombreSP">Nombre del procedimiento almacenado</param>
        /// <param name="parametros">Parámetros del procedimiento almacenado, null si no tiene</param>
        /// <returns>DataRow con los datos</returns>
        public DataRow ObtenerDataRow(string nombreSP, SqlParameter[] parametros)
        {
            DataTable dt = new DataTable();
            int intentoActual = 0;
            int maximoIntentos = int.Parse(ConfigurationManager.AppSettings["maxIntentosConexion"]);
            while (intentoActual < maximoIntentos)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(cadenaConexion))
                    using (SqlCommand cmd = new SqlCommand(nombreSP, conn))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parametros != null)
                        {
                            cmd.Parameters.AddRange(parametros);
                        }
                        da.Fill(dt);
                        intentoActual = maximoIntentos;
                    }
                }
                catch (Exception e)
                {
                    intentoActual++;
                    if (intentoActual == maximoIntentos)
                    {
                        throw e;
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y retorna la cantidad de filas afectadas
        /// </summary>
        /// <param name="nombreSP">Nombre del procedimiento almacenado</param>
        /// <param name="parametros">Parámetros del procedimiento almacenado, null si no tiene</param>
        /// <returns></returns>
        public int EjecutarQuery(string nombreSP, SqlParameter[] parametros)
        {
            int filasAfectadas = 0;
            int intentoActual = 0;
            int maximoIntentos = int.Parse(ConfigurationManager.AppSettings["maxIntentosConexion"]);
            while (intentoActual < maximoIntentos)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(cadenaConexion))
                    using (SqlCommand cmd = new SqlCommand(nombreSP, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parametros != null)
                        {
                            cmd.Parameters.AddRange(parametros);
                        }
                        conn.Open();
                        filasAfectadas = cmd.ExecuteNonQuery();
                        intentoActual = maximoIntentos;
                    }
                }
                catch (Exception e)
                {
                    intentoActual++;
                    if (intentoActual == maximoIntentos)
                    {
                        throw e;
                    }
                }
            }
            return filasAfectadas;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y retorna la primera columna de la primera fila
        /// </summary>
        /// <param name="nombreSP">Nombre del procedimiento almacenado</param>
        /// <param name="parametros">Parámetros del procedimiento almacenado, null si no tiene</param>
        /// <returns>Primera columna de la primera fila</returns>
        public object EjecutarScalar(string nombreSP, SqlParameter[] parametros)
        {
            object respuesta = null;
            int intentoActual = 0;
            int maximoIntentos = int.Parse(ConfigurationManager.AppSettings["maxIntentosConexion"]);
            while (intentoActual < maximoIntentos)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(cadenaConexion))
                    using (SqlCommand cmd = new SqlCommand(nombreSP, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parametros != null)
                        {
                            cmd.Parameters.AddRange(parametros);
                        }
                        conn.Open();
                        respuesta = cmd.ExecuteScalar();
                        intentoActual = maximoIntentos;
                    }
                }
                catch (Exception e)
                {
                    intentoActual++;
                    if (intentoActual == maximoIntentos)
                    {
                        throw e;
                    }
                }
            }
            return respuesta;
        }

        /// <summary>
        /// Ejecuta una consulta y retorna la primera columna de la primera fila
        /// </summary>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="parametros">Parámetros, null si no tiene</param>
        /// <returns>Primera columna de la primera fila</returns>
        public object EjecutarEscalar(string query, SqlParameter[] parametros)
        {
            object respuesta = null;
            int intentoActual = 0;
            int maximoIntentos = int.Parse(ConfigurationManager.AppSettings["maxIntentosConexion"]);
            while (intentoActual < maximoIntentos)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(cadenaConexion))
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (parametros != null)
                        {
                            cmd.Parameters.AddRange(parametros);
                        }
                        conn.Open();
                        respuesta = cmd.ExecuteScalar();
                        intentoActual = maximoIntentos;
                    }
                }
                catch (Exception e)
                {
                    intentoActual++;
                    if (intentoActual == maximoIntentos)
                    {
                        throw e;
                    }
                }
            }
            return respuesta;
        }

        /// <summary>
        /// Método que convierte una fila obtenida de una consulta en un objeto de clase definida por parámetro. Es necesario que las columnas tengan el mismo nombre que las propiedades de la clase.
        /// </summary>
        /// <typeparam name="T">La clase del objeto retornado.</typeparam>
        /// <param name="fila">La fila a convertir.</param>
        /// <returns>Objeto convertido.</returns>
        public T Materializar<T>(DataRow fila)
        {
            T objeto = Activator.CreateInstance<T>();
            foreach (PropertyInfo item in objeto.GetType().GetProperties())
            {
                Type tipoPropiedad = item.PropertyType;
                if (DBNull.Value.Equals(fila[item.Name]))
                {
                    item.SetValue(objeto, null, null);
                }
                else
                {
                    item.SetValue(objeto, Convert.ChangeType(fila[item.Name], tipoPropiedad), null);
                }
            }
            return objeto;
        }

        /// <summary>
        /// Método que convierte un DataTable obtenido de una consulta en un arreglo de objetos de clase definida por parámetro. Es necesario que las columnas del DataTable tengan el mismo nombre que las propiedades de la clase.
        /// </summary>
        /// <typeparam name="T">La clase del arreglo de objetos retornado.</typeparam>
        /// <param name="dt">El DataTable a convertir.</param>
        /// <returns>Arreglo de objetos convertidos.</returns>
        public T[] Materializar<T>(DataTable dt)
        {
            List<T> listaObjetos = new List<T>();

            foreach (DataRow r in dt.Select())
            {
                listaObjetos.Add(Materializar<T>(r));
            }

            return listaObjetos.ToArray();
        }
    }
}
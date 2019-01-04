using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace POCOs
{
    [DataContract]
    public class SerieDatos
    {
        [DataMember]
        public string NombreSerie { get; set; }
        [DataMember]
        public Dictionary<DateTime, double> Datos { get; set; }

        public SerieDatos()
        {
            Datos = new Dictionary<DateTime, double>();
        }
    }
}

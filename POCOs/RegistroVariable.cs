using System.Runtime.Serialization;

namespace POCOs
{
    [DataContract]
    public class RegistroVariable
    {
        [DataMember]
        public string CodigoMID { get; set; }
        [DataMember]
        public string Variable { get; set; }
        [DataMember]
        public string Fecha { get; set; }
        [DataMember]
        public string Hora { get; set; }
        [DataMember]
        public string Valor { get; set; }
    }
}

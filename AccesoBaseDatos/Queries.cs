namespace AccesoBaseDatos
{
    public static class Queries
    {
        public const string TagMapeo = "SELECT CONCAT(m.B1_SCADA,':', m.B2_SCADA,':', m.B3_SCADA,':P:MvMoment') AS Tag FROM dbo.Mapeo m WHERE objID = @codigoMID";
    }
}

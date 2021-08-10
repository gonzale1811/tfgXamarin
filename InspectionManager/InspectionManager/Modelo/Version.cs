using System;
namespace InspectionManager.Modelo
{
    public class Version
    {
        public string NumeroVersion { get; set; }

        public Version(string numeroVersion)
        {
            NumeroVersion = numeroVersion;
        }
    }
}

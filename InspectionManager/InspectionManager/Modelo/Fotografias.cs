using System;
namespace InspectionManager.Modelo
{
    public class Fotografias
    {
        public Guid IdFotografia { get; set; }

        public Fotografias()
        {
            IdFotografia = Guid.NewGuid();
        }

        public Fotografias(Guid idFotografia)
        {
            IdFotografia = idFotografia;
        }
    }
}

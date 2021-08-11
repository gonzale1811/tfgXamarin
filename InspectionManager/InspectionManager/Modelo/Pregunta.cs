using System;
namespace InspectionManager.Modelo
{
    public interface IPregunta<T>
    {
        Guid IdPregunta
        {
            get;
        }

        string Nombre
        {
            get;
            set;
        }

        string PuestoTrabajo
        {
            get;
            set;
        }

        IRespuesta<T> RespuestaPregunta
        {
            get;
            set;
        }

        void Responder(T valor);

        void EditarRespuesta(T valor);
    }
}

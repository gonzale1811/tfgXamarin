using System;
namespace InspectionManager.Modelo
{
    public interface IPregunta<T>
    {
        Guid IdPregunta
        {
            get;
            set;
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

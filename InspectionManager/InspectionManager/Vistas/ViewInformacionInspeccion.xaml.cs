using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewInformacionInspeccion : ContentPage
    {
        private Inspeccion inspeccion;
        private IFirebaseConsultService consult;

        public ViewInformacionInspeccion(Inspeccion inspeccionElegida)
        {
            InitializeComponent();

            inspeccion = inspeccionElegida;

            consult = DependencyService.Get<IFirebaseConsultService>();

            nombreEntry.Text = "Nombre: "+inspeccion.Nombre;
            fechaInicioPicker.Date = inspeccion.FechaInicio;
            fechaFinPicker.Date = inspeccion.FechaFin;
            calleEntry.Text = "Calle: "+inspeccion.DireccionInspeccion.Calle;
            numeroEntry.Text = "Número: "+inspeccion.DireccionInspeccion.Numero;
            localidadEntry.Text = "Localidad: "+inspeccion.DireccionInspeccion.Localidad;
            codigoPostalEntry.Text = "Código Postal: "+inspeccion.DireccionInspeccion.CodigoPostal;
        }

        public void ProcesarEliminarInspeccion(object sender, EventArgs e)
        {

        }

        public void ProcesarDescargarInspeccion(object sender, EventArgs e)
        {
            consult.GenerarPdfInspeccion(inspeccion);
        }

        public void ProcesarEditarInspeccion(object sender, EventArgs e)
        {

        }

        public void ProcesarObtenerBloques(object sender, EventArgs e)
        {

        }
    }
}

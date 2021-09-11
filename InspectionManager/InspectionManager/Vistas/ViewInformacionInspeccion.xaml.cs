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
        private List<Bloque> bloquesInspeccion;
        private IFirebaseConsultService consult;

        public ViewInformacionInspeccion(Inspeccion inspeccionElegida)
        {
            InitializeComponent();

            inspeccion = inspeccionElegida;

            consult = DependencyService.Get<IFirebaseConsultService>();

            bloquesInspeccion = consult.GetBloquesByInspeccion(inspeccion);

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

        public async void ProcesarDescargarInspeccion(object sender, EventArgs e)
        {
            consult.GenerarPdfInspeccion(inspeccion, bloquesInspeccion);
            await DisplayAlert("Correcto", "Archivo pdf generado en la carpeta InspectionManager", "Ok");
        }

        public void ProcesarEditarInspeccion(object sender, EventArgs e)
        {

        }

        public void ProcesarObtenerBloques(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewMenuPrincipal : TabbedPage
    {
        private IFirebaseAuthService auth;
        private IFirebaseConsultService consult;
        private Inspector usuario;
        private List<Inspeccion> inspecciones;
        private DateTime fechaDeNacimientoUsuario;

        public ViewMenuPrincipal()
        {
            InitializeComponent();

            CurrentPage = Children[1];

            auth = DependencyService.Get<IFirebaseAuthService>();
            consult = DependencyService.Get<IFirebaseConsultService>();

            string emailUsuario = auth.GetUserEmail();

            usuario = consult.GetInspectorByEmail(emailUsuario);

            if (usuario.Inspecciones.Count > 0)
            {
                informacionLabel.Text = "Lista de inspecciones";

                inspecciones = consult.GetInspeccionesByUsuario(usuario);

                List<InspeccionListViewModel> items = new List<InspeccionListViewModel>();

                foreach (Inspeccion inspeccion in inspecciones)
                {
                    items.Add(new InspeccionListViewModel(inspeccion.IdInspeccion.ToString(), inspeccion.Nombre, inspeccion.FechaInicio.ToString(),
                        inspeccion.FechaFin.ToString(), inspeccion.Bloques.Count));
                }

                inspeccionesListView.ItemsSource = items;
            }
            else
            {
                informacionLabel.Text = "No tiene inspecciones";
            }

            if (usuario != null)
            {
                dniEntry.Text = usuario.Dni;
                nombreEntry.Text = usuario.Nombre;
                apellidosEntry.Text = usuario.Apellidos;
                usernameEntry.Text = usuario.Usuario;
                passwordEntry.Text = usuario.Password;
                fechaNacimientoPicker.Date = usuario.FechaNacimiento;
                fechaDeNacimientoUsuario = usuario.FechaNacimiento;
            }
        }

        public async void ProcesarCerrarSesion(object sender, EventArgs e)
        {
            bool logout = await auth.LogOut();

            if (logout)
            {
                App.Current.MainPage = new NavigationPage(new ViewLogin());
            }
            else
            {
                await DisplayAlert("Error", "No se ha podido cerrar sesion", "Ok");
            }
        }

        public void ProcesarEditarPerfil(object sender, EventArgs e)
        {
            nombreEntry.IsEnabled = true;
            apellidosEntry.IsEnabled = true;
            fechaNacimientoPicker.IsEnabled = true;

            editarPerfilButton.IsEnabled = false;
            editarPerfilButton.IsVisible = false;

            guardarPerfilButton.IsVisible = true;
            guardarPerfilButton.IsEnabled = true;
        }

        public async void ProcesarGuardarPerfil(object sender, EventArgs e)
        {
            OcultarError();

            if (ComprobarCampos())
            {
                Inspector inspectorActualizado = new Inspector(usuario.Dni, nombreEntry.Text, apellidosEntry.Text, usuario.Usuario, usuario.Password, fechaDeNacimientoUsuario);
                consult.ActualizarInformacionUsuario(inspectorActualizado);

                usuario.Nombre = inspectorActualizado.Nombre;
                usuario.Apellidos = inspectorActualizado.Apellidos;
                usuario.FechaNacimiento = inspectorActualizado.FechaNacimiento;

                nombreEntry.IsEnabled = false;
                apellidosEntry.IsEnabled = false;
                fechaNacimientoPicker.IsEnabled = false;

                guardarPerfilButton.IsEnabled = false;
                guardarPerfilButton.IsVisible = false;

                editarPerfilButton.IsVisible = true;
                editarPerfilButton.IsEnabled = true;
            }
            else
            {
                await DisplayAlert("Error", "Alguno de los campos es incorrecto o esta vacío.", "Ok");
            }
        }

        private bool ComprobarCampos()
        {
            if (String.IsNullOrWhiteSpace(nombreEntry.Text))
            {
                MostrarError("El campo nombre no puede estar vacío.");
                return false;
            }
            if (String.IsNullOrWhiteSpace(apellidosEntry.Text))
            {
                MostrarError("El campo apellidos no puede estar vacío.");
                return false;
            }
            if(!(fechaDeNacimientoUsuario.AddYears(18) <= DateTime.Today))
            {
                MostrarError("Debe ser mayor de 18 años para poder continuar.");
                return false;
            }

            return true;
        }

        private void MostrarError(string error)
        {
            errorLabel.Text = error;
            errorLabel.TextColor = Color.Red;
            errorLabel.IsVisible = true;
        }

        private void OcultarError()
        {
            errorLabel.IsVisible = false;
        }

        public async void ProcesarCrearInspeccion(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewDatosInspeccion(usuario));
        }

        public async void ProcesarCrearPlantilla(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewDatosPlantilla(null, null)));
        }

        public async void HandleItemTapped(object sender, ItemTappedEventArgs e)
        {
            string idSeleccionado = ((InspeccionListViewModel)((ListView)sender).SelectedItem).Id;

            foreach(Inspeccion i in inspecciones)
            {
                if (i.IdInspeccion.ToString() == idSeleccionado)
                {
                    await Navigation.PushAsync(new ViewInformacionInspeccion(i));
                }
            }
        }

        public void ProcesarFechaNacimiento(object sender, DateChangedEventArgs e)
        {
            fechaDeNacimientoUsuario = fechaNacimientoPicker.Date;
        }
    }
}

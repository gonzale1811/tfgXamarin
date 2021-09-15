using System;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace InspectionManagerTest
{
    [TestFixture(Platform.Android)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        public void TestA_RegistrarUsuario()
        {
            app.Tap(x => x.Marked("testRegistrarseButton"));

            DateTime fecha = new DateTime(1980, 5, 16);
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_DNI_VACIO");

            app.Query(x => x.Marked("testRegistrarDniEntry").Invoke("setText", "1234876"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_DNI_CORTO");

            app.ClearText(x => x.Marked("testRegistrarDniEntry"));
            app.Query(x => x.Marked("testRegistrarDniEntry").Invoke("setText", "1234876543"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_DNI_LARGO");

            app.ClearText(x => x.Marked("testRegistrarDniEntry"));
            app.Query(x => x.Marked("testRegistrarDniEntry").Invoke("setText", "123487659"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_DNI_FALTA_LETRA");

            app.ClearText(x => x.Marked("testRegistrarDniEntry"));
            app.Query(x => x.Marked("testRegistrarDniEntry").Invoke("setText", "1234876TT"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_DNI_SOBRAN_LETRAS");

            app.ClearText(x => x.Marked("testRegistrarDniEntry"));
            app.Query(x => x.Marked("testRegistrarDniEntry").Invoke("setText", "12348765G"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_NOMBRE_VACIO");

            app.Query(x => x.Marked("testRegistrarNombreEntry").Invoke("setText", "Test"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_APELLIDOS_VACIOS");

            app.Query(x => x.Marked("testRegistrarApellidosEntry").Invoke("setText", "Prueba Automatizada"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_EMAIL_VACIO");

            app.Query(x => x.Marked("testRegistrarUsernameEntry").Invoke("setText", "test@inspectionmanager.com"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_EMAIL_VACIO");

            app.Query(x => x.Marked("testRegistrarPasswordEntry").Invoke("setText", "Ab32Zp8"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_CONFIRMAR_PASSWORD_VACIO");

            app.Query(x => x.Marked("testRegistrarPasswordConfirmEntry").Invoke("setText", "b32Zp8"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_CONFIRMAR_PASSWORD_DISTINTO_VACIO");

            app.Query(x => x.Marked("testRegistrarPasswordConfirmEntry").Invoke("setText", "Ab32Zp8"));
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_FECHA_NACIMIENTO_VACIA");

            app.Tap(x => x.Marked("testRegistrarFechaNacimientoPicker"));
            Thread.Sleep(1000);
            SetDatePicker(DateTime.Today.AddYears(-18).AddDays(1));
            Thread.Sleep(1000);
            app.Tap(x => x.Marked("testCompletarRegistroButton")); app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_MENOR_DE_EDAD_VACIA");

            app.Tap(x => x.Marked("testRegistrarFechaNacimientoPicker"));
            Thread.Sleep(1000);
            SetDatePicker(DateTime.Today.AddYears(-18).AddMonths(-1).AddDays(-1));
            Thread.Sleep(1000);
            app.Tap(x => x.Marked("testCompletarRegistroButton"));
            Thread.Sleep(1000);

            app.Screenshot("RegistradoUsusario");
        }

        [Test]
        public void TestB_Login()
        {
            app.Tap(x => x.Marked("testLoginButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_CORREO_VACIO");

            app.Query(e => e.Marked("testUsernameEntry").Invoke("setText", "test@inspectionmanager.com"));
            app.Tap(x => x.Marked("testLoginButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_PASSWORD_VACIO");

            app.Query(e => e.Marked("testPasswordEntry").Invoke("setText", "Ab32Zp8"));

            app.Tap(x => x.Marked("testLoginButton"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("Mis Inspecciones"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("Mi Perfil"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("Nueva Inspeccion"));
            Thread.Sleep(1000);
        }

        [Test]
        public void TestC_CrearPlantilla()
        {
            Login();
            Thread.Sleep(1000);
            app.Tap(x => x.Marked("testAddPlantillaButton"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testAddNuevoBloquePlantillaButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_NOMBRE_PLANTILLA_VACIO");
            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);

            app.Query(x => x.Marked("testNombrePlantillaEntry").Invoke("setText", "Plantilla Test"));
            app.Tap(x => x.Marked("testAddNuevoBloquePlantillaButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_TIPO_TRABAJO_PLANTILLA_VACIO");
            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testPickerTipoPlantilla"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("Oficina"));
            Thread.Sleep(1000);
            int contBloques;
            int contPreguntas;

            for (contBloques = 1; contBloques < 3; contBloques++)
            {
                app.Tap(x => x.Marked("testAddNuevoBloquePlantillaButton"));
                Thread.Sleep(1000);

                app.Tap(x => x.Marked("testCrearNuevoBloquePlantillaButton"));
                Thread.Sleep(1000);
                app.Screenshot("ERROR_NOMBRE_BLOQUE_PLANTILLA_VACIO");
                app.Tap(x => x.Text("Ok"));
                Thread.Sleep(1000);

                app.Query(x => x.Marked("testNuevoBloquePlantillaEntry").Invoke("setText", "BloqueTest" + contBloques));
                app.Tap(x => x.Marked("testCrearNuevoBloquePlantillaButton"));
                Thread.Sleep(1000);

                for (contPreguntas = 1; contPreguntas < 4; contPreguntas++)
                {
                    if (contPreguntas == 1)
                    {
                        app.Tap(x => x.Marked("testCrearNuevaPreguntaBloqueButton"));
                        Thread.Sleep(1000);
                        app.Screenshot("ERROR_ENUNCIADO_PREGUNTA_VACIO");
                        app.Tap(x => x.Text("Ok"));
                        Thread.Sleep(1000);

                        app.Query(x => x.Marked("testEnunciadoPreguntaBloqueEntry").Invoke("setText", "PreguntaTexto-BloqueTest" + contBloques));
                        app.Tap(x => x.Marked("testTipoPreguntaBloquePicker"));
                        Thread.Sleep(1000);
                        app.Tap(x => x.Text("Pregunta de Texto"));
                        Thread.Sleep(1000);
                    }
                    else if (contPreguntas == 2)
                    {
                        app.Query(x => x.Marked("testEnunciadoPreguntaBloqueEntry").Invoke("setText", "PreguntaBoolean-BloqueTest" + contBloques));

                        app.Tap(x => x.Marked("testCrearNuevaPreguntaBloqueButton"));
                        Thread.Sleep(1000);
                        app.Screenshot("ERROR_TIPO_PREGUNTA_VACIO");
                        app.Tap(x => x.Text("Ok"));
                        Thread.Sleep(1000);

                        app.Tap(x => x.Marked("testTipoPreguntaBloquePicker"));
                        Thread.Sleep(1000);
                        app.Tap(x => x.Text("Pregunta Verdadero/Falso"));
                        Thread.Sleep(1000);
                    }
                    else if (contPreguntas == 3)
                    {
                        app.Query(x => x.Marked("testEnunciadoPreguntaBloqueEntry").Invoke("setText", "PreguntaValor-BloqueTest" + contBloques));
                        app.Tap(x => x.Marked("testTipoPreguntaBloquePicker"));
                        Thread.Sleep(1000);
                        app.Tap(x => x.Text("Pregunta Numérica"));
                        Thread.Sleep(1000);
                    }

                    app.Tap(x => x.Marked("testCrearNuevaPreguntaBloqueButton"));
                    Thread.Sleep(1000);
                }

                if (contBloques == 2)
                {
                    app.Tap(x => x.Marked("testCambiarNombreNuevoBloquePlantilla"));
                    Thread.Sleep(1000);
                    app.Query(x => x.Marked("testNuevoBloquePlantillaEntry").Invoke("setText", "BloqueTestEditado" + contBloques));
                    app.Tap(x => x.Marked("testCrearNuevoBloquePlantillaButton"));
                    Thread.Sleep(1000);
                }

                if (contBloques == 1)
                {
                    app.Tap(x => x.Marked("testCancelarNuevaPlantillaBloqueButton"));
                    Thread.Sleep(1000);
                    app.Tap(x => x.Text("No"));
                }

                app.Tap(x => x.Marked("testGuardarNuevaPlantillaBloqueButton"));
                Thread.Sleep(1000);
            }

            app.Tap(x => x.Marked("testCancelarDatosNuevaPlantillaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Marked("No"));
            Thread.Sleep(1000);
            app.Tap(x => x.Marked("testGuardarNuevaPlantillaButton"));
            Thread.Sleep(1000);
        }

        [Test]
        public void TestD_EditarPerfilDeUsuario()
        {
            Login();

            app.Tap(x => x.Text("Mi Perfil"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testEditarPerfilButton"));
            Thread.Sleep(1000);

            app.Query(x => x.Marked("testNombreDatosPerfilEntry").Invoke("setText", ""));
            app.Tap(x => x.Marked("testGuardarPerfilEditadoButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_NOMBRE_EDITADO_PERFIL_VACIO");

            app.Query(x => x.Marked("testNombreDatosPerfilEntry").Invoke("setText", "TestEditado"));
            app.Query(x => x.Marked("testApellidosDatosPerfilEntry").Invoke("setText", ""));
            app.Tap(x => x.Marked("testGuardarPerfilEditadoButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_APELLIDOS_EDITADOS_PERFIL_VACIOS");

            app.Query(x => x.Marked("testApellidosDatosPerfilEntry").Invoke("setText", "PruebaEdit AutomatizadaEdit"));
            app.Tap(x => x.Marked("testFechaDeNacimientoDatosPerfilEntry"));
            Thread.Sleep(1000);
            SetDatePicker(DateTime.Today.AddYears(-18).AddMonths(-1).AddDays(1));
            app.Tap(x => x.Marked("testGuardarPerfilEditadoButton"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_MENOR_DE_EDAD_PERFIL");
            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);

            app.Query(x => x.Marked("testApellidosDatosPerfilEntry").Invoke("setText", "PruebaEdit AutomatizadaEdit"));
            app.Tap(x => x.Marked("testFechaDeNacimientoDatosPerfilEntry"));
            Thread.Sleep(1000);
            SetDatePicker(DateTime.Today.AddYears(-18).AddMonths(-1));
            app.Tap(x => x.Marked("testGuardarPerfilEditadoButton"));
            Thread.Sleep(1000);
        }

        [Test]
        public void TestE_CrearInspeccion()
        {
            Login();

            app.Tap(x => x.Marked("testAddInspeccionButton"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testSeleccionarPlantillaNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Screenshot("ERROR_NOMBRE_NUEVA_INSPECCION_VACIO");

            app.Query(x => x.Marked("testNombreNuevaInspeccionEntry").Invoke("setText", "Inspeccion Automatizada"));
            app.Tap(x => x.Marked("testSeleccionarPlantillaNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Screenshot("ERROR_CAllE_NUEVA_INSPECCION_VACIO");

            app.Query(x => x.Marked("testCalleNuevaInspeccionEntry").Invoke("setText", "Calle Automatizada"));
            app.Tap(x => x.Marked("testSeleccionarPlantillaNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Screenshot("ERROR_LOCALIDAD_NUEVA_INSPECCION_VACIA");

            app.Query(x => x.Marked("testLocalidadNuevaInspeccionEntry").Invoke("setText", "Localidad Automatizada"));
            app.Tap(x => x.Marked("testSeleccionarPlantillaNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Screenshot("ERROR_CODIGO_POSTAL_NUEVA_INSPECCION_VACIO");

            app.Query(x => x.Marked("testCodigoPostalNuevaInspeccionEntry").Invoke("setText", "1234"));
            app.Tap(x => x.Marked("testSeleccionarPlantillaNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Screenshot("ERROR_CODIGO_POSTAL_NUEVA_INSPECCION_DEBE_TENER_5_CARACTERES");

            app.Query(x => x.Marked("testCodigoPostalNuevaInspeccionEntry").Invoke("setText", "123456"));
            app.Tap(x => x.Marked("testSeleccionarPlantillaNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Screenshot("ERROR_CODIGO_POSTAL_NUEVA_INSPECCION_DEBE_TENER_5_CARACTERES");

            app.Query(x => x.Marked("testCodigoPostalNuevaInspeccionEntry").Invoke("setText", "12345"));
            app.Tap(x => x.Marked("testSeleccionarPlantillaNuevaInspeccionButton"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testCancelarPlantillaNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Tap(x => x.Text("Si"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testAddInspeccionButton"));
            Thread.Sleep(3000);

            app.Query(x => x.Marked("testNombreNuevaInspeccionEntry").Invoke("setText", "Inspeccion Automatizada"));
            app.Query(x => x.Marked("testCalleNuevaInspeccionEntry").Invoke("setText", "Calle Automatizada"));
            app.Query(x => x.Marked("testNumeroNuevaInspeccionEntry").Invoke("setText", "15"));
            app.Query(x => x.Marked("testLocalidadNuevaInspeccionEntry").Invoke("setText", "Localidad Automatizada"));
            app.Query(x => x.Marked("testCodigoPostalNuevaInspeccionEntry").Invoke("setText", "12345"));

            app.Tap(x => x.Marked("testCancelarNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Tap(x => x.Text("No"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testSeleccionarPlantillaNuevaInspeccionButton"));
            Thread.Sleep(3000);

            app.Tap(x => x.Text("Plantilla Test"));
            Thread.Sleep(3000);

            app.Tap(x => x.Text("BloqueTest1"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testSeleccionarPuestoTrabajoNuevoBloqueInspeccionButton"));
            Thread.Sleep(3000);
            app.Screenshot("ERROR_SE_DEBE_ELEGIR_PUESTO_DE_TRABAJO");
            app.Tap(x => x.Text("Ok"));

            app.Tap(x => x.Marked("testPuestoTrabajoBloqueNuevaInspeccionPicker"));
            Thread.Sleep(3000);
            app.Tap(x => x.Text("Secretario"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testSeleccionarPuestoTrabajoNuevoBloqueInspeccionButton"));
            Thread.Sleep(3000);

            app.Query(x => x.Marked("testPreguntaBooleanBloqueNuevaInspeccion1CheckBox").Invoke("setChecked", true));

            app.Query(x => x.Marked("testPreguntaValorBloqueNuevaInspeccion1Entry").Invoke("setText", "85"));

            app.Tap(x => x.Marked("testCancelarPreguntaBloqueNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Tap(x => x.Text("No"));

            app.Tap(x => x.Marked("testAceptarPreguntaBloqueNuevaInspeccionButton"));
            Thread.Sleep(3000);

            app.Tap(x => x.Text("BloqueTest1"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testPuestoTrabajoBloqueNuevaInspeccionPicker"));
            Thread.Sleep(3000);
            app.Tap(x => x.Text("Secretario"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testSeleccionarPuestoTrabajoNuevoBloqueInspeccionButton"));
            Thread.Sleep(3000);
            app.Screenshot("ERROR_ELEGIDO_EL_MISMO_PUESTO_MAS_DE_UNA_VEZ_PARA_UN_BLOQUE");

            app.Tap(x => x.Marked("Ok"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testPuestoTrabajoBloqueNuevaInspeccionPicker"));
            Thread.Sleep(3000);
            app.Tap(x => x.Text("Oficinista"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testSeleccionarPuestoTrabajoNuevoBloqueInspeccionButton"));
            Thread.Sleep(3000);

            app.Query(x => x.Marked("testPreguntaTextoBloqueNuevaInspeccion1Entry").Invoke("setText", "RespuestaTextoBloque1"));

            app.Query(x => x.Marked("testPreguntaValorBloqueNuevaInspeccion1Entry").Invoke("setText", "85"));

            app.Tap(x => x.Marked("testAceptarPreguntaBloqueNuevaInspeccionButton"));
            Thread.Sleep(3000);

            app.Tap(x => x.Text("BloqueTestEditado2"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testPuestoTrabajoBloqueNuevaInspeccionPicker"));
            Thread.Sleep(3000);
            app.Tap(x => x.Text("Secretario"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testSeleccionarPuestoTrabajoNuevoBloqueInspeccionButton"));
            Thread.Sleep(3000);

            app.Query(x => x.Marked("testPreguntaTextoBloqueNuevaInspeccion1Entry").Invoke("setText", "RespuestaTextoBloque1"));

            app.Query(x => x.Marked("testPreguntaBooleanBloqueNuevaInspeccion1CheckBox").Invoke("setChecked", true));

            app.Tap(x => x.Marked("testAceptarPreguntaBloqueNuevaInspeccionButton"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testCancelarBloqueNuevaInspeccionButton"));
            Thread.Sleep(3000);
            app.Tap(x => x.Text("No"));
            Thread.Sleep(3000);

            app.Tap(x => x.Marked("testFinalizarNuevaInspeccionButton"));
            Thread.Sleep(3000);

            app.Tap(x => x.Text("Mis Inspecciones"));
            Thread.Sleep(3000);

            app.Screenshot("FINALIZADO_TEST_CREAR_INSPECCION");
        }

        [Test]
        public void TestF_ConsultarUnaInspeccionYEditarSusDatos()
        {
            Login();

            app.Tap(x => x.Text("Mis Inspecciones"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("Inspeccion Automatizada"));
            Thread.Sleep(1000);

            app.Screenshot("INSPECCION_PREVIAMENTE_REALIZADA");

            app.Tap(x => x.Marked("testEditarInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Query(x => x.Marked("testNombreInspeccionListaEntry").Invoke("setText", ""));

            app.Tap(x => x.Marked("testGuardarEdicionInspeccionListaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("Ok"));
            app.Screenshot("ERROR_EDITAR_INSPECCION_NOMBRE_VACIO");

            app.Query(x => x.Marked("testNombreInspeccionListaEntry").Invoke("setText", "Inspeccion Automatizada Editada"));
            app.Tap(x => x.Marked("testFechaInicioInspeccionListaDatePicker"));
            Thread.Sleep(1000);
            SetDatePicker(DateTime.Today.AddMonths(1).AddDays(10));
            Thread.Sleep(1000);

            app.Query(x => x.Marked("testCalleInspeccionListaEntry").Invoke("setText", ""));

            app.Tap(x => x.Marked("testGuardarEdicionInspeccionListaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_EDITAR_INSPECCION_CALLE_VACIA");

            app.Query(x => x.Marked("testCalleInspeccionListaEntry").Invoke("setText", "Calle Automatizada Editada"));
            app.Query(x => x.Marked("testNumeroInspeccionListaEntry").Invoke("setText", ""));
            app.Query(x => x.Marked("testLocalidadInspeccionListaEntry").Invoke("setText", ""));

            app.Tap(x => x.Marked("testGuardarEdicionInspeccionListaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_EDITAR_INSPECCION_LOCALIDAD_VACIA");

            app.Query(x => x.Marked("testLocalidadInspeccionListaEntry").Invoke("setText", "LocalidadAutomatizadaEditada"));
            app.Query(x => x.Marked("testCodigoPostalInspeccinListaEntry").Invoke("setText", ""));

            app.Tap(x => x.Marked("testGuardarEdicionInspeccionListaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_EDITAR_INSPECCION_CODIGO_POSTAL_VACIO");

            app.Query(x => x.Marked("testCodigoPostalInspeccinListaEntry").Invoke("setText", "1234"));

            app.Tap(x => x.Marked("testGuardarEdicionInspeccionListaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_EDITAR_INSPECCION_CODIGO_POSTAL_CORTO");

            app.Query(x => x.Marked("testCodigoPostalInspeccinListaEntry").Invoke("setText", "123456"));

            app.Tap(x => x.Marked("testGuardarEdicionInspeccionListaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);
            app.Screenshot("ERROR_EDITAR_INSPECCION_CODIGO_POSTAL_LARGO");

            app.Query(x => x.Marked("testCodigoPostalInspeccinListaEntry").Invoke("setText", "54321"));

            app.Tap(x => x.Marked("testGuardarEdicionInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Screenshot("TEST_COMPLETADO");
        }

        [Test]
        public void TestG_DescargarUnaInspeccion()
        {
            Login();

            app.Tap(x => x.Text("Mis Inspecciones"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("Inspeccion Automatizada Editada"));
            Thread.Sleep(1000);

            app.Screenshot("INSPECCION_PREVIAMENTE_REALIZADA");

            app.Tap(x => x.Marked("testDescargarInspeccionListaButton"));
            Thread.Sleep(3000);

            app.Screenshot("ARCHIVO_PDF_GENERADO");

            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);
        }

        [Test]
        public void TestH_EditarRespuestaDeUnaInspeccion()
        {
            Login();

            app.Tap(x => x.Text("Mis Inspecciones"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("Inspeccion Automatizada Editada"));
            Thread.Sleep(1000);

            app.Screenshot("INSPECCION_PREVIAMENTE_REALIZADA");

            app.Tap(x => x.Marked("testVerBloquesInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("BloqueTest1"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testEditarPreguntasBloqueInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Query(x => x.Marked("testPreguntaTextoBloqueInspeccionLista1Entry").Invoke("setText", "RespuestaEditada"));

            app.Query(x => x.Marked("testPreguntaBooleanBloqueInspeccionLista1Checkbox").Invoke("setChecked", false));

            app.Tap(x => x.Marked("testGuardarEdicionPreguntasBloqueInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testCancelarPreguntasBloqueInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("BloqueTestEditado2"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testEditarPreguntasBloqueInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Query(x => x.Marked("testPreguntaBooleanBloqueInspeccionLista1Checkbox").Invoke("setChecked", false));

            app.Query(x => x.Marked("testPreguntaValorBloqueInspeccionLista1Entry").Invoke("setText", "58"));

            app.Tap(x => x.Marked("testGuardarEdicionPreguntasBloqueInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testCancelarPreguntasBloqueInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("BloqueTest1"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testVerFotosPreguntasBloqueInspeccionListaButton"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testCargarFotoSeleccionadaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("Ok"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testSelectorFotosPicker"));
            Thread.Sleep(1000);
            app.Screenshot("AQUI_APARECERIAN_IMAGENES_PERO_CON_NUNIT_NO_SE_PUEDEN_GENERE");
            app.Tap(x => x.Text("Cancel"));
            Thread.Sleep(1000);
        }

        [Test]
        public void TestI_EliminarUnaInspeccion()
        {
            Login();

            app.Tap(x => x.Text("Mis Inspecciones"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("Inspeccion Automatizada Editada"));
            Thread.Sleep(1000);

            app.Screenshot("INSPECCION_PREVIAMENTE_REALIZADA");

            app.Tap(x => x.Marked("testEliminarInspeccionListaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("No"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testEliminarInspeccionListaButton"));
            Thread.Sleep(1000);
            app.Tap(x => x.Text("Si"));
            Thread.Sleep(1000);

            app.Tap(x => x.Text("Mis Inspecciones"));
            Thread.Sleep(1000);

            app.Screenshot("INSPECCION_ELIMINADA");
        }

        [Test]
        public void TestJ_CerrarSesion()
        {
            Login();

            app.Tap(x => x.Text("Mi Perfil"));
            Thread.Sleep(1000);

            app.Tap(x => x.Marked("testCerrarSesionButton"));
            Thread.Sleep(1000);

            app.Screenshot("CERRADA_SESION_CORRECTAMENTE");
        }

        public void SetDatePicker(DateTime fecha)
        {
            app.Query(x => x.Class("DatePicker").Invoke("updateDate", fecha.Year, fecha.Month, fecha.Day));
            app.Tap("OK");
        }

        public void Login()
        {
            app.Query(e => e.Marked("testUsernameEntry").Invoke("setText", "test@inspectionmanager.com"));
            app.Query(e => e.Marked("testPasswordEntry").Invoke("setText", "Ab32Zp8"));

            app.Tap(x => x.Marked("testLoginButton"));
            Thread.Sleep(1000);
        }
    }
}

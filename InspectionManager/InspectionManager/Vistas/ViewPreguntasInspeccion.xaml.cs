using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewPreguntasInspeccion : ContentPage
    {
        private IFirebaseConsultService consult;
        private CameraService camera;

        private Inspeccion inspeccion;
        private Bloque bloque;
        private List<IPregunta<string>> preguntasString;
        private List<IPregunta<bool>> preguntasBoolean;
        private List<IPregunta<int>> preguntasInt;
        private int contadorDeFotos;

        private readonly string MENSAJE = "Responde aqui a la pregunta";

        public ViewPreguntasInspeccion(Inspeccion inspeccionRecibida, Bloque bloqueRecibido)
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();
            camera = new CameraService();

            inspeccion = inspeccionRecibida;
            bloque = bloqueRecibido;
            contadorDeFotos = bloque.Fotografias.Count + 1;

            preguntasString = consult.GetPreguntasTextoByBloqueInspeccion(bloque);
            preguntasBoolean = consult.GetPreguntasBooleanByBloqueInspeccion(bloque);
            preguntasInt = consult.GetPreguntasValorByBloqueInspeccion(bloque);

            Label tituloTexto = new Label();
            tituloTexto.Text = "Preguntas de texto";
            tituloTexto.HorizontalTextAlignment = TextAlignment.Center;
            tituloTexto.FontSize = 24;
            layoutPreguntas.Children.Add(tituloTexto);

            foreach (IPregunta<string> pregunta in preguntasString)
            {
                Label texto = new Label();
                texto.Text = pregunta.Nombre;
                Entry campoRespuesta = new Entry();
                campoRespuesta.Text = "Respuesta: "+pregunta.RespuestaPregunta.ValorRespuesta;
                campoRespuesta.Keyboard = Keyboard.Text;
                campoRespuesta.IsEnabled = false;
                layoutPreguntas.Children.Add(texto);
                layoutPreguntas.Children.Add(campoRespuesta);
            }

            Label tituloBoolean = new Label();
            tituloBoolean.Text = "Preguntas de verdadero/falso";
            tituloBoolean.HorizontalTextAlignment = TextAlignment.Center;
            tituloBoolean.FontSize = 24;
            Label explicacion = new Label();
            explicacion.Text = "Marque los check si es verdadero.";
            explicacion.FontSize = 16;
            layoutPreguntas.Children.Add(tituloBoolean);
            layoutPreguntas.Children.Add(explicacion);

            foreach (IPregunta<bool> pregunta1 in preguntasBoolean)
            {
                Label texto = new Label();
                texto.Text = pregunta1.Nombre;
                CheckBox respuesta = new CheckBox();
                respuesta.IsChecked = pregunta1.RespuestaPregunta.ValorRespuesta;
                respuesta.IsEnabled = false;
                layoutPreguntas.Children.Add(texto);
                layoutPreguntas.Children.Add(respuesta);
            }

            Label tituloValor = new Label();
            tituloValor.Text = "Preguntas numericas";
            tituloValor.HorizontalTextAlignment = TextAlignment.Center;
            tituloValor.FontSize = 24;
            layoutPreguntas.Children.Add(tituloValor);

            foreach (IPregunta<int> pregunta2 in preguntasInt)
            {
                Label texto = new Label();
                texto.Text = pregunta2.Nombre;
                Entry campoRespuesta = new Entry();
                campoRespuesta.Text = "Respuesta: " + pregunta2.RespuestaPregunta.ValorRespuesta;
                campoRespuesta.Keyboard = Keyboard.Numeric;
                campoRespuesta.IsEnabled = false;
                layoutPreguntas.Children.Add(texto);
                layoutPreguntas.Children.Add(campoRespuesta);
            }

            StackLayout botones = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            Button cancelar = new Button
            {
                Text = "Cancelar",
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            Button foto = new Button
            {
                ImageSource = "add_foto_black.png",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Button editar = new Button
            {
                Text = "Editar",
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };

            Button guardar = new Button
            {
                Text = "Guardar",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                IsEnabled = false,
                IsVisible = false
            };

            cancelar.Clicked += ProcesarCancelar;
            foto.Clicked += ProcesarFotografia;
            editar.Clicked += ProcesarEditar;
            guardar.Clicked += ProcesarGuardar;

            botones.Children.Add(cancelar);
            botones.Children.Add(foto);
            botones.Children.Add(editar);
            botones.Children.Add(guardar);

            StackLayout verFotos = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Button fotos = new Button
            {
                Text = "Ver fotografías",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            fotos.Clicked += ProcesarVerFotos;

            verFotos.Children.Add(fotos);

            layoutPreguntas.Children.Add(botones);
            layoutPreguntas.Children.Add(verFotos);
        }

        private void ProcesarGuardar(object sender, EventArgs e)
        {
            int numeroPreguntasTexto = preguntasString.Count;
            int numeroPreguntasBoolean = preguntasBoolean.Count;
            int numeroPreguntasValor = preguntasInt.Count;

            List<IPregunta<string>> preguntasTextoRespondidas = new List<IPregunta<string>>();
            List<IPregunta<bool>> preguntasBooleanRespondidas = new List<IPregunta<bool>>();
            List<IPregunta<int>> preguntasIntRespondidas = new List<IPregunta<int>>();

            foreach (View v in layoutPreguntas.Children)
            {
                Type tipo = v.GetType();

                if (tipo.Equals(typeof(Entry)))
                {
                    var respuesta = (Entry)v;
                    if (numeroPreguntasTexto > 0)
                    {

                        IPregunta<string> preguntaTextoRespondida = new PreguntaTexto(preguntasString[numeroPreguntasTexto - 1].IdPregunta,preguntasString[numeroPreguntasTexto - 1].Nombre);
                        preguntaTextoRespondida.Responder(respuesta.Text);
                        preguntasTextoRespondidas.Add(preguntaTextoRespondida);
                        numeroPreguntasTexto--;
                    }
                    else
                    {
                        IPregunta<int> preguntaIntRespondida = new PreguntaValor(preguntasInt[numeroPreguntasValor - 1].IdPregunta, preguntasInt[numeroPreguntasValor - 1].Nombre);
                        var respuestaEntry = respuesta.Text;
                        if (respuestaEntry != null)
                        {
                            preguntaIntRespondida.Responder(Int32.Parse(respuesta.Text));
                        }
                        else
                        {
                            preguntaIntRespondida.Responder(0);
                        }
                        preguntasIntRespondidas.Add(preguntaIntRespondida);
                        numeroPreguntasBoolean--;
                    }
                }
                else if (tipo.Equals(typeof(CheckBox)))
                {
                    var respuesta = (CheckBox)v;
                    IPregunta<bool> preguntaBooleanRespondida = new PreguntaBoolean(preguntasBoolean[numeroPreguntasBoolean - 1].IdPregunta, preguntasBoolean[numeroPreguntasBoolean - 1].Nombre);
                    preguntaBooleanRespondida.Responder(respuesta.IsChecked);
                    preguntasBooleanRespondidas.Add(preguntaBooleanRespondida);
                    numeroPreguntasBoolean--;
                }
            }

            consult.ActualizarPreguntasTextoInspeccion(preguntasTextoRespondidas);
            consult.ActualizarPreguntasBooleanInspeccion(preguntasBooleanRespondidas);
            consult.ActualizarPreguntasValorInspeccion(preguntasIntRespondidas);

            preguntasString = preguntasTextoRespondidas;
            preguntasBoolean = preguntasBooleanRespondidas;
            preguntasInt = preguntasIntRespondidas;

            numeroPreguntasTexto = preguntasString.Count;
            numeroPreguntasBoolean = preguntasBoolean.Count;
            numeroPreguntasValor = preguntasInt.Count;

            int contTexto = 0;
            int contBool = 0;
            int contInt = 0;

            foreach (View v in layoutPreguntas.Children)
            {
                Type tipo = v.GetType();

                if (tipo.Equals(typeof(Entry)))
                {
                    var elemento = (Entry)v;

                    if (numeroPreguntasTexto > 0)
                    {
                        elemento.Text = "Respuesta: " + preguntasString[contTexto].RespuestaPregunta.ValorRespuesta;
                        elemento.IsEnabled = false;
                        contTexto++;
                        numeroPreguntasTexto--;
                    }
                    else
                    {
                        elemento.Text = "Respuesta: " + preguntasInt[contInt].RespuestaPregunta.ValorRespuesta;
                        elemento.IsEnabled = false;
                        contInt++;
                        numeroPreguntasValor--;
                    }
                }
                else if (tipo.Equals(typeof(CheckBox)))
                {
                    var elemento = (CheckBox)v;
                    elemento.IsChecked = preguntasBoolean[contBool].RespuestaPregunta.ValorRespuesta;
                    elemento.IsEnabled = false;
                    contBool++;
                    numeroPreguntasBoolean--;
                }
                else if (tipo.Equals(typeof(StackLayout)))
                {
                    var elemento = (StackLayout)v;

                    foreach (View vS in elemento.Children)
                    {
                        Type tipoS = vS.GetType();

                        if (tipoS.Equals(typeof(Button)))
                        {
                            var boton = (Button)vS;

                            if (boton.Text == "Editar")
                            {
                                boton.IsEnabled = true;
                                boton.IsVisible = true;
                            }
                            else if (boton.Text == "Guardar")
                            {
                                boton.IsVisible = false;
                                boton.IsEnabled = false;
                            }
                        }
                    }
                }
            }
        }

        public async void ProcesarVerFotos(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewFotos(inspeccion,bloque)));
        }

        public void ProcesarEditar(object sender, EventArgs e)
        {
            foreach(View v in layoutPreguntas.Children)
            {
                Type tipo = v.GetType();

                if (tipo.Equals(typeof(Entry)))
                {
                    var elemento = (Entry)v;

                    string separacion = " ";
                    char caracter = separacion[0];
                    elemento.Text = elemento.Text.Split(caracter)[1];
                    elemento.IsEnabled = true;
                }else if (tipo.Equals(typeof(CheckBox)))
                {
                    var elemento = (CheckBox)v;
                    elemento.IsEnabled = true;
                }else if (tipo.Equals(typeof(StackLayout)))
                {
                    var elemento = (StackLayout)v;

                    foreach(View vS in elemento.Children)
                    {
                        Type tipoS = vS.GetType();

                        if (tipoS.Equals(typeof(Button)))
                        {
                            var boton = (Button)vS;

                            if (boton.Text == "Editar")
                            {
                                boton.IsEnabled = false;
                                boton.IsVisible = false;
                            }else if(boton.Text == "Guardar")
                            {
                                boton.IsVisible = true;
                                boton.IsEnabled = true;
                            }
                        }
                    }
                }
            }


        }

        public async void ProcesarFotografia(object sender, EventArgs e)
        {
            try
            {
                var resultado = await camera.TakePhoto();
                var subida = resultado;

                var url = consult.UploadFoto(inspeccion.IdInspeccion.ToString(), bloque.IdBloque.ToString() + "_" + bloque.PuestoTrabajo, contadorDeFotos, subida);

                bloque.Fotografias.Add(url);

                contadorDeFotos++;
                await DisplayAlert("Correcto", "Fotografia realizada correctamente", "Ok");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Se ha producido un error al realizar la foto.", "Ok");
            }
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            List<Bloque> bloquesInspeccion = consult.GetBloquesByInspeccion(inspeccion);
            await Navigation.PushModalAsync(new NavigationPage(new ViewBloquesInspeccion(inspeccion, bloquesInspeccion)));
        }
    }
}

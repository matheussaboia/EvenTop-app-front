using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Eventop.Models;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Diagnostics;
using Plugin.Media;
using Eventop.Util;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Eventop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        //public Item Item { get; set; }

        private Usuario usuarioL;

        private static byte[] imagemPrincipalDoEvento;
        private static Entry nomeDoEvento;
        private static Picker categoriaDoEvento;
        private static Entry descricaoDoEvento;
        private static DatePicker dataDoEvento;
        private static TimePicker horarioDoEvento;


        public NewItemPage()
        {
            if (new Sessao().CarregaLogin() != null){
                usuarioL = new Sessao().CarregaLogin();
            }
            else {
                Navigation.PushModalAsync(new LoginPage(), true);
            }

            InitializeComponent();

            nomeDoEvento = txtNomeDoEvento;
            categoriaDoEvento = pckCategoriaDoEvento;
            descricaoDoEvento = txtDescricaoDoEvento;
            dataDoEvento = dtPckDataDoEvento;
            horarioDoEvento = tmPckHorarioDoEvento;

            imgPrincipalEvento.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => Button_Clicked_1(null, null)),
            });

            /*Item = new Item
            {
                //Text = "Item name",
                //Description = "This is an item description."
            };

            BindingContext = this;
            */
        }
        

        async void Save_Clicked(object sender, EventArgs e)
        {
            //MessagingCenter.Send(this, "AddItem", Item);
            // await Navigation.PopModalAsync();


            //Validações de campos
            var propriedadesObrigatorias = new
            {
                imagemPrincipalDoEvento = imagemPrincipalDoEvento.ToString(),
                NomeDoEvento = nomeDoEvento.Text,
                CategoriaDoEvento = categoriaDoEvento.SelectedItem,
                DescricaoDoEvento = descricaoDoEvento.Text,
                DataDoEvento = Convert.ToDateTime(dataDoEvento.Date).ToString(),
                HorarioDoEvento = horarioDoEvento.Time.ToString()
            };

            var verificarCamposNaoPreenchidos = new ValidacaoCampos().ValidarCampos(propriedadesObrigatorias);

            if (verificarCamposNaoPreenchidos.Count == 0)
            {
                var dataEHoraDoEvento = Convert.ToDateTime(dataDoEvento.Date.ToString("dd/MM/yyyy") + " " + propriedadesObrigatorias.HorarioDoEvento);

                await DisplayAlert("Ótimo", "Falta pouco para que seu evento: \n" + propriedadesObrigatorias.NomeDoEvento + "\n seja criado ;)", "Próxima etapa");

                //Parametros: ID do criador, nome do evento, categoria e data
                await Navigation.PushAsync(new CadastroEventoSegundaEtapa(usuarioL.Id, imagemPrincipalDoEvento , propriedadesObrigatorias.NomeDoEvento, propriedadesObrigatorias.CategoriaDoEvento.ToString(), propriedadesObrigatorias.DescricaoDoEvento, dataEHoraDoEvento));


                //Evento novoEvento = new Evento {
                //    IdCriadorDoEvento = 1,
                //    NomeDoEvento = nomeDoEvento.Text,
                //    CategoriaDoEvento = categoriaDoEvento.Text,
                //    DataDoEvento = Convert.ToDateTime(dataEHoraDoEvento)
                //};

                //Armazena a conversão do objeto usuário para JSON
                //var json = JsonConvert.SerializeObject(novoEvento);

                //new JsonHelper().enviarPostParaServidor(json, "Eventos") != null

                //if ()
                //{



                return;
                //}
                //else
                //{
                //    await DisplayAlert("Opa, temos um problema", "Tente novamente mais tarde!", "OK");
                //}
            }




        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Ops", "Nenhuma câmera detectada ;/", "OK");
                    return;
                }

                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }

                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                {
                    
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        SaveToAlbum = true
                        //Directory = "Sample",
                        //Name = "test.jpg"
                    });


                    if (file == null)
                        return;

                    await DisplayAlert("Local da imagem", file.Path, "OK");

                    //if (imgPrincipalEvento.Source == null)
                    //{
                    //    Console.WriteLine("MyImage.Source == null ==> OK");
                    //}

                    imgPrincipalEvento.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                }
                else
                {
                    await DisplayAlert("Permissão Negada", "Não é possível tirar fotos.", "OK");
                }

            }
            catch (Exception te)
            {
                Console.WriteLine("EXCEPTION HERE =: " + te);
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Ops", "Nenhuma câmera detectada ;/", "OK");
                    return;
                }

                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }

                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                {

                    var attachment = new
                    {
                        Name = "file_" + DateTime.Now.Ticks + ".jpg",
                        Extension = "jpg"
                    };

                    var file = await CrossMedia.Current.PickPhotoAsync();


                    if (file == null)
                        return;
                    
                    //await DisplayAlert("Local da imagem", file.Path, "OK");

                    /*imgPrincipalEvento.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;

                    });
                    */
                    imagemPrincipalDoEvento = ImageToBinary(file.Path);
                    imgPrincipalEvento.Source = ImageSource.FromStream(() => new MemoryStream(imagemPrincipalDoEvento));
                    
                    imgPrincipalEvento.WidthRequest = 400;
                    imgPrincipalEvento.HeightRequest = 250;
                }
                else {
                    await DisplayAlert("Permissão Negada", "Não é possível tirar fotos.", "OK");
                }

            }
            catch (Exception te)
            {
                Console.WriteLine("EXCEPTION HERE =: " + te);
            }
        }

        public byte[] ImageToBinary(string imagePath)
        {

            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }
    }
}
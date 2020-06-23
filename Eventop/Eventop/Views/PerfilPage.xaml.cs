using Eventop.Models;
using Eventop.Util;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Eventop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilPage : ContentPage
    {
        private Usuario usuarioL = new Sessao().CarregaLogin();
        private static byte[] imagemDoUsuario;

        public PerfilPage()
        {
            InitializeComponent();

            Perfil.Title = "@" + usuarioL.Login;

            DesabilitarCampos();
            CarregarDadosUsuario();
            CarregarImagemDoUsuario();

            imgDoUsuario.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => PegarImagemDaGaleria()),
            });
        }

        protected void CarregarDadosUsuario()
        {
            if (usuarioL.Nome == null)
            {
                txtNomeCompleto.Text = "Não informado";
                txtNomeCompleto.TextColor = Color.Red;
            }
            else
            {
                txtNomeCompleto.Text = usuarioL.Nome;
            }
            txtNomeUsuario.Text = usuarioL.Login;
            txtEmailUsuario.Text = usuarioL.Email;
            if (usuarioL.Whatsapp == null)
            {
                txtWhatsappUsuario.Text = "Não informado";
                txtWhatsappUsuario.TextColor = Color.Red;
            }
            else
            {
                txtWhatsappUsuario.Text = usuarioL.Whatsapp;
            }
            txtDocumentoUsuario.Text = usuarioL.Documento;
            if (usuarioL.IdEndereco == null)
            {
                lblErroEndereco.IsVisible = true;
                lblErroEndereco.TextColor = Color.Red;
            }
        }

        protected void CarregarImagemDoUsuario()
        {
            try
            {
                var respostaDoServidor = new JsonHelper().getParaOServidor("Galeria_Usuario", "id=" + usuarioL.Id);
                List<Galeria> imagemDoUsuario = JsonConvert.DeserializeObject<List<Galeria>>(respostaDoServidor);

                imgDoUsuario.Source = ImageFromBase64(imagemDoUsuario.First().imagemGaleria);
            }
            catch { }

        }

        protected void DesabilitarCampos()
        {
            btnAtualizar.IsVisible = false;
            txtNomeCompleto.IsEnabled = false;
            txtNomeUsuario.IsEnabled = false;
            txtDocumentoUsuario.IsEnabled = false;
            txtEmailUsuario.IsEnabled = false;
            txtWhatsappUsuario.IsEnabled = false;
        }

        protected void HabilitarCampos()
        {
            btnAtualizar.IsVisible = true;
            txtNomeCompleto.IsEnabled = true;
            txtNomeUsuario.IsEnabled = true;
            txtDocumentoUsuario.IsEnabled = true;
            txtEmailUsuario.IsEnabled = true;
            txtWhatsappUsuario.IsEnabled = true;
        }

        protected void Cancel_Clicked(object sender, EventArgs e)
        {
            HabilitarCampos();
        }

        private async void BtnAtualizar_Clicked(object sender, EventArgs e)
        {
            //Atualização do USUARIO
            usuarioL.Nome = txtNomeCompleto.Text;
            usuarioL.Login = txtNomeUsuario.Text;
            usuarioL.Documento = txtDocumentoUsuario.Text;
            usuarioL.Email = txtEmailUsuario.Text;
            usuarioL.Whatsapp = txtWhatsappUsuario.Text;

            var json = JsonConvert.SerializeObject(usuarioL);
            var resposta = new JsonHelper().PutParaOServidor(json, "Usuarios?id=" + usuarioL.Id + "&usuario=" + json);

            //Cadastro da foto na GALERIA
            Galeria imagem = new Galeria();
            imagem.imagemGaleria = imagemDoUsuario;
            imagem.imagemPrincipal = true;

            var jsonGaleria = JsonConvert.SerializeObject(imagem);
            var cadastroImagemUsuario = new JsonHelper().enviarPostParaServidorJson(jsonGaleria, "Galerias");

            Galeria imagemCadastrada = JsonConvert.DeserializeObject<Galeria>(cadastroImagemUsuario);

            //Alocação da foto na GALERIA DO USUARIO
            Galeria_Usuario imgU = new Galeria_Usuario();
            imgU.IdGaleria = imagemCadastrada.idGaleria;
            imgU.IdUsuario = usuarioL.Id;

            var jsonGaleriaUsuario = JsonConvert.SerializeObject(imgU);
            cadastroImagemUsuario = new JsonHelper().enviarPostParaServidorJson(jsonGaleriaUsuario, "Galeria_Usuario");


            if (cadastroImagemUsuario != null)
            {
                await DisplayAlert("Muito bom", "Seu perfil foi atualizado!", "Beleza");
                //DesabilitarCampos();
                await Navigation.PushAsync(new InicioDetalhes(usuarioL.Id, usuarioL.Login));
            }
            else
            {
                await DisplayAlert("Hmm..", "Tivemos um problema", "Tente novamente");
            }
        }

        private async void PegarImagemDaGaleria()
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
                    imagemDoUsuario = ImageToBinary(file.Path);
                    imgDoUsuario.Source = ImageSource.FromStream(() => new MemoryStream(imagemDoUsuario));
                    
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

        public byte[] ImageToBinary(string imagePath)
        {

            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        public static ImageSource ImageFromBase64(byte[] base64picture)
        {
            //byte[] imageBytes = Convert.FromBase64String(base64picture);
            return ImageSource.FromStream(() => new MemoryStream(base64picture));
        }






    }
}
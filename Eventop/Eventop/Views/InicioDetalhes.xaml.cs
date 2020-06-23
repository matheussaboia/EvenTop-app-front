using Eventop.Models;
using Eventop.Util;
using Newtonsoft.Json;
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
    public partial class InicioDetalhes : MasterDetailPage
    {
        private Usuario usuarioL = new Sessao().CarregaLogin();

        public InicioDetalhes(long idDoUsuario, string loginDoUsuario)
        {
            if (usuarioL == null)
            {
                Navigation.PushModalAsync(new LoginPage());
            }

            InitializeComponent();

            CarregarImagemDoUsuario(idDoUsuario);
            lblNomeDoUsuario.Text = loginDoUsuario;

            //MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        protected void CarregarImagemDoUsuario(long idDoUsuario)
        {
            try
            {
                var respostaDoServidor = new JsonHelper().getParaOServidor("Galeria_Usuario", "id=" + idDoUsuario);
                List<Galeria> eventoBuscado = JsonConvert.DeserializeObject<List<Galeria>>(respostaDoServidor);

                imgDoUsuario.Source = ImageFromBase64(eventoBuscado.First().imagemGaleria);
            }
            catch { }

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            /*var item = e.SelectedItem as InicioDetalhesMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
            */
        }

        async void Perfil_Tapped(object sender, EventArgs e) {
            await Navigation.PushAsync(new PerfilPage());
            MasterPage.IsPresented = false;
        }

        private async void MeusEventos_Tapped(object sender, EventArgs e) {
            await Navigation.PushAsync(new MeusEventos(usuarioL.Id, usuarioL.TipoUsuario));
            MasterPage.IsPresented = false;
        }

        private void Avaliacoes_Tapped(object sender, EventArgs e) {

        }

        public static ImageSource ImageFromBase64(byte[] base64picture)
        {
            //byte[] imageBytes = Convert.FromBase64String(base64picture);
            return ImageSource.FromStream(() => new MemoryStream(base64picture));
        }

        private  void Sair_Tapped(object sender, EventArgs e)
        {
            Application.Current.Properties["idUsuario"] = null;

            Application.Current.Properties.Clear();

            Application.Current.SavePropertiesAsync();

            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            });
        }
    }
}
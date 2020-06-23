using Acr.UserDialogs;
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
    public partial class MeusEventos : TabbedPage
    {
        List<Evento_Inicio> listaDeEventosPublicados = new List<Evento_Inicio>();
        List<Evento_Inicio> listaDeEventosInscritos = new List<Evento_Inicio>();

        public MeusEventos(long idUsuario, string tipoDeUsuario)
        {
            InitializeComponent();

            //Carrega eventos em alta
            CarregarEventosPublicados(idUsuario);

            lstvEventosPublicados.RefreshCommand = new Command(() => {
                CarregarEventosPublicados(idUsuario);
                lstvEventosPublicados.IsRefreshing = false;
            });

            //Carrega eventos em alta
            CarregarEventosInscritos(idUsuario);

            lstvEventosInscritos.RefreshCommand = new Command(() => {
                CarregarEventosInscritos(idUsuario);
                lstvEventosInscritos.IsRefreshing = false;
            });

        }

        protected void CarregarEventosPublicados(long idUsuario)
        {

            listaDeEventosPublicados.Clear();

            var respostaDoServidor = new JsonHelper().getParaOServidor("Eventos", "idUsuario=" + idUsuario + "&relatoriosEventos=Publicados");
            List<Evento_Inicio> eventoBuscado = JsonConvert.DeserializeObject<List<Evento_Inicio>>(respostaDoServidor);

            foreach (var item in eventoBuscado)
            {
                Evento_Inicio teste = new Evento_Inicio();
                teste.IdEvento = item.IdEvento;
                teste.IdCriadorDoEvento = item.IdCriadorDoEvento;
                teste.ImgPrincipalDoEventoConvert = ImageFromBase64(item.ImgPrincipalDoEvento);
                teste.IdEnderecoDoEvento = item.IdEnderecoDoEvento;
                teste.NomeDoEvento = item.NomeDoEvento;
                teste.CategoriaDoEvento = item.CategoriaDoEvento;
                teste.DescricaoDoEvento = item.DescricaoDoEvento;
                teste.DataDoEvento = item.DataDoEvento;
                teste.NomeDoAutor = item.NomeDoAutor;
                teste.EmailDoAutor = item.EmailDoAutor;
                teste.WhatsappDoAutor = item.WhatsappDoAutor;
                teste.Cidade = item.Cidade;
                teste.Quantidade = item.Quantidade;
                listaDeEventosPublicados.Add(teste);
            }

            lstvEventosPublicados.ItemsSource = listaDeEventosPublicados;
        }

        protected void CarregarEventosInscritos(long idUsuario)
        {

            listaDeEventosInscritos.Clear();

            var respostaDoServidor = new JsonHelper().getParaOServidor("Eventos", "idUsuario=" + idUsuario + "&relatoriosEventos=Inscritos");
            List<Evento_Inicio> eventoBuscado = JsonConvert.DeserializeObject<List<Evento_Inicio>>(respostaDoServidor);

            foreach (var item in eventoBuscado)
            {
                Evento_Inicio teste = new Evento_Inicio();
                teste.IdEvento = item.IdEvento;
                teste.IdCriadorDoEvento = item.IdCriadorDoEvento;
                teste.ImgPrincipalDoEventoConvert = ImageFromBase64(item.ImgPrincipalDoEvento);
                teste.IdEnderecoDoEvento = item.IdEnderecoDoEvento;
                teste.NomeDoEvento = item.NomeDoEvento;
                teste.CategoriaDoEvento = item.CategoriaDoEvento;
                teste.DescricaoDoEvento = item.DescricaoDoEvento;
                teste.DataDoEvento = item.DataDoEvento;
                teste.NomeDoAutor = item.NomeDoAutor;
                teste.EmailDoAutor = item.EmailDoAutor;
                teste.WhatsappDoAutor = item.WhatsappDoAutor;
                teste.Cidade = item.Cidade;
                teste.Quantidade = item.Quantidade;
                listaDeEventosInscritos.Add(teste);

                if (item.DataDoEvento < DateTime.Now)
                {
                    teste.Notificacao = "Evento já realizado!";
                }
            }

            lstvEventosInscritos.ItemsSource = listaDeEventosInscritos;
        }

        public static ImageSource ImageFromBase64(string base64picture)
        {
            byte[] imageBytes = Convert.FromBase64String(base64picture);
            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        private void LstvEventosPublicados_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            using (UserDialogs.Instance.Loading("Attendi..."))
            {
                //Device.BeginInvokeOnMainThread(() =>
                //{
                if (e.Item == null)
                    return;

                Evento_Inicio te = (Evento_Inicio)e.Item;

                Navigation.PushAsync(new DetalhesEvento(te.IdEvento, te.IdCriadorDoEvento, te.ImgPrincipalDoEventoConvert, te.Quantidade, true));

                //Deselect Item
                ((ListView)sender).SelectedItem = null;
                //});
            }
        }
    }
}
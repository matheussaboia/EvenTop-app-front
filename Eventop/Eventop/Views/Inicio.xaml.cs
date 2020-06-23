using Acr.UserDialogs;
using Eventop.Models;
using Eventop.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Eventop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inicio : TabbedPage
    {
        

        List<Evento_Inicio> eventosEmAlta = new List<Evento_Inicio>();
        List<Evento_Inicio> eventosPublicos = new List<Evento_Inicio>();
        List<Evento_Inicio> eventosPrivados = new List<Evento_Inicio>();

        public ObservableCollection<string> Items { get; set; }

        public Inicio()
        {
            //if (usuarioL == null) {
            //    Navigation.PushModalAsync(new LoginPage(), true);
            //}

            InitializeComponent();

            //ChamarDialogo();

            //Carrega eventos em alta
            CarregarEventosEmAlta();

            MyListView.RefreshCommand = new Command(() => { 
                CarregarEventosEmAlta();
                MyListView.IsRefreshing = false;
            });
            //________________________


            //Carrega eventos públicos
            CarregarEventosPublicos();

            lstvPublico.RefreshCommand = new Command(() => {
                CarregarEventosPublicos();
                lstvPublico.IsRefreshing = false;
            });
            //________________________


            //Carrega eventos PRIVADOS
            CarregarEventosPrivados();

            lstvPrivado.RefreshCommand = new Command(() => {
                CarregarEventosPrivados();
                lstvPrivado.IsRefreshing = false;
            });
            //________________________

            lblProcurarEventos.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ProcurarEventos()),
            });

            DesligarDialogo();
        }
        
        public static ImageSource ImageFromBase64(string base64picture)
        {
            byte[] imageBytes = Convert.FromBase64String(base64picture);
            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            using (UserDialogs.Instance.Loading("Attendi..."))
            {
                //Device.BeginInvokeOnMainThread(() =>
                //{
                if (e.Item == null)
                        return;

                    Evento_Inicio te = (Evento_Inicio)e.Item;


                    Navigation.PushAsync(new DetalhesEvento(te.IdEvento, te.IdCriadorDoEvento, te.ImgPrincipalDoEventoConvert, te.Quantidade));

                    //Deselect Item
                    ((ListView)sender).SelectedItem = null;
                //});
             }

        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
        }
        async void PerfilItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PerfilPage());
            //if (new Sessao().DeslogarDoAplicativo())
            //{
            //    await Navigation.PushModalAsync(new LoginPage());
            //}
        }

        protected async Task InserirDialogo()
        {
            await Task.Run(() => { UserDialogs.Instance.Loading("Quase lá..."); });
        }

        protected async Task RetirarDialogo()
        {
            await Task.Run(() => { UserDialogs.Instance.HideLoading(); });
        }

        protected async void ChamarDialogo()
        {
            await InserirDialogo();
        }

        protected async void DesligarDialogo()
        {
            await RetirarDialogo();
        }

        protected void CarregarEventosEmAlta() {

            eventosEmAlta.Clear();

            var respostaDoServidor = new JsonHelper().getParaOServidor("Eventos", "");
            List<Evento_Inicio> eventoBuscado = JsonConvert.DeserializeObject<List<Evento_Inicio>>(respostaDoServidor);

            foreach (var item in eventoBuscado){
                Evento_Inicio teste = new Evento_Inicio();
                teste.IdEvento = item.IdEvento;
                teste.IdCriadorDoEvento = item.IdCriadorDoEvento;
                teste.ImgPrincipalDoEventoConvert = ImageFromBase64(item.ImgPrincipalDoEvento);
                teste.IdEnderecoDoEvento = item.IdEnderecoDoEvento;
                teste.NomeDoEvento = item.NomeDoEvento;
                teste.CategoriaDoEvento = item.CategoriaDoEvento;
                teste.DescricaoDoEvento = item.DescricaoDoEvento;
                teste.DataDoEvento = item.DataDoEvento;
                teste.DataConvertida = item.DataDoEvento.ToString("dd/MM/yyyy HH:mm");
                teste.NomeDoAutor = item.NomeDoAutor;
                teste.EmailDoAutor = item.EmailDoAutor;
                teste.WhatsappDoAutor = item.WhatsappDoAutor;
                teste.Cidade = item.Cidade;
                teste.Quantidade = item.Quantidade;
                eventosEmAlta.Add(teste);
            }

            MyListView.ItemsSource = eventosEmAlta;
        }

        protected void CarregarEventosPublicos(){

            eventosPublicos.Clear();

            var respostaDoServidor = new JsonHelper().getParaOServidor("Eventos", "tipoEvento=false");
            List<Evento_Inicio> eventoBuscado = JsonConvert.DeserializeObject<List<Evento_Inicio>>(respostaDoServidor);
            foreach (var item in eventoBuscado){
                Evento_Inicio teste = new Evento_Inicio();
                teste.IdEvento = item.IdEvento;
                teste.IdCriadorDoEvento = item.IdCriadorDoEvento;
                teste.ImgPrincipalDoEventoConvert = ImageFromBase64(item.ImgPrincipalDoEvento);
                teste.IdEnderecoDoEvento = item.IdEnderecoDoEvento;
                teste.NomeDoEvento = item.NomeDoEvento;
                teste.CategoriaDoEvento = item.CategoriaDoEvento;
                teste.DescricaoDoEvento = item.DescricaoDoEvento;
                teste.DataDoEvento = item.DataDoEvento;
                teste.DataConvertida = item.DataDoEvento.ToString("dd/MM/yyyy HH:mm");
                teste.NomeDoAutor = item.NomeDoAutor;
                teste.EmailDoAutor = item.EmailDoAutor;
                teste.WhatsappDoAutor = item.WhatsappDoAutor;
                teste.Cidade = item.Cidade;
                teste.Quantidade = item.Quantidade;
                eventosPublicos.Add(teste);
            }
            lstvPublico.ItemsSource = eventosPublicos;
            
        }
        
        protected void CarregarEventosPrivados()
        {
            eventosPrivados.Clear();

            var respostaDoServidor = new JsonHelper().getParaOServidor("Eventos", "tipoEvento=true");
            List<Evento_Inicio> eventoBuscado = JsonConvert.DeserializeObject<List<Evento_Inicio>>(respostaDoServidor);

            foreach (var item in eventoBuscado){
                Evento_Inicio eventoPrivado = new Evento_Inicio();
                eventoPrivado.IdEvento = item.IdEvento;
                eventoPrivado.IdCriadorDoEvento = item.IdCriadorDoEvento;
                eventoPrivado.ImgPrincipalDoEventoConvert = ImageFromBase64(item.ImgPrincipalDoEvento);
                eventoPrivado.IdEnderecoDoEvento = item.IdEnderecoDoEvento;
                eventoPrivado.NomeDoEvento = item.NomeDoEvento;
                eventoPrivado.CategoriaDoEvento = item.CategoriaDoEvento;
                eventoPrivado.DescricaoDoEvento = item.DescricaoDoEvento;
                eventoPrivado.DataDoEvento = item.DataDoEvento; //Não exibe no ListView
                eventoPrivado.DataConvertida = item.DataDoEvento.ToString("dd/MM/yyyy HH:mm");
                eventoPrivado.UrlDoEvento = item.UrlDoEvento; //Não exibe no ListView e tem apenas para eventos privados
                eventoPrivado.NomeDoAutor = item.NomeDoAutor;
                eventoPrivado.EmailDoAutor = item.EmailDoAutor;
                eventoPrivado.WhatsappDoAutor = item.WhatsappDoAutor;
                eventoPrivado.Cidade = item.Cidade;
                eventoPrivado.Quantidade = item.Quantidade;
                eventosPrivados.Add(eventoPrivado);
            }

            //lstvPrivado.ItemsSource = eventosPrivados;
        }

        private void Inicio_CurrentPageChanged(object sender, EventArgs e)
        {
            Ini.BarBackgroundColor = ((ContentPage)Ini.CurrentPage).BackgroundColor;
        }

        private void SearchEventosPopulares_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyListView.ItemsSource = eventosEmAlta.Where(x => x.NomeDoEvento.ToLower().Contains(searchEventosPopulares.Text.ToLower()));
        }

        private void SearchEventosPublicos_TextChanged(object sender, TextChangedEventArgs e)
        {
            lstvPublico.ItemsSource = eventosPublicos.Where(x => x.NomeDoEvento.ToLower().Contains(searchEventosPublicos.Text.ToLower()));
        }

        private void SearchEventosPrivados_TextChanged(object sender, TextChangedEventArgs e)
        {
            lstvPrivado.ItemsSource = eventosPrivados.Where(x => x.UrlDoEvento.ToLower().Contains(searchEventosPrivados.Text.ToLower()));
        }

        protected void ProcurarEventos()
        {
            var listaDeEventos = eventosPublicos
                                     .Where(x => x.NomeDoEvento.ToLower().Contains(txtNomeDoEvento.Text.ToLower())
                                            && (txtCidadeDoEvento.Text == null || x.Cidade.ToLower().Contains(txtCidadeDoEvento.Text.ToLower()))
                                            && (dtPckDataDoEvento.Date.ToString("dd/MM/yyyy") == x.DataDoEvento.Date.ToString("dd/MM/yyyy"))
                                            //&& x.


                                            );
            lstvBuscar.ItemsSource = listaDeEventos;
        }

    }
}



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
using Xamarin.Essentials;
using Acr.UserDialogs;

namespace Eventop.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetalhesEvento : ContentPage
	{

        private Usuario usuarioL = new Sessao().CarregaLogin();

        private long idEvento;
        private DateTime dataDoEvento;
        private long idDoAutorDetalhe;
        private ImageSource imagemPrincipalDetalhe;
        private int quantidadeDetalhe;

        private long idDoUsuarioQueEstaParticipando;

        public DetalhesEvento () {

            if (usuarioL == null) {
                Navigation.PushModalAsync(new LoginPage(), true);
            }

            InitializeComponent ();
            
        }

        public DetalhesEvento(long idDoEvento, long idDoAutor, ImageSource imagemPrincipal, int quantidade)
        {
            if (usuarioL == null) {
                Navigation.PushModalAsync(new LoginPage(), true);
            }
            
            InitializeComponent();

            // (idDoAutor != usuarioL.Id) {
            //    toolAtualizar.
            //}
            //else {
            //
            //}

            lblAtualizar.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => CarregarComentariosDoEvento(idDoEvento)),
            });

            lblUrlEvento.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => CompartilharURL()),
            });

            //Armazena os valores dos parametros em variáveis da classe
            idDoAutorDetalhe = idDoAutor;
            imagemPrincipalDetalhe = imagemPrincipal;
            quantidadeDetalhe = quantidade;

            //Rotinas
            CarregarDadosDoEvento(idDoEvento, imagemPrincipal, quantidade);
            if (usuarioL.TipoUsuario == "Uma pessoa")
            {
                VerificarSeParticipaDoEvento(idDoEvento);
            }
            CarregarComentariosDoEvento(idDoEvento);

            //Map.OpenAsync(-22.907, -43.1819, new MapLaunchOptions
            //{
            //    Name = "Teste",
            //    NavigationMode = NavigationMode.Driving
            //});
        }

        public DetalhesEvento(long idDoEvento, long idDoAutor, ImageSource imagemPrincipal, int quantidade, bool meuEvento)
        {
            if (usuarioL == null)
            {
                Navigation.PushModalAsync(new LoginPage(), true);
            }

            InitializeComponent();

            // (idDoAutor != usuarioL.Id) {
            //    toolAtualizar.
            //}
            //else {
            //
            //}

            lblAtualizar.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => CarregarComentariosDoEvento(idDoEvento)),
            });

            lblUrlEvento.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => CompartilharURL()),
            });

            lblRelatorioUsuarios.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => CarregarUsuariosParticipantes(idDoEvento)),
            });

            //Armazena os valores dos parametros em variáveis da classe
            idDoAutorDetalhe = idDoAutor;
            imagemPrincipalDetalhe = imagemPrincipal;
            quantidadeDetalhe = quantidade;

            //Rotinas
            CarregarDadosDoEvento(idDoEvento, imagemPrincipal, quantidade);
            if (usuarioL.TipoUsuario == "Uma pessoa")
            {
                VerificarSeParticipaDoEvento(idDoEvento);
            }
            CarregarComentariosDoEvento(idDoEvento);

            //Map.OpenAsync(-22.907, -43.1819, new MapLaunchOptions
            //{
            //    Name = "Teste",
            //    NavigationMode = NavigationMode.Driving
            //});
        }

        protected void CarregarDadosDoEvento(long idDoEvento, ImageSource imagemPrincipal, int quantidade) {

            //Faz a busca no servidor e converte para o tipo real
            var respostaDoServidor = new JsonHelper().getParaOServidor("Eventos", "id=" + idDoEvento);
            List<Evento_Inicio> detalhesDoEvento = JsonConvert.DeserializeObject<List<Evento_Inicio>>(respostaDoServidor);
            
            //Armazena em variável local
            idEvento = idDoEvento;
            dataDoEvento = detalhesDoEvento[0].DataDoEvento;

            //Dados do evento
            NomeDoEvento.Title = detalhesDoEvento[0].NomeDoEvento;

            imgPrincipalDoEvento.Source = imagemPrincipal;
            imgPrincipalDoEvento.WidthRequest = 400;
            imgPrincipalDoEvento.HeightRequest = 250;

            lblNomeDoEvento.Text = detalhesDoEvento[0].NomeDoEvento;
            lblLocalDoEvento.Text = detalhesDoEvento[0].Cidade;
            lblDataDoEvento.Text = detalhesDoEvento[0].DataDoEvento.ToLongDateString();

            lblDescricaoDoEvento.Text = detalhesDoEvento[0].DescricaoDoEvento;
            lblParticipantesDoEvento.Text = detalhesDoEvento[0].Quantidade.ToString() + " já estão participando!";

            lblNomeDoProdutor.Text = "@" + detalhesDoEvento[0].NomeDoAutor;
            lblEmailDoProdutor.Text = detalhesDoEvento[0].EmailDoAutor;
            lblTelefoneDoProdutor.Text = detalhesDoEvento[0].WhatsappDoAutor;

            //Verifica se evento já passou da data
            //Se não, realiza rotinas e impede participação
            if (DateTime.Now >= detalhesDoEvento[0].DataDoEvento){

                lblAtualizar.Text = "Esse evento já foi realizado.";
                lblInfoAtualizar.IsVisible = false;
                btnParticiparDoEvento.IsVisible = false;
            }
            else{

                //Verifica se o usuário na sessão é uma empresa e retira os botões da tela
                if (usuarioL.TipoUsuario == "Uma empresa") {
                    btnParticiparDoEvento.IsVisible = false;
                    btnAvaliarEvento.IsVisible = false;
                    btnDesinscreverDoEvento.IsVisible = false;
                }

                //Verifica se o usuário é o dono do evento
                if (detalhesDoEvento[0].IdCriadorDoEvento == usuarioL.Id){
                    lblInfoAtualizar.IsVisible = true;
                    lblAtualizar.IsVisible = true;
                    frameRelatorio.IsVisible = true;
                }
                else
                {
                    lblInfoAtualizar.IsVisible = false;
                    lblAtualizar.IsVisible = false;
                }
            }

            //Rotinas de tipo de evento
            switch (detalhesDoEvento[0].TipoDoEvento){
                case true:
                    lblInformacaoTipoEvento.Text = "Evento particular, acessível somente por esse link:";
                    lblInformacaoTipoEvento.TextColor = Color.Blue;
                    lblUrlEvento.Text = detalhesDoEvento[0].UrlDoEvento;
                    lblUrlEvento.IsVisible = true;
                    break;
                case false:
                    lblInformacaoTipoEvento.Text = "Evento sob produção de empresa!";
                    lblInformacaoTipoEvento.TextColor = Color.BlueViolet;

                    break;
            }
            
        }

        protected void CarregarComentariosDoEvento(long idDoEvento)
        {
            var respostaDoServidor = new JsonHelper().getParaOServidor("Participante_Evento", "id=" + idDoEvento + "&temComentario=true");

            List<Participante_Evento> listaDeComentarios = JsonConvert.DeserializeObject<List<Participante_Evento>>(respostaDoServidor);

            //Verifica se há comentários para o evento
            if (listaDeComentarios.Count > 0){
                lstvComentarios.ItemsSource = listaDeComentarios;
            }
            else{
                lblComentarios.Text = "Não há comentários para esse evento.";
            }
            
            //Verifica se o usuário já tem avaliação no evento
            var usuarioPart = listaDeComentarios.Where(x => x.IdUsuarioParticipante == usuarioL.Id && x.DescricaoDaAvaliacao != null);
            if(usuarioPart.Count() > 0)
            {
                btnAvaliarEvento.IsVisible = false;
                lblAvaliar.Text = "Você já avaliou esse evento";
                lblAvaliar.IsVisible = true;
            }
        }

        protected void VerificarSeParticipaDoEvento(long idDoEvento) {

            var respostaDoServidor = new JsonHelper().getParaOServidor("Participante_Evento", "idEvento=" + idDoEvento + "&idParticipante=" + usuarioL.Id);

            List<Participante_Evento> listaDeComentarios = JsonConvert.DeserializeObject<List<Participante_Evento>>(respostaDoServidor);

            if (respostaDoServidor == "[]") {
                btnParticiparDoEvento.IsVisible = true;
                btnDesinscreverDoEvento.IsVisible = false;
            }
            else {

                btnParticiparDoEvento.IsVisible = false;

                //Verifica se a data do evento já passou
                if (DateTime.Now >= dataDoEvento) {

                    btnDesinscreverDoEvento.IsVisible = false;

                    //Verifica se o usuário já avaliou o evento
                    if (listaDeComentarios[0].DescricaoDaAvaliacao == null) {
                        btnAvaliarEvento.IsVisible = true;
                    }
                    else {
                        btnAvaliarEvento.IsVisible = false;
                    }
                }
                else {
                    btnDesinscreverDoEvento.IsVisible = true;
                }
                //lstvComentarios.ItemsSource = listaDeComentarios;
                
                idDoUsuarioQueEstaParticipando = listaDeComentarios[0].IdParticipante;
            }

        }

        protected async void CarregarUsuariosParticipantes(long idDoEvento)
        {
            await Navigation.PushAsync(new RelatorioUsuariosParticipantes(idDoEvento));
        }

        private void AtualizarEvento_Clicked(object sender, EventArgs e)
        {

        }

        private async void Participar_Clicked(object sender, EventArgs e){

            Participante_Evento participante = new Participante_Evento();
            participante.IdUsuarioParticipante = usuarioL.Id;
            participante.IdDoEvento = idEvento;
            participante.DataDaParticipacao = DateTime.Now;
            
            var jsonParticipanteEvento = JsonConvert.SerializeObject(participante);

            var respostaDoServidor = new JsonHelper().enviarPostParaServidorJson(jsonParticipanteEvento, "Participante_Evento");

            await DisplayAlert("Muito bom!!", "Você se inscreveu nesse evento e receberá notificações quando estiver perto.", "Ok!");

            VerificarSeParticipaDoEvento(idEvento);
            CarregarDadosDoEvento(idEvento, imagemPrincipalDetalhe, quantidadeDetalhe);
        }


        
        async void CompartilharURL()
        {
            await CompartilharUri(lblUrlEvento.Text);
        }
        public async Task CompartilharUri(string uri)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = uri,
                Title = "Compartilhando link"
            });
        }

        private void BtnAvaliarEvento_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AvaliacaoEvento(idEvento, usuarioL.Id, lblNomeDoEvento.Text), true);
        }

        private async void BtnDesinscreverDoEvento_Clicked(object sender, EventArgs e)
        {
            var acao = await DisplayAlert("Hm.. que triste!", "Deseja realmente se desinscrever do evento?.", "Desejo", "Cancelar");

            switch (acao) {
                case true:
                    btnParticiparDoEvento.IsVisible = true;
                    btnDesinscreverDoEvento.IsVisible = false;

                    var respostaDoServidor = new JsonHelper().DeleteParaOServidor("Participante_Evento?id=" + idDoUsuarioQueEstaParticipando);

                    CarregarDadosDoEvento(idEvento, imagemPrincipalDetalhe, quantidadeDetalhe);
                    break;

                case false:

                    btnParticiparDoEvento.IsVisible = false;

                    btnDesinscreverDoEvento.IsVisible = true;
                    break;
            }
        }

        protected async Task RetirarDialogo()
        {
            await Task.Run(() => { UserDialogs.Instance.HideLoading(); });
        }
        protected async void DesligarDialogo()
        {
            await RetirarDialogo();
        }
        protected async Task InserirDialogo()
        {
            await Task.Run(() => { UserDialogs.Instance.ShowLoading("Quase lá..."); });
        }
        protected async void ChamarDialogo()
        {
            await InserirDialogo();
        }


    }
}
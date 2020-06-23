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
	public partial class CadastroEventoPreVisualizacao : ContentPage
	{

        private Usuario usuarioL = new Sessao().CarregaLogin();

        //Evento
        private long idDoCriadorEvento;
        private byte[] imagemPrincipalDoEvento;
        private string nomeDoEvento;
        private string categoriaDoEvento;
        private string descricaoDoEvento;
        private DateTime dataDoEvento;
        private string urlDoEvento;

        //Endereço
        private string cepDoEvento;
        private string cidadeDoEvento;
        private string bairroDoEvento;
        private string enderecoDoEvento;
        private string complementoDoEvento;
        private string numeroDoEvento;

        public CadastroEventoPreVisualizacao ()
		{
            if (usuarioL == null) {
                Navigation.PushModalAsync(new LoginPage(), true);
            }

            InitializeComponent ();
		}

        public CadastroEventoPreVisualizacao(long idDoCriadorPreview, byte[] imagemPrincipalDoEventoPreview, string nomeDoEventoPreview, string categoriaDoEventoPreview, string descricaoDoEventoPreview, DateTime dataDoEventoPreview, string cepDoEventoPreview, string cidadeDoEventoPreview,string bairroDoEventoPreview, string enderecoDoEventoPreview, string complementoDoEventoPreview, string numeroDoEventoPreview)
        {
            InitializeComponent();

            FrameTitulo.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => AbrirInformacoesDeLocal()),
            });

            switch (usuarioL.TipoUsuario){
                case "Uma pessoa": //Pessoa física = evento privado
                    lblInformacaoTipoEvento.Text = "Evento particular, acessível somente este link: ";
                    break;
                case "Uma empresa": //Pessoa jurídica = evento público
                    lblInformacaoTipoEvento.Text = "Esse evento é de empresa e público!";
                    break;
            }

            //Inicializando variável para cadastro de Evento
            idDoCriadorEvento = idDoCriadorPreview;
            imagemPrincipalDoEvento = imagemPrincipalDoEventoPreview;
            nomeDoEvento = nomeDoEventoPreview;
            categoriaDoEvento = categoriaDoEventoPreview;
            descricaoDoEvento = descricaoDoEventoPreview;
            dataDoEvento = dataDoEventoPreview;
            urlDoEvento = "https://www.eventop.com/br/pt-br/evento/" + idDoCriadorEvento + "" + dataDoEvento.ToString("ddmmyyyy");

            //Inicializando variável para cadastro de Endereço
            cepDoEvento = cepDoEventoPreview;
            cidadeDoEvento = cidadeDoEventoPreview;
            bairroDoEvento = bairroDoEventoPreview;
            enderecoDoEvento = enderecoDoEventoPreview;
            complementoDoEvento = complementoDoEventoPreview;
            numeroDoEvento = numeroDoEventoPreview;

            //Título e local
            imgPrincipalDoEvento.Source = ImageSource.FromStream(() => new MemoryStream(imagemPrincipalDoEvento));
            imgPrincipalDoEvento.WidthRequest = 600;
            imgPrincipalDoEvento.HeightRequest = 250;

            lblNomeDoEvento.Text = nomeDoEvento;
            lblLocalDoEvento.Text = cidadeDoEvento + " - " + bairroDoEvento;
            lblDataDoEvento.Text = "Vai ocorrer em " + dataDoEvento.ToString("dd/MM/yyyy");

            //Descrição
            lblDescricaoDoEvento.Text = descricaoDoEvento;
            lblCategoriaDoEvento.Text = categoriaDoEvento;

            //Contatos
            lblEmailDoAutor.Text = usuarioL.Email;
            lblWhatsappDoAutor.Text = usuarioL.Whatsapp;



        }

        protected void AbrirInformacoesDeLocal()
        {
            DisplayAlert("Os detalhes do local","CEP: " + cepDoEvento, "Ok");
        }

        private async void Publicar_Clicked(object sender, EventArgs e)
        {
            //Cadastro do Endereço
            Endereco enderecoCadastrado = new Endereco();
            enderecoCadastrado.CepEndereco = cepDoEvento;
            enderecoCadastrado.CidadeEndereco = cidadeDoEvento;
            enderecoCadastrado.BairroEndereco = bairroDoEvento;
            enderecoCadastrado.NumeroEndereco = numeroDoEvento;
            enderecoCadastrado.ComplementoEndereco = complementoDoEvento.Replace("'", "''");
            enderecoCadastrado.LogradouroEndereco = enderecoDoEvento;

            var jsonEndereco = JsonConvert.SerializeObject(enderecoCadastrado);

            var cadastroEndereco = new JsonHelper().enviarPostParaServidorJson(jsonEndereco, "Enderecoes");

            Endereco enderecoConvertido = JsonConvert.DeserializeObject<Endereco>(cadastroEndereco);

            //Endereco enderecoConvertido = (Endereco)cadastroEndereco;

            //Cadastro do Evento
            Evento evento = new Evento();
            evento.IdCriadorDoEvento = idDoCriadorEvento;
            evento.NomeDoEvento = nomeDoEvento;
            evento.CategoriaDoEvento = categoriaDoEvento;
            evento.DescricaoDoEvento = descricaoDoEvento;
            switch(usuarioL.TipoUsuario) {
                case "Uma pessoa": //Pessoa física = evento privado
                    evento.TipoDoEvento = true;
                    break;
                case "Uma empresa": //Pessoa jurídica = evento público
                    evento.TipoDoEvento = false;
                    break;
            }
            evento.DataDoEvento = dataDoEvento;
            evento.UrlDoEvento = urlDoEvento;
            evento.IdEnderecoDoEvento = enderecoConvertido.IdEndereco; //id do endereço cadastrado acima

            var jsonEvento = JsonConvert.SerializeObject(evento);

            var cadastroEvento = new JsonHelper().enviarPostParaServidorJson(jsonEvento, "Eventos");

            Evento eventoCadastrado = JsonConvert.DeserializeObject<Evento>(cadastroEvento);

            //Cadastro da imagem principal
            Galeria imagemPrincipal = new Galeria();
            //imagemPrincipal.IdEventoGaleria = eventoCadastrado.IdEvento;
            imagemPrincipal.imagemGaleria = imagemPrincipalDoEvento;
            imagemPrincipal.imagemPrincipal = true;

            var jsonGaleria = JsonConvert.SerializeObject(imagemPrincipal);

            var cadastroImagem = new JsonHelper().enviarPostParaServidorJson(jsonGaleria, "Galerias");

            Galeria imagemCadastrada = JsonConvert.DeserializeObject<Galeria>(cadastroImagem);

            //Alocação da imagem em GALERIA - EVENTO
            Galeria_Evento galEven = new Galeria_Evento();
            galEven.IdEvento = eventoCadastrado.IdEvento;
            galEven.IdGaleria = imagemCadastrada.idGaleria;

            var jsonGaleriaEvento = JsonConvert.SerializeObject(galEven);

            var cadastroImagemEvento = new JsonHelper().enviarPostParaServidorJson(jsonGaleriaEvento, "Galeria_Evento");

            //Galeria_Evento imagemEventoCadastrada = JsonConvert.DeserializeObject<Galeria_Evento>(jsonGaleriaEvento);

            //await Navigation.PushModalAsync(new InicioDetalhes(usuarioL.Id, usuarioL.Login), true);
            await DisplayAlert("É isso aí!", "Você publicou " + evento.NomeDoEvento + " e ele já pode ser visto.", "Partiu!");
            Application.Current.MainPage = new NavigationPage(new InicioDetalhes(usuarioL.Id, usuarioL.Login));


            /*if (respostaDoServidor != null)
            {
                await DisplayAlert(, "Bem-vindo ao EvenTop!", "Partiu!");
                await Navigation.PushModalAsync(new MainPage(), true);
            }
            else
            {
                await DisplayAlert("Hm.. tivemos um problema", "Não conseguimos encontrar sua conta!", "Tente novamente!");
            }*/
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Eventop.Models;
using Eventop.Views;
using System.Diagnostics;
using Eventop.Util;
using Acr.UserDialogs;

namespace Eventop
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
    {
        private Usuario usuarioL = new Sessao().CarregaLogin();

        private static Entry usuarioDeLogin;
        private static Entry senhaDeLogin;
        private static Label cadastrarUsuario;

        public LoginPage ()
		{
            if (usuarioL != null) {
                Navigation.PushModalAsync(new NavigationPage(new InicioDetalhes(usuarioL.Id, usuarioL.Login)));
            }

            InitializeComponent();
            usuarioDeLogin = txtUsuario;
            senhaDeLogin = txtSenha;
            cadastrarUsuario = lblCadastrarUsuario;
            cadastrarUsuario.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => irParaTelaDeCadastro()),
            });
        }

        protected async Task InserirDialogo()
        {
            await Task.Run(() => { UserDialogs.Instance.ShowLoading("Quase lá..."); });
        }

        protected async Task RetirarDialogo()
        {
            await Task.Run(() => { UserDialogs.Instance.HideLoading(); });
        }

        private async void Button_Clicked(object sender, EventArgs e){

            await InserirDialogo();
            //await Task.Delay(4000);
            if (usuarioDeLogin.Text != null && senhaDeLogin.Text != null)
                {

                    //Atenção para espaços, o segundo parametro é passagem pela URL
                    var respostaDoServer = new JsonHelper().getParaOServidor("Usuarios", "usuario=" + usuarioDeLogin.Text + "&senha=" + senhaDeLogin.Text);

                    List<Usuario> listaDeUsuario = JsonConvert.DeserializeObject<List<Usuario>>(respostaDoServer);

                    if (listaDeUsuario.Count > 0)
                    {

                        //await RetirarDialogo();
                        Usuario usuario = listaDeUsuario.First();

                        Application.Current.Properties["idUsuario"] = usuario.Id;
                        //await DisplayAlert(usuario.Login, "Bem-vindo ao EvenTop!", "Partiu!");
                        //CrossBadge.Current.SetBadge(1);
                        Application.Current.MainPage = new NavigationPage(new InicioDetalhes(usuario.Id, usuario.Login));
                        //await Navigation.PushAsync(new InicioDetalhes(usuario.Id, usuario.Login));
                    }
                    else
                    {
                        await RetirarDialogo();
                        actInd.IsVisible = false;
                        actInd.IsRunning = false;
                        await DisplayAlert("Hm.. tivemos um problema", "Não conseguimos encontrar sua conta!", "Tente novamente!");
                    }



                /*HttpClient client = new HttpClient{
                    BaseAddress = new Uri("http://192.168.1.208:3000/api/Usuarios")
                };
                try
                {
                    var request = client.GetAsync("/api/Usuarios?usuario=" + usuarioDeLogin.Text + "&senha=" + senhaDeLogin.Text + "").Result;
                    if (request.IsSuccessStatusCode)
                    {
                        var responseJson = request.Content.ReadAsStringAsync().Result;
                        var resultado = JsonConvert.DeserializeObject<List<Usuario>>(responseJson);
                        if (resultado != null)
                        {
                        }
                    }
                    //await Navigation.PushModalAsync(new MainPage());

                    //Some risky client call that will call parallell code / async /TPL or in some way cause an AggregateException 

                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        Debug.WriteLine(errInner); //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                    }
                }*/
            }
        }

        private async void irParaTelaDeCadastro()
        {
            await Navigation.PushAsync(new CadastroPage());
        }
    }
}
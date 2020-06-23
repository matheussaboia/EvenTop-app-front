using Eventop.Models;
using Eventop.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Eventop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroPage : ContentPage
    {
        private static Picker tipoDeUsuario;
        private static string tipoDeUsuarioSelecionado;
        private static Entry cpfDoUsuario;
        private static Entry cnpjDoUsuario;
        private static Entry usuarioDeLogin;
        private static Entry senhaDeLogin;
        private static Entry emailDoUsuario;


        public CadastroPage()
        {
            InitializeComponent();

            //Torna os elementos do XAML utilizáveis
            tipoDeUsuario = pckTipoUsuario;
            usuarioDeLogin = txtUsuario;
            cpfDoUsuario = txtCPF;
            cnpjDoUsuario = txtCNPJ;
            senhaDeLogin = txtSenha;
            emailDoUsuario = txtEmail;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //Armazena na variável estática o documento do tipo da pessoa
            switch (tipoDeUsuario.SelectedIndex) {
                case 0:
                    tipoDeUsuarioSelecionado = cpfDoUsuario.Text;
                    break;
                case 1:
                    tipoDeUsuarioSelecionado = cnpjDoUsuario.Text;
                    break;
            }

            //Recebe e armazena as informações que precisam ser digitadas
            var propriedadesObrigatorias = new {
                Tipo = tipoDeUsuario.SelectedItem,
                Documento = tipoDeUsuarioSelecionado,
                Usuario = usuarioDeLogin.Text,
                Senha = senhaDeLogin.Text,
                Email = emailDoUsuario.Text
            };
            
            //Armazena em uma variável genérica os campos que não foram preenchidos
            var verificarCamposNaoPreenchidos = new ValidacaoCampos().ValidarCampos(propriedadesObrigatorias);

            //Verifica se os termos de uso estão aceitos
            switch (swtTermosDeUso.IsToggled) {
                case false:
                    await DisplayAlert("Ops ;/", "É necessário aceitar os termos de uso para prosseguir", "Ok");
                    return;
            }

            //Verifica se há campos que não foram preenchidos
            if (verificarCamposNaoPreenchidos.Count == 0) {

                //Armazena na classe 'Usuario' as informações digitadas
                Usuario usuario = new Usuario();
                usuario.TipoUsuario = propriedadesObrigatorias.Tipo.ToString();
                usuario.Documento = propriedadesObrigatorias.Documento;
                usuario.Login = propriedadesObrigatorias.Usuario.ToLower();
                usuario.Senha = propriedadesObrigatorias.Senha;
                usuario.Email = propriedadesObrigatorias.Email;
                usuario.DataDaCriacao = DateTime.Now;
                usuario.IdEndereco = null;

                //Armazena a conversão do objeto usuário para JSON
                var json = JsonConvert.SerializeObject(usuario);
                //string result = "";

                var resposta = new JsonHelper().enviarPostParaServidorJson(json, "Usuarios");


                if (resposta != "[]")
                {
                    Usuario usuarioCadastrado = JsonConvert.DeserializeObject<Usuario>(resposta);

                    await DisplayAlert("@"+usuario.Login, "Bem-vindo ao EvenTop!", "Partiu");

                    Application.Current.Properties["idUsuario"] = usuarioCadastrado.Id;
                    await Navigation.PushAsync(new InicioDetalhes(usuarioCadastrado.Id, usuarioCadastrado.Login));
                    return;
                }

                /*Envia as informações como POST para o servidor(WebAPI) fazer o registro no BANCO DE DADOS
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.208:3000/api/Usuarios");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())){
                    Debug.Write(json);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                //Tenta ler as informações e verifica, no servidor, se elas foram enviadas com sucesso
                try{
                    using (var response = httpWebRequest.GetResponse() as HttpWebResponse){
                        if (httpWebRequest.HaveResponse && response != null){
                            using (var reader = new StreamReader(response.GetResponseStream())) {
                                result = reader.ReadToEnd();
                                Usuario usuarioCadastrado = JsonConvert.DeserializeObject<Usuario>(result);
                                if (result != null) {
                                    await DisplayAlert("Bem-vindo, " + usuarioCadastrado.Login + "", "Participe, marque e crie eventos. Chegou a hora de desfrutar!", "Vamos lá");
                                    await Navigation.PushModalAsync(new MainPage(), true);
                                }
                            }
                        }
                    }
                }
                catch (WebException h) {
                    if (h.Response != null) {
                        using (var errorResponse = (HttpWebResponse)h.Response) {
                            using (var reader = new StreamReader(errorResponse.GetResponseStream())) {
                                string error = reader.ReadToEnd();
                                result = error;
                            }
                        }
                    }
                }
                return; */
            }
            string mensagensDeErros = string.Empty;
            foreach (string erros in verificarCamposNaoPreenchidos){
                mensagensDeErros += erros + "\n";
            }
            await DisplayAlert("Hm.. ainda não", mensagensDeErros, "Vou preencher");
        }

        private void PckTipoUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tipoDeUsuario.SelectedIndex)
            {
                case 0:
                    tipoDeUsuarioSelecionado = cpfDoUsuario.Text;
                    cpfDoUsuario.IsVisible = true;
                    cnpjDoUsuario.IsVisible = false;
                    break;
                case 1:
                    tipoDeUsuarioSelecionado = cnpjDoUsuario.Text;
                    cnpjDoUsuario.IsVisible = true;
                    cpfDoUsuario.IsVisible = false;
                    break;

            }
        }
    }
}
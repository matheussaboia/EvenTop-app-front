using Eventop.Models;
using Eventop.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Eventop.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CadastroEventoSegundaEtapa : ContentPage
	{
        private static long criadorDoEventoSegunda;
        private static byte[] imagemPrincipalDoEventoSegunda;
        private static string nomeDoEventoSegunda;
        private static string categoriaDoEventoSegunda;
        private static string descricaoDoEventoSegunda;
        private static DateTime dataDoEventoSegunda;

        public CadastroEventoSegundaEtapa(long criadorDoEventoCad, byte[] imagemPrincipalDoEvento, string nomeDoEventoCad, string categoriaDoEventoCad, string descricaoDoEventoCad, DateTime dataDoEventoCad)
        {
            InitializeComponent();

            criadorDoEventoSegunda = criadorDoEventoCad;
            imagemPrincipalDoEventoSegunda = imagemPrincipalDoEvento;
            nomeDoEventoSegunda = nomeDoEventoCad;
            categoriaDoEventoSegunda = categoriaDoEventoCad;
            descricaoDoEventoSegunda = descricaoDoEventoCad;
            dataDoEventoSegunda = dataDoEventoCad;
        }

        public CadastroEventoSegundaEtapa ()
        {
            InitializeComponent ();
        }

        private async void TxtCEP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCEP.Text.Length == 8)
            {
                //MostrarIndicarDeAtividade();
                    //PegarDadosDoCEP();
                
                await Task.Run(() => { PegarDadosDoCEP(); });
            }
            else {
                EsconderFormulario();
            }
        }

        protected void PegarDadosDoCEP()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var respostaDoServidor = new JsonHelper().PegarDadosEnderecoAPI("Correios", "cep=" + txtCEP.Text);

                //RetirarIndicadorDeAtividade();
                if (respostaDoServidor != null)
                {
                    lblErroCEP.IsVisible = false;
                    lblCidade.IsVisible = true;
                    lblBairro.IsVisible = true;
                    lblEndereco.IsVisible = true;
                    txtCidade.IsVisible = true;
                    txtBairro.IsVisible = true;
                    txtEndereco.IsVisible = true;
                    lblInformacoes.IsVisible = true;
                    lblComplemento.IsVisible = true;
                    txtComplemento.IsVisible = true;
                    lblNumero.IsVisible = true;
                    txtNumero.IsVisible = true;
                    btnVerPreview.IsVisible = true;

                    txtCidade.Text = respostaDoServidor[0].ToString();
                    txtBairro.Text = respostaDoServidor[1].ToString();
                    txtEndereco.Text = respostaDoServidor[2].ToString();
                }
                else
                {
                    lblErroCEP.IsVisible = true;
                    EsconderFormulario();
                    LimparFormulario();
                }
            });
        }

        protected void MostrarIndicarDeAtividade()
        {
            actInd.IsVisible = true;
            actInd.IsRunning = true;
        }

        protected void RetirarIndicadorDeAtividade()
        {
            actInd.IsVisible = false;
            actInd.IsRunning = false;
        }

        protected void EsconderFormulario() {
            lblCidade.IsVisible = false;
            lblBairro.IsVisible = false;
            lblEndereco.IsVisible = false;
            txtCidade.IsVisible = false;
            txtBairro.IsVisible = false;
            txtEndereco.IsVisible = false;
        }

        protected void LimparFormulario()
        {
            txtCidade.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtEndereco.Text = string.Empty;
        }

        private async void BtnVerPreview_Clicked(object sender, EventArgs e)
        {
            var propriedadesObrigatorias = new
            {
                CepPreview = txtCEP.Text,
                CidadePreview = txtCidade.Text,
                BairroPreview = txtBairro.Text,
                EnderecoPreview = txtEndereco.Text,
                ComplementoPreview = txtComplemento.Text,
                NumeroPreview = txtNumero.Text
            };

            //Armazena em uma variável genérica os campos que não foram preenchidos
            var verificarCamposNaoPreenchidos = new ValidacaoCampos().ValidarCampos(propriedadesObrigatorias);

            if(verificarCamposNaoPreenchidos.Count == 0)
            {
                await DisplayAlert("Quase tudo pronto", "Agora é hora de ver a pré-visualização do seu evento!", "Vamos lá");
                await Navigation.PushAsync(new CadastroEventoPreVisualizacao(criadorDoEventoSegunda, imagemPrincipalDoEventoSegunda, nomeDoEventoSegunda, 
                    categoriaDoEventoSegunda, descricaoDoEventoSegunda, dataDoEventoSegunda, propriedadesObrigatorias.CepPreview, propriedadesObrigatorias.CidadePreview,
                    propriedadesObrigatorias.BairroPreview, propriedadesObrigatorias.EnderecoPreview, propriedadesObrigatorias.ComplementoPreview,
                    propriedadesObrigatorias.NumeroPreview));
            }
            else
            {

            }
        }
    }
}
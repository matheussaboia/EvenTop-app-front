using Eventop.Models;
using Eventop.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Eventop.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AvaliacaoEvento : ContentPage
	{
        List<Participante_Evento> participante = new List<Participante_Evento>();
        

        public AvaliacaoEvento(long idEvento, long idUsuario, string nomeDoEvento)
		{
			InitializeComponent ();

            var respostaDoServidor = new JsonHelper().getParaOServidor("Participante_Evento", "idEvento=" + idEvento + "&idParticipante=" + idUsuario);
            participante = JsonConvert.DeserializeObject<List<Participante_Evento>>(respostaDoServidor);

            Avaliacao.Title = nomeDoEvento;             
        }

        private void BtnAvaliar_Clicked(object sender, EventArgs e)
        {
            participante.First().DescricaoDaAvaliacao = txtComentarioParticipante.Text;
            participante.First().DataDaAvaliacao = DateTime.Now;
            
            var jsonParticipanteAvaliacao = JsonConvert.SerializeObject(participante.First());

            var respostaDoServidor = new JsonHelper().PutParaOServidor(jsonParticipanteAvaliacao, "Participante_Evento?idDoEvento=" + participante.First().IdParticipante +"&participante="+ jsonParticipanteAvaliacao);
        }
    }
}
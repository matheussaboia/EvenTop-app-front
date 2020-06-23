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
	public partial class RelatorioUsuariosParticipantes : ContentPage
	{
		public RelatorioUsuariosParticipantes (long idDoEvento)
		{
			InitializeComponent ();

            var respostaDoServidor = new JsonHelper().getParaOServidor("Participante_Evento", "id=" + idDoEvento);

            List<Participante_Evento> listaDeComentarios = JsonConvert.DeserializeObject<List<Participante_Evento>>(respostaDoServidor);

            lstvUsuariosParticipantes.ItemsSource = listaDeComentarios;
        }


        private void LstvUsuariosParticipantes_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}
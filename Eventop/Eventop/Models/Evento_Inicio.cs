using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Eventop.Models
{
    class Evento_Inicio : Evento
    {
        [JsonProperty("USUA_LOGIN_USUARIO")]
        public string NomeDoAutor { get; set; }
        [JsonProperty("USUA_EMAIL_USUARIO")]
        public string EmailDoAutor { get; set; }
        [JsonProperty("USUA_WHATSAPP_USUARIO")]
        public string WhatsappDoAutor { get; set; }
        [JsonProperty("END_CIDADE_ENDERECO")]
        public string Cidade { get; set; }

        //Model para recebe o valor em JSON e converter em ImageSource
        [JsonProperty("GAL_BYTE_IMAGEM_GALERIA")]
        public string ImgPrincipalDoEvento { get; set; }
        public ImageSource ImgPrincipalDoEventoConvert { get; set; }


        [JsonProperty("Quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("Notificacao")]
        public string Notificacao { get; set; }

        [JsonProperty("DataConvertida")]
        public string DataConvertida { get; set; }
    }
}

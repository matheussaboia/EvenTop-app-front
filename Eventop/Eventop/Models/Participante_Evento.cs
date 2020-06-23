using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventop.Models
{
    class Participante_Evento
    {
        [JsonProperty("PAR_ID_PARTICIPANTE")]
        public long IdParticipante { get; set; }
        [JsonProperty("USUA_ID_USUARIO")]
        public long IdUsuarioParticipante { get; set; }
        [JsonProperty("EVEN_ID_EVENTO")]
        public long IdDoEvento { get; set; }
        [JsonProperty("PAR_DATA_PARTICIPANTE")]
        public DateTime DataDaParticipacao { get; set; }
        [JsonProperty("PAR_QTD_AVALIACAO")]
        public float QuantidadeAvaliacao { get; set; }
        [JsonProperty("PAR_DESCRICAO_AVALIACAO")]
        public string DescricaoDaAvaliacao { get; set; }
        [JsonProperty("PAR_DATA_AVALIACAO")]
        public DateTime? DataDaAvaliacao { get; set; }

        //Nome do usuário
        [JsonProperty("USUA_LOGIN_USUARIO")]
        public string LoginDoUsuario { get; set; }

        //Nome do usuário
        [JsonProperty("USUA_EMAIL_USUARIO")]
        public string EmailDoUsuario { get; set; }

        //Nome do usuário
        [JsonProperty("USUA_WHATSAPP_USUARIO")]
        public string WhatsappDoUsuario { get; set; }

        //Nome do usuário
        [JsonProperty("USUA_SEXO_USUARIO")]
        public string SexoDoUsuario { get; set; }
    }
}

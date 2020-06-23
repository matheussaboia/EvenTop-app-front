using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Eventop.Models
{
    class Evento
    {
        [JsonProperty("EVEN_ID_EVENTO")]
        public int IdEvento { get; set; }
        [JsonProperty("EVEN_ID_CRIADOR_EVENTO")]
        public long IdCriadorDoEvento { get; set; }
        [JsonProperty("END_ID_ENDERECO")]
        public long IdEnderecoDoEvento { get; set; }
        [JsonProperty("EVEN_NOME_EVENTO")]
        public string NomeDoEvento { get; set; }
        [JsonProperty("EVEN_CATEGORIA_EVENTO")]
        public string CategoriaDoEvento { get; set; }
        [JsonProperty("EVEN_DESCRICAO_EVENTO")]
        public string DescricaoDoEvento { get; set; }
        [JsonProperty("EVEN_TIPO_EVENTO")]
        public bool TipoDoEvento { get; set; }
        [JsonProperty("EVEN_NUMERO_DE_PARTICIPANTES_EVENTO")]
        public int NumeroDeParticipantes { get; set; }
        [JsonProperty("EVEN_DATA_EVENTO")]
        public DateTime DataDoEvento { get; set; }
        [JsonProperty("EVEN_URL_EVENTO")]
        public string UrlDoEvento { get; set; }
        /*[JsonProperty("USUA_TIPO_USUARIO")]
        public string TipoUsuario { get; set; }
        [JsonProperty("USUA_DOCUMENTO_USUARIO")]
        public string Documento { get; set; }
        */
    }
}

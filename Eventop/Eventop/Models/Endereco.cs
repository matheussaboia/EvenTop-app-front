using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System;

namespace Eventop.Models
{
    class Endereco
    {
        [JsonProperty("END_ID_ENDERECO")]
        public int IdEndereco { get; set; }
        [JsonProperty("END_CEP_ENDERECO")]
        public string CepEndereco { get; set; }
        [JsonProperty("END_ESTADO_ENDERECO")]
        public string EstadoEndereco { get; set; }
        [JsonProperty("END_CIDADE_ENDERECO")]
        public string CidadeEndereco { get; set; }
        [JsonProperty("END_BAIRRO_ENDERECO")]
        public string BairroEndereco { get; set; }
        [JsonProperty("END_LOGRADOURO_ENDERECO")]
        public string LogradouroEndereco { get; set; }
        [JsonProperty("END_COMPLEMENTO_ENDERECO")]
        public string ComplementoEndereco { get; set; }
        [JsonProperty("END_NUMERO_ENDERECO")]
        public string NumeroEndereco { get; set; }
        [JsonProperty("END_LATITUDE_ENDERECO")]
        public string LatitudeEndereco { get; set; }
        [JsonProperty("END_LONGITUDE_ENDERECO")]
        public string LongitudeEndereco { get; set; }
    }
}
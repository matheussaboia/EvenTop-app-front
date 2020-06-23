using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventop.Models
{
    class Galeria
    {
        [JsonProperty("GAL_ID_GALERIA")]
        public long idGaleria { get; set; }
        [JsonProperty("GAL_BYTE_IMAGEM_GALERIA")]
        public byte[] imagemGaleria { get; set; }
        [JsonProperty("GAL_IMAGEM_PRINCIPAL_EVENTO")]
        public bool imagemPrincipal { get; set; }
    }
}

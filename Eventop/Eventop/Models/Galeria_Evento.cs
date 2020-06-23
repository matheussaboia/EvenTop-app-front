using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventop.Models
{
    class Galeria_Evento
    {
        [JsonProperty("EVEN_ID_EVENTO")]
        public long IdEvento { get; set; }
        [JsonProperty("GAL_ID_GALERIA")]
        public long IdGaleria { get; set; }
    }
}

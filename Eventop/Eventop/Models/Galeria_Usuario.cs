using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Eventop.Models
{
    class Galeria_Usuario
    {
        [JsonProperty("USUA_ID_USUARIO")]
        public long IdUsuario { get; set; }
        [JsonProperty("GAL_ID_GALERIA")]
        public long IdGaleria { get; set; }
    }
}
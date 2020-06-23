using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventop.Models
{

    public class Usuario
    {
        [JsonProperty("USUA_ID_USUARIO")]
        public int Id { get; set; }
        [JsonProperty("USUA_LOGIN_USUARIO")]
        public string Login { get; set; }
        [JsonProperty("USUA_NOME_USUARIO")]
        public string Nome { get; set; }
        [JsonProperty("USUA_SENHA_USUARIO")]
        public string Senha { get; set; }
        [JsonProperty("USUA_EMAIL_USUARIO")]
        public string Email { get; set; }
        [JsonProperty("USUA_WHATSAPP_USUARIO")]
        public string Whatsapp { get; set; }
        [JsonProperty("USUA_TIPO_USUARIO")]
        public string TipoUsuario { get; set; }
        [JsonProperty("USUA_DOCUMENTO_USUARIO")]
        public string Documento { get; set; }
        [JsonProperty("USUA_DATACRIACAO_USUARIO")]
        public DateTime DataDaCriacao { get; set; }

        //Chave estrangeira
        [JsonProperty("END_ID_ENDERECO")]
        public int? IdEndereco { get; set; }
    }
}

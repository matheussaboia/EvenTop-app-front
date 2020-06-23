using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Eventop.Util
{
    class ValidacaoCampos
    {
        public List<string> ValidarCampos(Object propriedades)
        {
            var errosValidacao = new List<String>();

            foreach (var item in propriedades.GetType().GetProperties()){
                CampoObrigatorio(item.Name, item.GetValue(propriedades), "Preencha o campo: "+ item.Name, errosValidacao);
            }

            return errosValidacao;
        }
        private void ValorMinimo(string nomeCampo, double valorMinimo, double valor, string mensagemPattern, List<string> errosValidacao)
        {
            if (valor < valorMinimo)
                errosValidacao.Add(string.Format(mensagemPattern, nomeCampo, valorMinimo));
        }
        private void CampoObrigatorio(string nomeCampo, object valor, string mensagemPattern, List<string> errosValidacao)
        {
            if (valor == null || (string)valor == "")
                errosValidacao.Add(string.Format(mensagemPattern, nomeCampo));
        }
    }
}

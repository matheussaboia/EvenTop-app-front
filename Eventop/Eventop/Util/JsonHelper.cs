using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Eventop.Models;
using Newtonsoft.Json;

namespace Eventop.Util
{
    class JsonHelper
    {
        private static string ipDoServidor = "http://192.168.1.247:3000/api/";

        public List<Object> PegarDadosEnderecoAPI(string servico, string dadosNaUrl)
        {
            var requisicaoWeb = WebRequest.CreateHttp(ipDoServidor + servico + "?" + dadosNaUrl);
            requisicaoWeb.Method = "GET";

            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                if (!objResponse.ToString().Contains("]") && objResponse.ToString() != "null") {
                    objResponse = objResponse.ToString() + "]";
                }

                var post = JsonConvert.DeserializeObject<List<Object>>(objResponse.ToString());

                streamDados.Close();
                resposta.Close();
                return post;
            }
        }

        public string getParaOServidor(string servico, string dadosNaUrl)
        {

            var requisicaoWeb = WebRequest.CreateHttp(ipDoServidor + servico + "?" + dadosNaUrl);
            requisicaoWeb.Method = "GET";
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                if (!objResponse.ToString().Contains("]") && objResponse.ToString() != "null")
                {
                    objResponse = objResponse.ToString() + "]";
                }
                //var post = JsonConvert.DeserializeObject<List<Object>>(objResponse.ToString());
                streamDados.Close();
                resposta.Close();
                return objResponse.ToString();
            }

           #region Antigo GET - Comentado
            /*
           HttpClient client = new HttpClient
           {
               BaseAddress = new Uri(ipDoServidor + servico)
           };
           try
           {
               var request = client.GetAsync("/api/"+ servico +"?"+ dadosNaUrl).Result;
               if (request.IsSuccessStatusCode)
               {
                   var responseJson = request.Content.ReadAsStringAsync().Result;
                   var resultado = JsonConvert.DeserializeObject<List<Object>>(responseJson);
                   if (resultado != null)
                   {
                       return resultado;
                   }
               }
               return null;
               //await Navigation.PushModalAsync(new MainPage());

               //Some risky client call that will call parallell code / async /TPL or in some way cause an AggregateException 

           }
           catch (AggregateException err)
           {
               foreach (var errInner in err.InnerExceptions)
               {
                   Debug.WriteLine(errInner); //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
               }
               return null;
           } */
           #endregion


        }

        public List<Usuario> loginNoServidor(string servico, string dadosNaUrl)
        {

        //http://192.168.1.247:3000/api/Usuarios?id=2

            var requisicaoWeb = WebRequest.CreateHttp(ipDoServidor + servico + "?" + dadosNaUrl);
            requisicaoWeb.Method = "GET";
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                if (!objResponse.ToString().Contains("]") && objResponse.ToString() != "null")
                {
                    objResponse = objResponse.ToString() + "]";
                }
                List<Usuario> post = JsonConvert.DeserializeObject<List<Usuario>>(objResponse.ToString());
                streamDados.Close();
                resposta.Close();
                return post;
            }

            #region Antigo GET - Comentado
            /*
           HttpClient client = new HttpClient
           {
               BaseAddress = new Uri(ipDoServidor + servico)
           };
           try
           {
               var request = client.GetAsync("/api/"+ servico +"?"+ dadosNaUrl).Result;
               if (request.IsSuccessStatusCode)
               {
                   var responseJson = request.Content.ReadAsStringAsync().Result;
                   var resultado = JsonConvert.DeserializeObject<List<Object>>(responseJson);
                   if (resultado != null)
                   {
                       return resultado;
                   }
               }
               return null;
               //await Navigation.PushModalAsync(new MainPage());

               //Some risky client call that will call parallell code / async /TPL or in some way cause an AggregateException 

           }
           catch (AggregateException err)
           {
               foreach (var errInner in err.InnerExceptions)
               {
                   Debug.WriteLine(errInner); //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
               }
               return null;
           } */
            #endregion


        }

        public Object enviarPostParaServidor(string json, string servico)
        {

            string result = "";

            //Envia as informações como POST para o servidor(WebAPI) fazer o registro no BANCO DE DADOS
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(ipDoServidor + servico);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                Debug.Write(json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            //Tenta ler as informações e verifica, no servidor, se elas foram enviadas com sucesso
            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpWebRequest.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();

                            if (result != null)
                            {
                                Object objeto = JsonConvert.DeserializeObject<Object>(result);
                                return objeto;
                                //return true;
                                //await DisplayAlert(eventoCadastrado.NomeDoEvento, "Falta pouco para você criar o seu evento!", "Vamos lá");
                                //await Navigation.PushModalAsync(new MainPage(), true);
                            }
                        }
                    }
                }
                return null;
                //return false;
            }
            catch (WebException h)
            {
                if (h.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)h.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            result = error;
                            return result;
                        }
                    }
                }
                return null;
                //return false;
            }
        }

        public string enviarPostParaServidorJson(string json, string servico)
        {

            string result = "";

            //Envia as informações como POST para o servidor(WebAPI) fazer o registro no BANCO DE DADOS
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(ipDoServidor + servico);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                Debug.Write(json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            //Tenta ler as informações e verifica, no servidor, se elas foram enviadas com sucesso
            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpWebRequest.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();

                            if (result != null)
                            {
                                //Object objeto = JsonConvert.DeserializeObject<Object>(result);
                                return result;
                                //return true;
                                //await DisplayAlert(eventoCadastrado.NomeDoEvento, "Falta pouco para você criar o seu evento!", "Vamos lá");
                                //await Navigation.PushModalAsync(new MainPage(), true);
                            }
                        }
                    }
                }
                return null;
                //return false;
            }
            catch (WebException h)
            {
                if (h.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)h.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            result = error;
                            return result;
                        }
                    }
                }
                return null;
                //return false;
            }
        }


        public string PutParaOServidor(string json, string servico)
        {

            string result = "";

            //Envia as informações como POST para o servidor(WebAPI) fazer o registro no BANCO DE DADOS
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(ipDoServidor + servico);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "PUT";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                Debug.Write(json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            //Tenta ler as informações e verifica, no servidor, se elas foram enviadas com sucesso
            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpWebRequest.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();

                            if (result != null)
                            {
                                //Object objeto = JsonConvert.DeserializeObject<Object>(result);
                                return result;
                                //return true;
                                //await DisplayAlert(eventoCadastrado.NomeDoEvento, "Falta pouco para você criar o seu evento!", "Vamos lá");
                                //await Navigation.PushModalAsync(new MainPage(), true);
                            }
                        }
                    }
                }
                return null;
                //return false;
            }
            catch (WebException h)
            {
                if (h.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)h.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            result = error;
                            return result;
                        }
                    }
                }
                return null;
                //return false;
            }
        }


        public string DeleteParaOServidor(string servico)
        {

            string result = "";

            //Envia as informações como POST para o servidor(WebAPI) fazer o registro no BANCO DE DADOS
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(ipDoServidor + servico);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "DELETE";

            //Tenta ler as informações e verifica, no servidor, se elas foram enviadas com sucesso
            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpWebRequest.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();

                            if (result != null)
                            {
                                //Object objeto = JsonConvert.DeserializeObject<Object>(result);
                                return result;
                                //return true;
                                //await DisplayAlert(eventoCadastrado.NomeDoEvento, "Falta pouco para você criar o seu evento!", "Vamos lá");
                                //await Navigation.PushModalAsync(new MainPage(), true);
                            }
                        }
                    }
                }
                return null;
                //return false;
            }
            catch (WebException h)
            {
                if (h.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)h.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            result = error;
                            return result;
                        }
                    }
                }
                return null;
                //return false;
            }
        }

    }
}
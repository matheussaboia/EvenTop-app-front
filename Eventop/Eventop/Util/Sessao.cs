using Eventop.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Eventop.Util
{
    class Sessao
    {
        private Usuario usuarioL;

        public Usuario CarregaLogin()
        {
            //Usuario log = new Login();
            long idusuarioV = 0;
            try{
                idusuarioV = Convert.ToInt64(Application.Current.Properties["idUsuario"].ToString());
            }
            catch{
                //await Navigation.PushModalAsync(new LoginPage(), true);
                return null;
            }
            //else if (log.RequestLogin(Request.Cookies["UserSistema"]) > 0)
            //{

            //    idusuarioV = log.RequestLogin(Request.Cookies["UserSistema"]);
            //}
            if (idusuarioV > 0){
                List<Usuario> usuarioQ = new JsonHelper().loginNoServidor("Usuarios", "id=" + idusuarioV);

                foreach (var usuarioF in usuarioQ){
                    usuarioL = usuarioF;
                }
                return usuarioL;
            }
            else{
                return null;
            }

        }

    }
}
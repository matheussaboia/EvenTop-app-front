using Eventop.Models;
using Eventop.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Eventop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicioDetalhesMaster : ContentPage
    {
        private Usuario usuarioL = new Sessao().CarregaLogin();

        public ListView ListView;

        public InicioDetalhesMaster()
        {
            InitializeComponent();

            lblNomeDoUsuario.Text = usuarioL.Login;

            BindingContext = new InicioDetalhesMasterViewModel();
            ListView = MenuItemsListView;
        }

        class InicioDetalhesMasterViewModel : INotifyPropertyChanged
        {
            
            private Usuario usuarioL = new Sessao().CarregaLogin();

            public ObservableCollection<InicioDetalhesMenuItem> MenuItems { get; set; }
            
            public InicioDetalhesMasterViewModel()
            {
                if (usuarioL.TipoUsuario == "Uma pessoa")
                {
                    MenuItems = new ObservableCollection<InicioDetalhesMenuItem>(new[]
                    {
                        new InicioDetalhesMenuItem { Id = 0, Title = "Perfil"},
                        new InicioDetalhesMenuItem { Id = 1, Title = "Meus eventos" },
                        new InicioDetalhesMenuItem { Id = 3, Title = "Avaliações" },
                        //new InicioDetalhesMenuItem { Id = 2, Title = "Certificado" },
                    //new InicioDetalhesMenuItem { Id = 4, Title = "Page 5" },
                    });
                }
                else
                {
                    MenuItems = new ObservableCollection<InicioDetalhesMenuItem>(new[]
                                    {
                    new InicioDetalhesMenuItem { Id = 0, Title = "Perfil" },
                    new InicioDetalhesMenuItem { Id = 1, Title = "Meus eventos" },
                    new InicioDetalhesMenuItem { Id = 3, Title = "Avaliações" },
                    new InicioDetalhesMenuItem { Id = 2, Title = "Certificado" },
                    //new InicioDetalhesMenuItem { Id = 4, Title = "Page 5" },
                });
                }

                
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}
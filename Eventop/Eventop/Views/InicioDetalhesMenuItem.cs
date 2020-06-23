using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventop.Views
{

    public class InicioDetalhesMenuItem
    {
        public InicioDetalhesMenuItem()
        {
            TargetType = typeof(InicioDetalhesDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
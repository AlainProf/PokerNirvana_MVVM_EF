using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerNirvana_MVVM_EF.ViewModel.Service
{
    interface IApplicationService
    {
        void ChangerVue<T>(T vue);
        void Configurer();
    }
}

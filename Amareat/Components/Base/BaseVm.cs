using System;
using System.Threading.Tasks;
using MvvmHelpers;

namespace Amareat.Components.Base
{
    public class BaseVm : BaseViewModel
    {
        public BaseVm()
        {
        }

        public virtual Task Init()
        {
            return Task.FromResult(true);
        }
    }

    public class BaseViewModel<TInitParameter> : BaseVm
    {
        public BaseViewModel()
        {

        }

        public virtual Task Init(TInitParameter parameter)
        {
            return Task.FromResult(true);
        }
    }
}

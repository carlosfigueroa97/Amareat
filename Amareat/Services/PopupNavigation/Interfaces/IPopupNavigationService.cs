using System;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Services.Navigation.Interfaces;

namespace Amareat.Services.PopupNavigation.Interfaces
{
    public interface IPopupNavigationService
    {
        void RegisterViewMapping(Type viewModel, Type view);

        Task<TResponse> PresentPopupPageAsync<TVM, TParameter, TResponse>(TParameter parameter) where TVM : BaseVm, INavigationAwaitable<TResponse>;

        Task PresentPopupPage<TVM, TParameter>(TParameter parameter) where TVM : BaseVm;

        Task PresentPopupPage<TVM>() where TVM : BaseVm;

        Task TogglePopupPage<TVM>() where TVM : BaseVm;

        Task TogglePopupPage<TVM, TInitParameter>(TInitParameter parameter) where TVM : BaseViewModel<TInitParameter>;

        Task DismissPopupPage(bool animated = true);

        Task ShowErrorDialog(string title, string detail, string cancel = null);
    }
}

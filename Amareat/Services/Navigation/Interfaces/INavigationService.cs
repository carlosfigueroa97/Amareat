using System;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Xamarin.Forms;

namespace Amareat.Services.Navigation.Interfaces
{
    public interface INavigationService
    {
        bool CanGoBack { get; }

        bool ModalCanGoBack { get; }

        void ClearBackStack();

        Task GoBack();

        Page CurrentPage();

        void RegisterViewMapping(Type viewModel, Type view);

        Task SetNewNavigationPage<TVM>() where TVM : BaseVm;

        Task SetNewNavigationPage<TVM, TInitParameter>(TInitParameter parameter) where TVM : BaseViewModel<TInitParameter>;

        Task NavigateTo<TVM>() where TVM : BaseVm;

        Task NavigateTo<TVM, TInitParameter>(TInitParameter parameter) where TVM : BaseViewModel<TInitParameter>;

        Task PopToRootAsync(bool animated = true);

        Task NavigateToModal<TVM>() where TVM : BaseVm;

        Task PopModalAsync(bool animated = true);
    }

    public interface INavigationAwaitable<T>
    {
        TaskCompletionSource<T> AwaitNavigationTask { get; set; }
    }
}

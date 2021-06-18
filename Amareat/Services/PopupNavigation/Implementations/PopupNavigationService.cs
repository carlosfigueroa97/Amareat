using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Components.Popups;
using Amareat.Components.Popups.Dialogs;
using Amareat.Services.Navigation.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using Amareat.Utils.ServiceLocator;
using Rg.Plugins.Popup.Pages;

namespace Amareat.Services.PopupNavigation.Implementations
{
    public class PopupNavigationService : IPopupNavigationService
    {
        #region Properties

        private IServiceLocator ServiceLocator => ServiceLocatorProvider.Instance.Current;

        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();

        #endregion

        public PopupNavigationService()
        {
        }

        #region Internal functions

        private PopupPage CreatePopupPageAndBind(Type viewModelType)
        {
            var viewType = GetViewTypeForViewModel(viewModelType);
            var view = Activator.CreateInstance(viewType) as PopupPage;
            var viewModel = ServiceLocator.Resolve(viewModelType) as BaseVm;
            view.BindingContext = viewModel;

            return view;
        }

        private Type GetViewTypeForViewModel(Type viewModelType)
        {
            Type viewType;
            if (!_map.TryGetValue(viewModelType, out viewType))
                throw new ArgumentException("View not found: ", viewModelType.FullName);

            return viewType;
        }

        #endregion

        public async Task DismissPopupPage(bool animated = true)
        {
            if (Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Any())
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        public async Task PresentPopupPage<TVM, TParameter>(TParameter parameter) where TVM : BaseVm
        {
            var popup = CreatePopupPageAndBind(typeof(TVM));
            var vm = popup.BindingContext as BaseViewModel<TParameter>;
            await vm.Init(parameter);
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        public async Task PresentPopupPage<TVM>() where TVM : BaseVm
        {
            var popup = CreatePopupPageAndBind(typeof(TVM));
            var vm = popup.BindingContext as BaseVm;
            await vm.Init();
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        public async Task<TResponse> PresentPopupPageAsync<TVM, TParameter, TResponse>(TParameter parameter) where TVM : BaseVm, INavigationAwaitable<TResponse>
        {
            TaskCompletionSource<TResponse> taskCompletionSource = new TaskCompletionSource<TResponse>();

            var page = CreatePopupPageAndBind(typeof(TVM));

            var viewModel = page.BindingContext as BaseViewModel<TParameter>;
            var awaitViewModel = page.BindingContext as INavigationAwaitable<TResponse>;

            if (awaitViewModel == null)
                throw new Exception("Target Viewmodel should implement INavigationAwaitable interface");

            awaitViewModel.AwaitNavigationTask = taskCompletionSource;

            await viewModel.Init(parameter);
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(page);

            return await taskCompletionSource.Task;
        }

        public void RegisterViewMapping(Type viewModel, Type view)
        {
            if (!_map.ContainsKey(viewModel))
                _map.Add(viewModel, view);
        }

        public Task ShowErrorDialog(string title, string detail, string cancel = null)
        {
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new InformationPopupPage(title, detail));
        }

        public Task TogglePopupPage<TVM>() where TVM : BaseVm
        {
            // TODO:
            throw new NotImplementedException();
        }

        public Task TogglePopupPage<TVM, TInitParameter>(TInitParameter parameter) where TVM : BaseViewModel<TInitParameter>
        {
            // TODO:
            throw new NotImplementedException();
        }

        public async Task ShowToastDialog(string textMessage, long miliseconds)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new ToastPopup(textMessage));

            await Task.Delay(TimeSpan.FromMilliseconds(miliseconds));

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}

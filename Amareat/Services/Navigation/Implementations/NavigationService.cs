using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Services.Navigation.Interfaces;
using Amareat.Utils.ServiceLocator;
using Xamarin.Forms;

namespace Amareat.Services.Navigation.Implementations
{
    public class NavigationService : INavigationService
    {
        #region Properties

        private INavigation MainNavigation => Application.Current.MainPage.Navigation;

        private IServiceLocator ServiceLocator => ServiceLocatorProvider.Instance.Current;

        private INavigation MainTabbedPageNavigation
        {
            get
            {
                var tabbedPage = Application.Current.MainPage as TabbedPage;

                if(tabbedPage != null)
                {
                    if (tabbedPage.Children.Any())
                    {
                        return tabbedPage.Navigation;
                    }
                }

                return null;
            }
        }

        private bool IsTabbedPageNavigation
        {
            get
            {
                if(Application.Current.MainPage is TabbedPage tabbedPage)
                {
                    if(tabbedPage.Children is MultiPage<Page>)
                    {
                        if(tabbedPage.Navigation != null)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();

        #endregion

        public NavigationService()
        {
        }

        #region Internal functions

        private Page CreatePageAndBind(Type viewModelType)
        {
            var viewType = GetViewTypeForViewModel(viewModelType);
            var view = Activator.CreateInstance(viewType) as Page;
            var viewModel = ServiceLocator.Resolve(viewModelType) as BaseVm;
            view.BindingContext = viewModel;

            return view;
        }

        private Type GetViewTypeForViewModel(Type viewModelType)
        {
            Type viewType;
            if (!_map.TryGetValue(viewModelType, out viewType))
            {
                throw new ArgumentException("View not found", viewModelType.FullName);
            }

            return viewType;
        }

        private async Task<Page> NavigateToView(Page view)
        {
            await MainNavigation.PushAsync(view, true);
            return view;
        }

        #endregion

        #region Methods

        public bool CanGoBack
        {
            get
            {
                return MainNavigation.NavigationStack != null && MainNavigation.NavigationStack.Count > 0;
            }
        }

        public bool ModalCanGoBack
        {
            get
            {
                return MainNavigation.ModalStack != null && MainNavigation.ModalStack.Count > 0;
            }
        }

        public void ClearBackStack()
        {
            if (MainNavigation.NavigationStack.Count <= 1)
                return;

            for (int i = 0; i < MainNavigation.NavigationStack.Count; i++)
            {
                MainNavigation.RemovePage(MainNavigation.NavigationStack[i]);
            }
        }

        public Page CurrentPage()
        {
            return MainNavigation.NavigationStack.LastOrDefault();
        }

        public async Task GoBack()
        {
            if (CanGoBack)
            {
                await MainNavigation.PopAsync(true);
            }
        }

        public async Task NavigateTo<TVM>() where TVM : BaseVm
        {
            Page view = CreatePageAndBind(typeof(TVM));
            //view.BackgroundColor = Colors.BlueBackgroundColor;
            var vm = view.BindingContext as BaseVm;
            await vm.Init();

            await NavigateToView(view);
        }

        public async Task NavigateTo<TVM, TInitParameter>(TInitParameter parameter) where TVM : BaseViewModel<TInitParameter>
        {
            Page view = CreatePageAndBind(typeof(TVM));
            //view.BackgroundColor = Colors.BlueBackgroundColor;
            var vm = view.BindingContext as BaseViewModel<TInitParameter>;
            await vm.Init(parameter);

            await NavigateToView(view);
        }

        public async Task NavigateToModal<TVM>() where TVM : BaseVm
        {
            var view = CreatePageAndBind(typeof(TVM));
            //view.BackgroundColor = Colors.BlueBackgroundColor;
            await MainNavigation.PushModalAsync(view, true);

            var vm = view.BindingContext as BaseVm;
            await vm.Init();
        }

        public async Task PopModalAsync(bool animated = true)
        {
            if (ModalCanGoBack)
            {
                await MainNavigation.PopModalAsync(animated);
            }
        }

        public async Task PopToRootAsync(bool animated = true)
        {
            await MainNavigation.PopToRootAsync(animated);
        }

        public void RegisterViewMapping(Type viewModel, Type view)
        {
            if (!_map.ContainsKey(viewModel))
                _map.Add(viewModel, view);
        }

        public async Task SetNewNavigationPage<TVM>() where TVM : BaseVm
        {
            Page view = CreatePageAndBind(typeof(TVM));
            //view.BackgroundColor = Colors.BlueBackgroundColor;
            Application.Current.MainPage = new NavigationPage(view)
            {
                //BarTextColor = Colors.BarTextColor,
                //BarBackgroundColor = Colors.BackgroundBarColor
            };

            var vm = view.BindingContext as BaseVm;
            await vm.Init();
        }

        public async Task SetNewNavigationPage<TVM, TInitParameter>(TInitParameter parameter) where TVM : BaseViewModel<TInitParameter>
        {
            var view = CreatePageAndBind(typeof(TVM));
            //view.BackgroundColor = Colors.BlueBackgroundColor;
            Application.Current.MainPage = new NavigationPage(view)
            {
                //BarTextColor = Colors.BarTextColor,
                //BarBackgroundColor = Colors.BackgroundBarColor
            };
            var vm = view.BindingContext as BaseViewModel<TInitParameter>;
            await vm.Init(parameter);
        }

        #endregion}
    }
}

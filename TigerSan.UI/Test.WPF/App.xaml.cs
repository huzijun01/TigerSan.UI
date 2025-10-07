using System.Windows;
using Test.WPF.ViewModels;

namespace Test.WPF
{
    public partial class App : PrismApplication
    {
        #region CreateShell
        protected override Window CreateShell()
        {
            var mainView = Container.Resolve<MainView>();
            var mainViewModel = Container.Resolve<MainViewModel>();
            mainView.DataContext = mainViewModel;

            return mainView;
        }
        #endregion CreateShell

        #region RegisterTypes
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 普通：
            containerRegistry.Register<MainView>();
            containerRegistry.Register<MainViewModel>();
            // 弹窗：
            containerRegistry.RegisterDialog<DialogWindow>();
            // 单例：
        }
        #endregion RegisterTypes
    }
}

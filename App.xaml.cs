using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;

namespace KfuPet
{
    public partial class App : Application
    {
        private MainWindow? _mainWindow;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _mainWindow = new MainWindow();
            _mainWindow.Activate();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(_mainWindow);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Hide();

            var splashWindow = new SplashWindow();
            splashWindow.SplashCompleted += (s, e) =>
            {
                appWindow.Show();
                _mainWindow?.PlayFadeInAnimation();
            };
            splashWindow.Activate();
        }
    }
}
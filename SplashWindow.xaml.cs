using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Windowing;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using Windows.UI;

namespace KfuPet
{
    public sealed partial class SplashWindow : Window
    {
        public event EventHandler? SplashCompleted;

        [DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        public SplashWindow()
        {
            InitializeComponent();
            SetupWindow();
            LoadVersion();
            SetupMask();
            StartEntranceAnimation();
            StartCountdown();
        }

        private void StartEntranceAnimation()
        {
            FadeInStoryboard.Begin();
        }

        private void SetupMask()
        {
            if (RootGrid.Background is SolidColorBrush brush)
            {
                var color = brush.Color;
                var transparent = Color.FromArgb(0, color.R, color.G, color.B);

                var gradient = new LinearGradientBrush
                {
                    StartPoint = new Windows.Foundation.Point(0, 0),
                    EndPoint = new Windows.Foundation.Point(1, 0)
                };
                gradient.GradientStops.Add(new GradientStop { Offset = 0, Color = color });
                gradient.GradientStops.Add(new GradientStop { Offset = 0.92, Color = color });
                gradient.GradientStops.Add(new GradientStop { Offset = 1, Color = transparent });

                SubtitleMask.Fill = gradient;
            }
        }

        private void LoadVersion()
        {
            try
            {
                var versionPath = Path.Combine(AppContext.BaseDirectory, "Config", "Version.json");
                var json = File.ReadAllText(versionPath);
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("version", out var versionElement))
                {
                    VersionText.Text = $"v{versionElement.GetString()}";
                }
            }
            catch
            {
                // 版本文件读取失败时不显示版本号
            }
        }

        private void SetupWindow()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            var dpi = GetDpiForWindow(hWnd);
            var scale = dpi / 96.0;
            var width = (int)(940 * scale);
            var height = (int)(560 * scale);
            appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));

            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest);
            if (displayArea != null)
            {
                var offsetX = (displayArea.WorkArea.Width - width) / 2;
                var offsetY = (displayArea.WorkArea.Height - height) / 2;
                appWindow.Move(new Windows.Graphics.PointInt32(offsetX, offsetY));
            }

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(null);

            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter != null)
            {
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = false;
                presenter.SetBorderAndTitleBar(false, false);
            }
        }

        private void StartCountdown()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(4);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                FadeOutStoryboard.Completed += FadeOutStoryboard_Completed;
                FadeOutStoryboard.Begin();
            };
            timer.Start();
        }

        private void FadeOutStoryboard_Completed(object? sender, object e)
        {
            FadeOutStoryboard.Completed -= FadeOutStoryboard_Completed;
            SplashCompleted?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
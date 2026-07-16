using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace KfuPet
{
    /// <summary>
    /// 透明无边框主窗口，承载角色渲染与鼠标交互。
    /// </summary>
    public partial class MainWindow : Window
    {
        // ── Win32 API ────────────────────────────────
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        // ── 长按拖动 ──────────────────────────────────
        private DispatcherTimer? _holdTimer;
        private bool _isDragging;
        private POINT _dragStartCursorPos;
        private double _windowStartLeft;
        private double _windowStartTop;
        private const int HOLD_DELAY_MS = 300;
        private const int DRAG_THRESHOLD = 5;

        public MainWindow()
        {
            InitializeComponent();
            CenterWindow();
        }

        private void CenterWindow()
        {
            var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            var screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            Left = (screenWidth - Width) / 2;
            Top = (screenHeight - Height) / 2;
        }

        /// <summary>
        /// 播放主窗口淡入动画。
        /// </summary>
        public void PlayFadeInAnimation()
        {
            var storyboard = (Storyboard)RootGrid.Resources["FadeInStoryboard"];
            storyboard.Begin();
        }

        private void RootGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GetCursorPos(out _dragStartCursorPos);
            _windowStartLeft = Left;
            _windowStartTop = Top;

            _holdTimer = new DispatcherTimer();
            _holdTimer.Interval = TimeSpan.FromMilliseconds(HOLD_DELAY_MS);
            _holdTimer.Tick += (s, args) =>
            {
                _holdTimer?.Stop();
                StartDrag();
            };
            _holdTimer.Start();

            Mouse.Capture(RootGrid);
        }

        private void RootGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_holdTimer == null && !_isDragging) return;

            if (!_isDragging)
            {
                // 还在长按等待中，检查是否移动超过阈值
                GetCursorPos(out POINT currentPos);
                int dx = currentPos.X - _dragStartCursorPos.X;
                int dy = currentPos.Y - _dragStartCursorPos.Y;

                if (dx * dx + dy * dy > DRAG_THRESHOLD * DRAG_THRESHOLD)
                {
                    _holdTimer?.Stop();
                    StartDrag();
                }
                return;
            }

            GetCursorPos(out POINT pos);
            Left = _windowStartLeft + (pos.X - _dragStartCursorPos.X);
            Top = _windowStartTop + (pos.Y - _dragStartCursorPos.Y);
        }

        private void StartDrag()
        {
            _isDragging = true;
            GetCursorPos(out _dragStartCursorPos);
            _windowStartLeft = Left;
            _windowStartTop = Top;
        }

        private void RootGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _holdTimer?.Stop();
            _holdTimer = null;

            if (_isDragging)
            {
                _isDragging = false;
            }

            Mouse.Capture(null);
        }
    }
}

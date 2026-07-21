using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using KfuPet.Models;

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

        [DllImport("user32.dll")]
        private static extern int GetDpiForWindow(IntPtr hWnd);

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
        private double _dpiScaleX = double.NaN;
        private double _dpiScaleY = double.NaN;

        // 此代码只做演示作用，后期会进行修改删除
        private Skeleton? _skeleton;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            int dpi = GetDpiForWindow(hwnd);
            double dpiScale = dpi / 96.0;
            Width = 512 / dpiScale;
            Height = 768 / dpiScale;
            CenterWindow();
            InitializeSkeleton(dpiScale);
        }

        // 此代码只做演示作用，后期会进行修改删除
        // 手动创建演示用骨骼结构，实际项目中将从配置文件加载角色数据
        private void InitializeSkeleton(double dpiScale)
        {
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton = new Skeleton();

            // ==================== 根骨骼 ====================
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "root",             // 骨骼唯一标识符
                Name = "Root",           // 骨骼显示名称
                ParentId = null,         // 无父骨骼，作为根节点
                LocalPosition = new Point(256 / dpiScale, 384 / dpiScale)  // 画布中心位置 (512x768)，转换为逻辑像素
            });

            // ==================== 身体骨骼 ====================
            // 调整身体长度，让整体比例协调（身体:腿 ≈ 1:1）
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "body",             // 身体骨骼
                Name = "Body",           // 身体
                ParentId = "root",       // 连接到根骨骼
                LocalPosition = new Point(0, -180 / dpiScale)   // 向上偏移180像素，转换为逻辑像素
            });

            // ==================== 颈部骨骼 ====================
            // 添加颈部骨骼，让头部转动更自然（点头、抬头）
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "neck",             // 颈部骨骼
                Name = "Neck",           // 颈部
                ParentId = "body",       // 连接到身体
                LocalPosition = new Point(0, -80 / dpiScale)    // 向上偏移80像素，转换为逻辑像素
            });

            // ==================== 头部骨骼 ====================
            // 头部连接到颈部，而非直接连接身体，方便表情和头部运动
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "head",             // 头部骨骼
                Name = "Head",           // 头部
                ParentId = "neck",       // 连接到颈部（而非直接连接身体）
                LocalPosition = new Point(0, -50 / dpiScale)    // 向上偏移50像素，转换为逻辑像素
            });

            // ==================== 左臂骨骼 ====================
            // 上臂：连接身体左侧肩部位置，水平向左延伸
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "arm_left_upper",       // 左上臂骨骼
                Name = "LeftArmUpper",       // 左上臂
                ParentId = "body",           // 连接到身体
                LocalPosition = new Point(-106 / dpiScale, 0)   // 向左偏移106，Y=0（水平延伸，肩部在身体中心高度），转换为逻辑像素
            });
            // 下臂：连接上臂末端，向下延伸（Y值为正数表示向下）
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "arm_left_lower",       // 左下臂骨骼
                Name = "LeftArmLower",       // 左下臂
                ParentId = "arm_left_upper", // 连接到左上臂（肘部关节）
                LocalPosition = new Point(0, 100 / dpiScale)    // 向下延伸100像素（自然下垂），转换为逻辑像素
            });

            // ==================== 右臂骨骼 ====================
            // 上臂：连接身体右侧肩部位置，水平向右延伸
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "arm_right_upper",      // 右上臂骨骼
                Name = "RightArmUpper",      // 右上臂
                ParentId = "body",           // 连接到身体
                LocalPosition = new Point(106 / dpiScale, 0)   // 向右偏移106，Y=0（水平延伸，肩部在身体中心高度），转换为逻辑像素
            });
            // 下臂：连接上臂末端，向下延伸（Y值为正数表示向下）
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "arm_right_lower",      // 右下臂骨骼
                Name = "RightArmLower",      // 右下臂
                ParentId = "arm_right_upper",// 连接到右上臂（肘部关节）
                LocalPosition = new Point(0, 100 / dpiScale)    // 向下延伸100像素（自然下垂），转换为逻辑像素
            });

            // ==================== 左腿骨骼 ====================
            // 大腿：连接身体下方左侧，缩短腿部比例更协调
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "leg_left_upper",       // 左大腿骨骼
                Name = "LeftLegUpper",       // 左大腿
                ParentId = "body",           // 连接到身体
                LocalPosition = new Point(-56 / dpiScale, 110 / dpiScale)  // 向左偏移56，向下偏移110（髋部位置），转换为逻辑像素
            });
            // 小腿：连接大腿末端，大腿小腿等长方便IK计算
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "leg_left_lower",       // 左小腿骨骼
                Name = "LeftLegLower",       // 左小腿
                ParentId = "leg_left_upper", // 连接到左大腿（膝盖关节）
                LocalPosition = new Point(0, 110 / dpiScale)    // 向下延伸110像素（与大腿等长），转换为逻辑像素
            });

            // ==================== 右腿骨骼 ====================
            // 大腿：连接身体下方右侧，缩短腿部比例更协调
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "leg_right_upper",      // 右大腿骨骼
                Name = "RightLegUpper",      // 右大腿
                ParentId = "body",           // 连接到身体
                LocalPosition = new Point(56 / dpiScale, 110 / dpiScale)   // 向右偏移56，向下偏移110（髋部位置），转换为逻辑像素
            });
            // 小腿：连接大腿末端，大腿小腿等长方便IK计算
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.AddBone(new Bone
            {
                Id = "leg_right_lower",      // 右小腿骨骼
                Name = "RightLegLower",      // 右小腿
                ParentId = "leg_right_upper",// 连接到右大腿（膝盖关节）
                LocalPosition = new Point(0, 110 / dpiScale)    // 向下延伸110像素（与大腿等长），转换为逻辑像素
            });

            // ==================== 更新变换 ====================
            // 此代码只做演示作用，后期会进行修改删除
            _skeleton.UpdateWorldTransforms();  // 计算所有骨骼的世界坐标
            // 此代码只做演示作用，后期会进行修改删除
            CharacterCanvas.Skeleton = _skeleton;  // 将骨骼绑定到渲染画布
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
                GetCursorPos(out POINT currentPos);
                int dx = currentPos.X - _dragStartCursorPos.X;
                int dy = currentPos.Y - _dragStartCursorPos.Y;

                if (dx * dx + dy * dy <= DRAG_THRESHOLD * DRAG_THRESHOLD)
                    return;

                // 超过阈值，立即进入拖拽（不重置起始参考点）
                _holdTimer?.Stop();
                _holdTimer = null;
                _isDragging = true;
                // 继续往下执行，立即更新窗口位置
            }

            GetCursorPos(out POINT pos);
            var (sx, sy) = GetDpiScale();
            Left = _windowStartLeft + (pos.X - _dragStartCursorPos.X) * sx;
            Top = _windowStartTop + (pos.Y - _dragStartCursorPos.Y) * sy;
        }

        private void StartDrag()
        {
            _isDragging = true;
        }

        private (double scaleX, double scaleY) GetDpiScale()
        {
            if (!double.IsNaN(_dpiScaleX))
                return (_dpiScaleX, _dpiScaleY);

            var source = PresentationSource.FromVisual(this);
            if (source?.CompositionTarget != null)
            {
                _dpiScaleX = source.CompositionTarget.TransformFromDevice.M11;
                _dpiScaleY = source.CompositionTarget.TransformFromDevice.M22;
            }
            else
            {
                _dpiScaleX = 1.0;
                _dpiScaleY = 1.0;
            }
            return (_dpiScaleX, _dpiScaleY);
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

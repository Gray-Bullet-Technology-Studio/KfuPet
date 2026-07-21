using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KfuPet.Models;
using KfuPet.Core.Rendering;

namespace KfuPet.Controls
{
    // 此代码只做演示作用，后期会进行修改删除
    // 使用黑色粗线条模拟骨骼结构，实际项目中将使用图片资源渲染角色
    public partial class CharacterCanvas : UserControl
    {
        // 此代码只做演示作用，后期会进行修改删除
        private SkeletonRenderer? _skeletonRenderer;
        private RenderContext? _renderContext;

        public static readonly DependencyProperty SkeletonProperty =
            DependencyProperty.Register(nameof(Skeleton), typeof(Skeleton), typeof(CharacterCanvas),
                new PropertyMetadata(null, OnSkeletonChanged));

        public Skeleton? Skeleton
        {
            get => (Skeleton?)GetValue(SkeletonProperty);
            set => SetValue(SkeletonProperty, value);
        }

        public CharacterCanvas()
        {
            InitializeComponent();
            Loaded += CharacterCanvas_Loaded;
            InitializeRenderer();
        }

        private void CharacterCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            var parent = Window.GetWindow(this);
            if (parent != null)
            {
                RenderCanvas.Width = parent.ActualWidth;
                RenderCanvas.Height = parent.ActualHeight;
            }
        }

        // 此代码只做演示作用，后期会进行修改删除
        private void InitializeRenderer()
        {
            _renderContext = new RenderContext(RenderCanvas);
            // 此代码只做演示作用，后期会进行修改删除
            _skeletonRenderer = new SkeletonRenderer(_renderContext);
        }

        private static void OnSkeletonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterCanvas canvas)
            {
                canvas.Render();
            }
        }

        // 此代码只做演示作用，后期会进行修改删除
        public void Render()
        {
            if (_skeletonRenderer != null && Skeleton != null)
            {
                Skeleton.UpdateWorldTransforms();
                // 此代码只做演示作用，后期会进行修改删除
                _skeletonRenderer.Render(Skeleton);
            }
        }

        // 此代码只做演示作用，后期会进行修改删除
        public void Clear()
        {
            // 此代码只做演示作用，后期会进行修改删除
            _skeletonRenderer?.Clear();
        }
    }
}
using System.Windows;
using System.Windows.Controls;

namespace KfuPet.Core.Rendering
{
    public class RenderContext
    {
        public Canvas Canvas { get; }

        public double DpiScaleX { get; set; } = 1.0;

        public double DpiScaleY { get; set; } = 1.0;

        public RenderContext(Canvas canvas)
        {
            Canvas = canvas;
        }

        public void UpdateDpiScale()
        {
            var source = PresentationSource.FromVisual(Canvas);
            if (source?.CompositionTarget != null)
            {
                DpiScaleX = source.CompositionTarget.TransformFromDevice.M11;
                DpiScaleY = source.CompositionTarget.TransformFromDevice.M22;
            }
            else
            {
                DpiScaleX = 1.0;
                DpiScaleY = 1.0;
            }
        }
    }
}
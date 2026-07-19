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
    }
}
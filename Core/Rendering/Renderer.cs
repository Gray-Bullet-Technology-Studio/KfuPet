using KfuPet.Models;

namespace KfuPet.Core.Rendering
{
    public abstract class Renderer
    {
        protected RenderContext Context { get; }

        protected Renderer(RenderContext context)
        {
            Context = context;
        }

        public abstract void Render(Skeleton skeleton);

        public abstract void Clear();
    }
}
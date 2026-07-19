using System.Windows;

namespace KfuPet.Models
{
    public class Character
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<string> Tags { get; } = new List<string>();

        public string DefaultAnimation { get; set; } = string.Empty;

        public string DefaultExpression { get; set; } = string.Empty;

        public Size CanvasSize { get; set; } = new Size(512, 768);

        public Point Origin { get; set; } = new Point(256, 768);

        public Skeleton? Skeleton { get; set; }

        public List<Attachment> Attachments { get; } = new List<Attachment>();

        public Dictionary<string, Expression> Expressions { get; } = new Dictionary<string, Expression>();

        public Dictionary<string, Animation> Animations { get; } = new Dictionary<string, Animation>();

        public void AddExpression(Expression expression)
        {
            Expressions[expression.Id] = expression;
        }

        public void AddAnimation(Animation animation)
        {
            Animations[animation.Id] = animation;
        }

        public Expression? FindExpression(string id)
        {
            return Expressions.TryGetValue(id, out var expression) ? expression : null;
        }

        public Animation? FindAnimation(string id)
        {
            return Animations.TryGetValue(id, out var animation) ? animation : null;
        }
    }
}
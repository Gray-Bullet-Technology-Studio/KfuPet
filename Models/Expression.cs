using System;

namespace KfuPet.Models
{
    public class ExpressionTarget
    {
        public string AttachmentId { get; set; } = string.Empty;

        public string ResourceId { get; set; } = string.Empty;

        public TimeSpan BlendDuration { get; set; } = TimeSpan.FromSeconds(0.2);
    }

    public class Expression
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public List<ExpressionTarget> Targets { get; } = new List<ExpressionTarget>();

        public TimeSpan? Duration { get; set; }

        public string? RevertExpression { get; set; }
    }
}
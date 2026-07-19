using System.Windows;

namespace KfuPet.Models
{
    public enum InterpolationType
    {
        Linear,
        Bezier,
        Step
    }

    public class BoneState
    {
        public Point? Position { get; set; }

        public double? Rotation { get; set; }

        public Point? Scale { get; set; }

        public InterpolationType Interpolation { get; set; } = InterpolationType.Linear;
    }

    public class Keyframe
    {
        public double Time { get; set; }

        public Dictionary<string, BoneState> BoneStates { get; } = new Dictionary<string, BoneState>();

        public Dictionary<string, string> AttachmentStates { get; } = new Dictionary<string, string>();
    }
}
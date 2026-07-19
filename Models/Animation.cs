namespace KfuPet.Models
{
    public class Animation
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double Duration { get; set; }

        public int Fps { get; set; } = 30;

        public bool Loop { get; set; } = true;

        public List<Keyframe> Keyframes { get; } = new List<Keyframe>();
    }
}
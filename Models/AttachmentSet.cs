namespace KfuPet.Models
{
    public class AttachmentSet
    {
        public Dictionary<string, string> Resources { get; } = new Dictionary<string, string>();

        public string? DefaultResource { get; set; }

        public string? CurrentResourceId { get; set; }

        public string? GetCurrentResourcePath()
        {
            var resourceId = CurrentResourceId ?? DefaultResource;
            if (resourceId != null && Resources.TryGetValue(resourceId, out var path))
            {
                return path;
            }
            return null;
        }

        public void SetResource(string resourceId)
        {
            if (Resources.ContainsKey(resourceId))
            {
                CurrentResourceId = resourceId;
            }
        }
    }
}
using KfuPet.Services.Commands;

namespace KfuPet.Services
{
    /// <summary>
    /// 情感服务（预留空壳，后续实现）。
    /// </summary>
    internal class EmotionService : ICommandService
    {
        public string ServiceName => "emotion";

        public CommandResponse Execute(string action, Dictionary<string, object>? parameters)
        {
            return CommandResponse.Fail("EmotionService not implemented yet");
        }
    }
}

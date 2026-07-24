using KfuPet.Services.Commands;

namespace KfuPet.Services
{
    /// <summary>
    /// 视觉服务（预留空壳，后续实现）。
    /// </summary>
    internal class VisionService : ICommandService
    {
        public string ServiceName => "vision";

        public CommandResponse Execute(string action, Dictionary<string, object>? parameters)
        {
            return CommandResponse.Fail("VisionService not implemented yet");
        }
    }
}

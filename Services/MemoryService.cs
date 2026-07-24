using KfuPet.Services.Commands;

namespace KfuPet.Services
{
    /// <summary>
    /// 记忆服务（预留空壳，后续实现）。
    /// </summary>
    internal class MemoryService : ICommandService
    {
        public string ServiceName => "memory";

        public CommandResponse Execute(string action, Dictionary<string, object>? parameters)
        {
            return CommandResponse.Fail("MemoryService not implemented yet");
        }
    }
}

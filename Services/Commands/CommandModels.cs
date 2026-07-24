namespace KfuPet.Services.Commands
{
    public class CommandRequest
    {
        public string Service { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public Dictionary<string, object>? Params { get; set; }
    }

    public class CommandResponse
    {
        public bool Success { get; set; }

        public object? Data { get; set; }

        public string? Error { get; set; }

        public static CommandResponse Ok(object? data = null)
        {
            return new CommandResponse { Success = true, Data = data };
        }

        public static CommandResponse Fail(string error)
        {
            return new CommandResponse { Success = false, Error = error };
        }
    }

    public interface ICommandService
    {
        string ServiceName { get; }

        CommandResponse Execute(string action, Dictionary<string, object>? parameters);
    }
}

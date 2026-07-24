namespace KfuPet.Services.Ipc
{
    public class IpcRequest
    {
        public string Action { get; set; } = string.Empty;

        public Dictionary<string, object>? Params { get; set; }
    }

    public class IpcResponse
    {
        public bool Success { get; set; }

        public object? Data { get; set; }

        public string? Error { get; set; }

        public static IpcResponse Ok(object? data = null)
        {
            return new IpcResponse { Success = true, Data = data };
        }

        public static IpcResponse Fail(string error)
        {
            return new IpcResponse { Success = false, Error = error };
        }
    }
}

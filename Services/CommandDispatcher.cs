using KfuPet.Services.Commands;

namespace KfuPet.Services
{
    internal class CommandDispatcher
    {
        private readonly Dictionary<string, ICommandService> _services = new();

        public void RegisterService(ICommandService service)
        {
            _services[service.ServiceName.ToLowerInvariant()] = service;
        }

        public CommandResponse Dispatch(CommandRequest request)
        {
            if (string.IsNullOrEmpty(request.Service))
            {
                return CommandResponse.Fail("Service name is required");
            }

            if (string.IsNullOrEmpty(request.Action))
            {
                return CommandResponse.Fail("Action is required");
            }

            var serviceKey = request.Service.ToLowerInvariant();
            if (!_services.TryGetValue(serviceKey, out var service))
            {
                return CommandResponse.Fail($"Unknown service: {request.Service}");
            }

            try
            {
                return service.Execute(request.Action, request.Params);
            }
            catch (Exception ex)
            {
                return CommandResponse.Fail(ex.Message);
            }
        }

        public CommandResponse Dispatch(string service, string action, Dictionary<string, object>? parameters = null)
        {
            return Dispatch(new CommandRequest
            {
                Service = service,
                Action = action,
                Params = parameters
            });
        }
    }
}

using Microsoft.AspNetCore.SignalR;

namespace TestAzureSignalRServer.Hub
{
    public class TestHub : Hub<ITestHub>
    {
        public bool InvokeMeOften()
        {
            return true;
        }
    }
}
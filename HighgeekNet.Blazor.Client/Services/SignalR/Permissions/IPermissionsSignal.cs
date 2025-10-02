
namespace HighgeekNet.Blazor.Client.Services.SignalR.Permissions
{
    public interface IPermissionClient
    {
        Task<bool> CheckUserPermissionAsync(string uuid, string permission);
        Task<bool> CheckGroupPermissionAsync(string group, string permission);
    }

    public interface IPermissionServer
    {
        Task<bool> CheckUserPermissionAsync(string uuid, string permission);
        Task<bool> CheckGroupPermissionAsync(string group, string permission);
    }
}

using System.Collections.Generic;
using System.Diagnostics;
using HighgeekNet.Common.Server.Config;
using OpenApi.Highgeek.LuckPermsApi.Api;
using OpenApi.Highgeek.LuckPermsApi.Client;
using OpenApi.Highgeek.LuckPermsApi.Model;

namespace HighgeekNet.Blazor.Client.Services.Authorization
{
    public class LuckPermsService
    {
        private readonly Configuration config = new Configuration();
        private readonly UsersApi _usersApi;
        private readonly GroupsApi _groupApi;

        public LuckPermsService()
        {
            config.BasePath = ConfigProvider.Get("LuckPermsOptions:Url");
            _usersApi = new UsersApi(config);
            _groupApi = new GroupsApi(config);
        }

        public UsersApi GetUsersApi()
        {
            return _usersApi;
        }

        public GroupsApi GetGroupsApi()
        {
            return _groupApi;
        }

        public async Task<string> GetUserUuidAsync(string username)
        {
            var result = await _usersApi.GetUserLookupAsync(username);
            return result.UniqueId.ToString();
        }

        public async Task<User> GetUserAsync(string uuid)
        {
            var result = await _usersApi.GetUserAsync(new Guid(uuid));
            return result;
        }

        public async Task<Group> GetGroupAsync(string groupname)
        {
            var result = await _groupApi.GetGroupAsync(groupname);
            return result;
        }

        public async Task<List<string>> SearchForNodeInGroupsAsync(string permission)
        {
            var result = await _groupApi.GetGroupSearchAsync(permission);
            List<string> list = new List<string>();
            foreach (var item in result)
            {
                foreach (var results in item.Results)
                {
                    if (results.Value == true)
                    {
                        list.Add(item.Name);
                    }
                }
            }
            return list;
        }

        public async Task<List<string>> SearchForNodeInUsersAsync(string permission)
        {
            var result = await _usersApi.GetUserSearchAsync(permission);
            List<string> list = new List<string>();
            foreach (var item in result)
            {
                list.Add(item.UniqueId.ToString());
            }
            return list;
        }

        public async Task<bool> HasUserNode(string permission, string uuid)
        {
            var result = await _usersApi.GetUserPermissionCheckAsync(new Guid(uuid), permission);
            if (result.Node != null)
            { 
                return result.Node.Value;
            } 
            else 
            {
                return false; 
            }
        }

        public async Task<bool> HasGroupNode(string permission, string groupName)
        {
            var result = await _groupApi.GetGroupPermissionCheckAsync(groupName, permission);
            if (result.Node != null)
            {
                return result.Node.Value;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> HasPermissionAsync(string permission, string uuid)
        {
            if(await HasUserNode(permission, uuid))
            {
                return true;
            }
            else
            {
                var groupList = await GetUserGroupsAsync(uuid);
                foreach (var group in groupList)
                {
                    if ("group." + group == permission)
                    {
                        return true;
                    }
                    else
                    {
                        var groups = await SearchForNodeInGroupsAsync(permission);
                        if (groups.Contains(group))
                        {  
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<List<string>> GetUserGroupsAsync(string uuid)
        {
            List<string> userGroups = new List<string>();
            var user = await GetUserAsync(uuid);
            return user.ParentGroups;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Models.API.Requests.Users;
using Amareat.Models.API.Responses.Users;

namespace Amareat.Services.Api.Interfaces
{
    public interface IUsersService
    {
        Task<bool> SaveUser(SaveUser signIn, CancellationToken cancellationToken);

        Task<bool> SignIn(SignIn signIn, CancellationToken cancellationToken);

        Task<bool> RefreshUserToken(CancellationToken cancellationToken);

        Task<bool> Logout(CancellationToken cancellationToken);

        Task<User> GetUser(string id, CancellationToken cancellationToken);

        Task<UserList> GetUsers(CancellationToken cancellationToken);

        Task<bool> EditUser(EditUser editUser, CancellationToken cancellationToken);

        Task<UserList> SearchUser(string searchValue, CancellationToken cancellationToken);
    }
}

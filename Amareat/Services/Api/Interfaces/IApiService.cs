using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Models.API.Base;
using Amareat.Models.API.Requests;
using Amareat.Models.API.Requests.Users;
using Amareat.Models.API.Responses;
using Amareat.Models.API.Responses.Users;

namespace Amareat.Services.Api.Interfaces
{
    public interface IApiService
    {
        #region Users

        //TODO: Save User

        Task<ResponseSignIn> UserSignIn(SignIn signIn, CancellationToken cancellationToken);

        Task<RefreshToken> RefreshUserToken(CancellationToken cancellationToken);

        Task<GenericSuccessfulResponse> Logout(CancellationToken cancellationToken);

        Task<User> GetUser(CancellationToken cancellationToken);

        Task<List<User>> GetUsers(CancellationToken cancellationToken);

        Task<GenericSuccessfulResponse> EditUser(EditUser editUser, CancellationToken cancellationToken);

        Task<List<User>> SearchUser(CancellationToken cancellationToken);
        #endregion

        #region Users
        #endregion
        #region Users
        #endregion
        #region Users
        #endregion
        #region Users
        #endregion
    }
}

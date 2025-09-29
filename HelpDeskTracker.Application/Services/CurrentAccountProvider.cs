using EFCoreSecondLevelCacheInterceptor;
using HelpDeskTracker.Application.Exceptions;
using HelpDeskTracker.Application.Interfaces;
using HelpDeskTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTracker.Application.Services
{
    public class CurrentAccountProvider : ICurrentAccountProvider
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IAuthenticationDataProvider _authenticationDataProvider;

        public CurrentAccountProvider(IAuthenticationDataProvider authenticationDataProvider,
            IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _authenticationDataProvider = authenticationDataProvider;
        }

        public async Task<int?> GetAccountId()
        {
            var userId = _authenticationDataProvider.GetUserId();
            if(userId != null)
            {
                return await _applicationDbContext.AccountUsers
                    .Where(au => au.UserId == userId.Value)
                    .OrderBy(au => au.Id)
                    .Select(au => (int?)au.AccountId)
                    .Cacheable()
                    .FirstOrDefaultAsync();
                    
            }

            return null;
        }

        public async Task<Account> GetAuthenticatedAccount()
        {
            var accountId = await GetAccountId();
            if(accountId == null)
            {
                throw new UnauthorizedAccessException();
            }

            var account = await _applicationDbContext.Accounts.Cacheable()
                .FirstOrDefaultAsync(a => a.Id == accountId.Value);

            if (account == null)
            {
                throw new ErrorException("AccountDoesNotExist");
            }

            return account;
        }
    }
}

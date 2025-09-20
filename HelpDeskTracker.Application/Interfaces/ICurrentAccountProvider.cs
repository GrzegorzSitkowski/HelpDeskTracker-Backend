using HelpDeskTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTracker.Application.Interfaces
{
    public interface ICurrentAccountProvider
    {
        Task<Account> GetAuthenticatedAccount();

        Task<int?> GetAccountId();
    }
}

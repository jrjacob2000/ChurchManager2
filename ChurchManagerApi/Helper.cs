using ChurchManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChurchManagerApi
{
    public static class UserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            if (user.FindFirst("Id") == null)
                throw new Exception("User Id cannot be null");

            return Guid.Parse(user.FindFirst("Id").Value);
        }

    }

    public static class AccountChartExtensions
    {
        public static IEnumerable<KeyValuePair<string,string>> GetAccountRegister(this IEnumerable<AccountChart> accts)
        {
            var result = accts.Where(x => x.ShowInRegister)
                 .Select(s => new KeyValuePair<string, string>(s.Id.ToString(), string.Format("{0} - {1}", s.Type.ToString().Substring(0, 3).ToUpper(), s.Name)))//, Selected = s.Id.ToString() == selectedValue })
                 .OrderBy(o => o.Value)
                 .ToList();
            return result;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetAccounts(this IEnumerable<AccountChart> accts)
        {
            var result = accts.Where(x => x.Type != AccountChartTypeEnum.FundBalance)
                 .Select(s => new KeyValuePair<string, string>( s.Id.ToString(), string.Format("{0} - {1}", s.Type.ToString().Substring(0, 3).ToUpper(), s.Name)))//, Selected = s.Id.ToString() == selectedValue })
                 .OrderBy(o => o.Value)
                 .ToList();
            return result;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetFunds(this IEnumerable<AccountChart> accts)
        {
            var result = accts.Where(x => x.Type == AccountChartTypeEnum.FundBalance)
                 .Select(s => new KeyValuePair<string, string>( s.Id.ToString(), s.Name))//, Selected = s.Id.ToString() == selectedValue })
                 .OrderBy(o => o.Value)
                 .ToList();
            return result;
        }
    }

}

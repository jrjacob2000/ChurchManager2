using ChurchManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
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

    public static class ReportExtensions
    {
        public static DataTable ToPivotTable<T, TColumn, TRow, TData>(this IEnumerable<T> source, Func<T, TColumn> columnSelector, Expression<Func<T, TRow>> rowSelector, Func<IEnumerable<T>, TData> dataSelector)
        {
            DataTable table = new DataTable();
            var rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            table.Columns.Add(new DataColumn(rowName));
            var columns = source.Select(columnSelector).Distinct();

            foreach (var column in columns)
                table.Columns.Add(new DataColumn(column.ToString()));

            var rows = source.GroupBy(rowSelector.Compile())
                             .Select(rowGroup => new
                             {
                                 Key = rowGroup.Key,
                                 Values = columns.GroupJoin(
                                     rowGroup,
                                     c => c,
                                     r => columnSelector(r),
                                     (c, columnGroup) => dataSelector(columnGroup))
                             });

            foreach (var row in rows)
            {
                if (row.Key != null)
                {
                    var dataRow = table.NewRow();
                    var items = row.Values.Cast<object>().ToList();
                    items.Insert(0, row.Key);
                    dataRow.ItemArray = items.ToArray();
                    table.Rows.Add(dataRow);
                }
            }

            return table;
        }

        public static List<dynamic> ToDynamicList(this DataTable dt)
        {
            var list = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                list.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return list;
        }
    }

}

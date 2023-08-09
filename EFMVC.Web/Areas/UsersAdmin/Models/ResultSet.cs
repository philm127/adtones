using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace EFMVC.Web.Areas.UsersAdmin.Models
{
    public class ResultSet
    {
        public List<UserResult> GetResult(string search, string sortOrder, int start, int length, List<UserResult> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<UserResult> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }
       
        public IQueryable<UserResult> FilterResult(string search, List<UserResult> dtResult, List<string> columnFilters)
        {
            IQueryable<UserResult> results = dtResult.AsQueryable();
            int[] UserId = new int[dtResult.Count()];
            int[] UserStatus = new int[dtResult.Count()];
            DateTime fromdate=new DateTime();
            DateTime todate=new DateTime();
            if (!String.IsNullOrEmpty(columnFilters[0]))
            {
                if (columnFilters[0] == "null")
                {
                    columnFilters[0] = null;
                }
            }

            if (!String.IsNullOrEmpty(columnFilters[1]))
            {
                if (columnFilters[1] == "null")
                {
                    columnFilters[1] = null;
                }
            }

            if (!String.IsNullOrEmpty(columnFilters[2]))
            {
                if (columnFilters[2] != "null")
                {
                    UserId = columnFilters[2].Split(',').Select(int.Parse).ToArray();
                }
                else
                {
                    columnFilters[2] = null;
                }
            }
            
            if (!String.IsNullOrEmpty(columnFilters[3]))
            {
                if (columnFilters[3] != "null")
                {
                    var data = columnFilters[3].Split(',').ToArray();
                    fromdate = Convert.ToDateTime(data[0]);
                    todate = Convert.ToDateTime(data[1]);
                }
                else
                {
                    columnFilters[3] = null;

                }
            }
            if (!String.IsNullOrEmpty(columnFilters[4]))
            {
                if (columnFilters[4] != "null")
                {
                    UserStatus = columnFilters[4].Split(',').Select(int.Parse).ToArray();
                }
                else
                {
                    columnFilters[4] = null;
                }
            }
            results = results.Where(p => (columnFilters[0] == null || (p.Email.ToLower() != null && p.Email.ToLower().Contains(columnFilters[0].ToLower())))
                     && (columnFilters[1] == null || ( p.MSISDN != null && p.MSISDN.Contains(columnFilters[1])))
                     && (columnFilters[2] == null || (UserId.Contains(p.UserId)))
                     && (columnFilters[3] == null || (p.DateCreated.Value.Date >= fromdate.Date && p.DateCreated.Value.Date<=todate.Date))
                     && (columnFilters[4] == null || (UserStatus.Contains(p.Activated)))
                     );
            

            return results;
        }
    }
}
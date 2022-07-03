using WebServiceTask.FilterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Helpers
{
    public static class QueryExtension
    {
        public static bool HasPrevious(this BaseFilter filter)
        {
            return (filter.Page > 1);
        }

        public static bool HasNext(this BaseFilter filter, int totalCount)
        {
            return (filter.Page < (int)GetTotalPages(filter, totalCount)); 
        }

        public static double GetTotalPages(this BaseFilter filter, int totalCount)
        {
            return Math.Ceiling(totalCount / (double)filter.PageCount);
        }

        public static bool HasQuery(this BaseFilter filter)
        {
            return !String.IsNullOrEmpty(filter.Search);
        }
    }
}

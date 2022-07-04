using WebServiceTask.FilterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Helpers
{
    public static class QueryExtension
    {
        public static bool HasPrevious(this GetAllRequest filter)
        {
            return (filter.Page > 1);
        }

        public static bool HasNext(this GetAllRequest filter, int totalCount)
        {
            return (filter.Page < (int)GetTotalPages(filter, totalCount)); 
        }

        public static double GetTotalPages(this GetAllRequest filter, int totalCount)
        {
            return Math.Ceiling(totalCount / (double)filter.PageCount);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.FilterModels
{
    /// <summary>
    ///  Most in some situations dynamic pagination is forced.
    /// If this is a web service written for mobile applications, I would say it is necessary.
    /// </summary>
    public class GetAllRequest
    {
        private const int maxPageCount = 10;
        public int Page { get; set; } = 1;

        private int _pageCount = maxPageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
    }
}

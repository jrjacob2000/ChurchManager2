using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchManagerApi.Models
{
    public class GridFilter
    {
        public Guid AccountRegisterId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
    }
}

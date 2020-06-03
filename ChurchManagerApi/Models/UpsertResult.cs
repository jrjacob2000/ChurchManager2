using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchManagerApi.Models
{
    public class UpsertResult
    {
        internal Transaction Data { get; set; }
        internal List<string> Errors { get; set; }
    }
}

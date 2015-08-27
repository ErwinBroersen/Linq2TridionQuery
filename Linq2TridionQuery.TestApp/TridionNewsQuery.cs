using Linq2TridionQuery;
using Linq2TridionQuery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2TridionQuery.TestApp
{
    public class TridionNewsQuery : TridionQueryObject
    {
        public string priority { get; set; }
        [Metadata]
        public DateTime item_publication_date { get; set; }
    }
}

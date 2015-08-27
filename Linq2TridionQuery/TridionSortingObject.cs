using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2TridionQuery
{
    public class TridionSortingObject : ITridionSortingObject
    {
        public bool ComponentSchema { get; set; }
        public bool ItemCreationDate { get; set; }
        public bool ItemId { get; set; }
        public bool ItemInitialPublication { get; set; }
        public bool ItemLastPublish { get; set; }
        public bool ItemMajorVersion { get; set; }
        public bool ItemMinorVersion { get; set; }
        public bool ItemOwningPublication { get; set; }
        public bool ItemPublication { get; set; }
        public bool ItemTitle { get; set; }
        public bool ItemTrustee { get; set; }
        public bool ItemType { get; set; }
        public bool PageFilename { get; set; }
        public bool PageTemplate { get; set; }
        public bool PageURL { get; set; }
    }
}

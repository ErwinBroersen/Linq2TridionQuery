using System;
namespace Linq2TridionQuery
{
    public interface ITridionSortingObject
    {
        bool ComponentSchema { get; set; }
        bool ItemCreationDate { get; set; }
        bool ItemId { get; set; }
        bool ItemInitialPublication { get; set; }
        bool ItemLastPublish { get; set; }
        bool ItemMajorVersion { get; set; }
        bool ItemMinorVersion { get; set; }
        bool ItemOwningPublication { get; set; }
        bool ItemPublication { get; set; }
        bool ItemTitle { get; set; }
        bool ItemTrustee { get; set; }
        bool ItemType { get; set; }
        bool PageFilename { get; set; }
        bool PageTemplate { get; set; }
        bool PageURL { get; set; }
    }
}

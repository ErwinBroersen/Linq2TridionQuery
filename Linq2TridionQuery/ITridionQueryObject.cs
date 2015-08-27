using System;

namespace Linq2TridionQuery
{
    public interface ITridionQueryObject
    {
        string BinaryType { get; set; }
        string CategoryName { get; set; }
        bool IsMultimedia { get; set; }

        int ItemId { get; set; }
        int ItemSchemaId { get; set; }
        int ItemTemplateId { get; set; }
        string ItemTitle { get; set; }
        int ItemType { get; set; }
        DateTime ItemCreationDate { get; set; }
        DateTime ItemInitialPublishDate { get; set; }
        DateTime ItemLastPublishedDate { get; set; }
        DateTime ItemModificationDate { get; set; }

        int PageTemplateId { get; set; }
        string PageURL { get; set; }

        int PublicationId { get; set; }
        string PublicationKey { get; set; }
        string PublicationMultimediaPath { get; set; }
        string PublicationMultimediaURL { get; set; }
        string PublicationPath { get; set; }
        string PublicationTitle { get; set; }
        string PublicationURL { get; set; }

        string SchemaTitle { get; set; }

        string StructureGroupUri { get; set; }
        string StructureGroupDirectory { get; set; }
        string StructureGroupTitle { get; set; }

        bool TaxonomyUsedForIdentification { get; set; }
        string TaxonomyUri { get; set; }

        // Sorting...
        ITridionSortingObject Sort { get; set; }
    }
}

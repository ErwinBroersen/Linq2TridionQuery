using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2TridionQuery
{
    public class TridionQueryObject : ITridionQueryObject
    {
        public string BinaryType { get; set; }
        public string CategoryName { get; set; }
        public bool IsMultimedia { get; set; }

        public int ItemId { get; set; }
        public int ItemSchemaId { get; set; }
        public int ItemTemplateId { get; set; }
        public string ItemTitle { get; set; }
        public int ItemType { get; set; }
        public DateTime ItemCreationDate { get; set; }
        public DateTime ItemInitialPublishDate { get; set; }
        public DateTime ItemLastPublishedDate { get; set; }
        public DateTime ItemModificationDate { get; set; }

        public int PageTemplateId { get; set; }
        public string PageURL { get; set; }

        public int PublicationId { get; set; }
        public string PublicationKey { get; set; }
        public string PublicationMultimediaPath { get; set; }
        public string PublicationMultimediaURL { get; set; }
        public string PublicationPath { get; set; }
        public string PublicationTitle { get; set; }
        public string PublicationURL { get; set; }

        public string SchemaTitle { get; set; }

        public string StructureGroupUri { get; set; }
        public string StructureGroupDirectory { get; set; }
        public string StructureGroupTitle { get; set; }

        public bool TaxonomyUsedForIdentification { get; set; }
        public string TaxonomyUri { get; set; }

        public ITridionSortingObject Sort { get; set; }
    }
}

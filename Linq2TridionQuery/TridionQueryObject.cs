using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using tridion = Tridion.ContentDelivery.DynamicContent.Query;

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

        public IStructureGroupUri StructureGroupUri { get; set; }
        public IStructureGroupDirectory StructureGroupDirectory { get; set; }
        public IStructureGroupTitle StructureGroupTitle { get; set; }

        public bool TaxonomyUsedForIdentification { get; set; }
        public string TaxonomyUri { get; set; }
        public ITaxonomyKeyword TaxonomyKeyword { get; set; }
        public ITaxonomyKeywordDescription TaxonomyKeywordDescription { get; set; }
        public ITaxonomyKeywordKey TaxonomyKeywordKey { get; set; }
        public ITaxonomyKeywordName TaxonomyKeywordName { get; set; }

        public ITridionSortingObject Sort { get; set; }
    }

    public sealed class StructureGroupUri : IStructureGroupUri
    {
        public bool IncludeChild { get; set; }
        public string Uri { get; set; }
    }

    public sealed class StructureGroupDirectory : IStructureGroupDirectory
    {
        public bool IncludeChild { get; set; }
        public string Directory { get; set; }
        public tridion.FieldOperator Operator { get; set; }
    }

    public sealed class StructureGroupTitle : IStructureGroupTitle
    {
        public bool IncludeChild { get; set; }
        public string Title { get; set; }
        public tridion.FieldOperator Operator { get; set; }
    }

    public abstract class TaxonomyKeywordBase : ITaxonomy
    {
        public int? PublicationId { get; set; }
        public int? TaxonomyId { get; set; }
        public string TaxonomyUri { get; set; }
        public bool IncludeKeywordBranch { get; set; }
    }

    public abstract class TaxonomyKeywordOperatorBase : TaxonomyKeywordBase
    {
        public tridion.FieldOperator Operator { get; set; }
    }

    public sealed class TaxonomyKeyword : TaxonomyKeywordBase, ITaxonomyKeyword
    {
        public int? KeywordId { get; set; }
        public string KeywordUri { get; set; }
    }

    public sealed class TaxonomyKeywordDescription : TaxonomyKeywordOperatorBase, ITaxonomyKeywordDescription
    {
        public string KeywordDescription { get; set; }
    }

    public sealed class TaxonomyKeywordKey : TaxonomyKeywordOperatorBase, ITaxonomyKeywordKey
    {
        public string KeywordKey { get; set; }
    }

    public sealed class TaxonomyKeywordName : TaxonomyKeywordOperatorBase, ITaxonomyKeywordName
    {
        public string KeywordName { get; set; }
    }
}

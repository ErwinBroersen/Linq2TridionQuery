using System;
using System.Linq.Expressions;
using tridion = Tridion.ContentDelivery.DynamicContent.Query;

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

        IStructureGroupUri StructureGroupUri { get; set; }
        IStructureGroupDirectory StructureGroupDirectory { get; set; }
        IStructureGroupTitle StructureGroupTitle { get; set; }

        bool TaxonomyUsedForIdentification { get; set; }
        string TaxonomyUri { get; set; }
        ITaxonomyKeyword TaxonomyKeyword { get; set; }
        ITaxonomyKeywordDescription TaxonomyKeywordDescription { get; set; }
        ITaxonomyKeywordKey TaxonomyKeywordKey { get; set; }
        ITaxonomyKeywordName TaxonomyKeywordName { get; set; }

        // Sorting...
        ITridionSortingObject Sort { get; set; }
    }

    public interface IOperator
    {
        tridion.FieldOperator Operator { get; set; }
    }

    public interface IIncludeKeywordBranches
    {
        bool IncludeKeywordBranch { get; set; }
    }

    public interface IIncludeChildren
    {
        bool IncludeChild { get; set; }
    }

    public interface IStructureGroupUri : IIncludeChildren
    {
        string Uri { get; set; }
    }

    public interface IStructureGroupDirectory : IIncludeChildren, IOperator
    {
        string Directory { get; set; }
    }

    public interface IStructureGroupTitle : IIncludeChildren, IOperator
    {
        string Title { get; set; }
    }

    public interface ITaxonomy
    {
        int? PublicationId { get; set; }
        int? TaxonomyId { get; set; }
        string TaxonomyUri { get; set; }
    }

    public interface ITaxonomyKeyword : ITaxonomy, IIncludeKeywordBranches
    {
        int? KeywordId { get; set; }
        string KeywordUri { get; set; }
    }

    public interface ITaxonomyKeywordDescription : ITaxonomy, IIncludeKeywordBranches, IOperator
    {
        string KeywordDescription { get; set; }
    }

    public interface ITaxonomyKeywordKey : ITaxonomy, IIncludeKeywordBranches, IOperator
    {
        string KeywordKey { get; set; }
    }

    public interface ITaxonomyKeywordName : ITaxonomy, IIncludeKeywordBranches, IOperator
    {
        string KeywordName { get; set; }
    }
}

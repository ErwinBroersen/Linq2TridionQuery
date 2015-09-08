# Linq2TridionQuery
Create a Tridion Query through a Linq Provider

This project tries to make it easier to create a Tridion query through the use of a LinqProvider.
The following other parts of Linq are available:
Where, OrderBy, OrderByDescending, ThenBy, ThenByDescending, Take, Skip

A TridionQueryFactory is needed to initiate a Query object where the Linq Provider can create a IQueryable object:
	TridionQueryFactory qx = new TridionQueryFactory();
You can also create a Generic TridionQueryFactory for specific Objects you want to query (i.e. a News Component in the Broker Database with metadata).
This specific object must implement ITridionQueryObject and is best to be derived from the TridionQueryObject (which implements ITridionQueryObject).
    public class TridionNewsQuery : TridionQueryObject
    {
        [Metadata]
        public DateTime item_publication_date { get; set; }
    }
In the TridionNewsQuery the item_publication_date decorated with the MetadataAttribute which tells the QueryProvider that this is a metadata field which can be used for creating a Tridion Query and that it van be used for Sorting the Tridion Query.
	TridionQueryFactory<TridionNewsQuery> qo = new TridionQueryFactory<TridionNewsQuery>();
    IQueryable q = qo.Query.Where(c => c.item_publication_date == DateTime.Now).OrderByDescending(c => c.item_publication_date);
You van now start iterating through the items retrieved by the query (q):
	foreach (var item in q)
    {
	}

The TridionQueryObject class contains almost all criteria mentioned in the API through either fields or objects and can be used in the Where part of the Linq Query:
    Tridion.ContentDelivery.DynamicContent.Query..::.BinaryTypeCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.CategoryCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ContentItemCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.CustomMetaDateRangeCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.CustomMetaKeyCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.CustomMetaKeyStringCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.CustomMetaStringRangeCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.CustomMetaValueCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemCreationDateCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemInitialPublishDateCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemLastPublishedDateCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemModificationDateCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemReferenceCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemSchemaCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemTemplateCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemTitleCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemTypeCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.KeywordCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.MultimediaCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.PageTemplateCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.PageURLCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.PublicationCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.PublicationKeyCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.PublicationMultimediaPathCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.PublicationMultimediaURLCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.PublicationPathCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.PublicationTitleCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.PublicationURLCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.SchemaTitleCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.StructureGroupCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.StructureGroupDirectoryCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.StructureGroupTitleCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.TaxonomyCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.TaxonomyKeywordCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.TaxonomyKeywordDescriptionCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.TaxonomyKeywordKeyCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.TaxonomyKeywordNameCriteria
    Tridion.ContentDelivery.DynamicContent.Query..::.TaxonomyUsedForIdentificationCriteria

The Skip and Take parts of the Linq Query result in LimitFilters on the Tridion Query (Limit or Page filter).

The Ordering of the Tridion Query can be done by a variety of fixed fields that you can order by from the Tridion API:
    Tridion.ContentDelivery.DynamicContent.Query..::.ComponentSchemaColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.CustomMetaKeyColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemCreationDateColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemIdColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemInitialPublicationColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemLastPublishColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemMajorVersionColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemMinorVersionColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemOwningPublicationColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemPublicationColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemTitleColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemTrusteeColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.ItemTypeColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.PageFilenameColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.PageTemplateColumn
    Tridion.ContentDelivery.DynamicContent.Query..::.PageURLColumn
These Sortfields are available from the TridionSortingObject which is part of the TridionQueryObject.
When sorting is needed on the Tridion Query you can add a sorting clause to the Linq Query either from the predefined columns or a Metadata column defined (.OrderByDescending(c => c.Sort.ItemTitle)) 


The TestApp shows a variety of queries in comments as shown below:
int schId = 1;
int aantal = 4;
TaxonomyKeywordName tkn = new TaxonomyKeywordName() { TaxonomyUri = "tcm:1-1-1024", KeywordName = "KeywordName" };

IQueryable q = qo.Query.Where(c => (c.ItemSchemaId == schId || c.ItemSchemaId == 2 || c.ItemSchemaId == 3) && c.PublicationId == 1).OrderByDescending(c => c.Sort.ItemTitleSorting);
IQueryable q = qo.Query.Where(c => (c.ItemSchemaId == 1 && c.PublicationId == 1) || (c.ItemSchemaId == 2 && c.PublicationId == 2)).OrderByDescending(c => c.Sort.ItemTitleSorting);
IQueryable q = qo.Query.Where(c => c.ItemSchemaId == schId).OrderByDescending(c => c.Sort.ItemTitle).Skip(aantal).Take(aantal);
IQueryable q = qo.Query.Where(c => c.item_publication_date == DateTime.Now).OrderByDescending(c => c.item_publication_date);
IQueryable q = qo.Query.Where(c => c.StructureGroupDirectory == new StructureGroupDirectory() { Directory = "DirectoryName"});
IQueryable q = qo.Query.Where(c => c.TaxonomyKeywordName == new TaxonomyKeywordName() { TaxonomyUri = "tcm:1-1-1024", KeywordName = "KeywordName" });
IQueryable q = qo.Query.Where(c => c.TaxonomyKeywordName == tkn );

using Linq2TridionQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using tridion = Tridion.ContentDelivery.DynamicContent.Query;

namespace Linq2TridionQuery.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example Tridion Query
            //tridion.PublicationCriteria pubCrit = new tridion.PublicationCriteria(22);
            //tridion.ItemSchemaCriteria schCrit = new tridion.ItemSchemaCriteria(36029);

            //tridion.Query query = new tridion.Query(new tridion.AndCriteria(pubCrit, schCrit));
            //string[] results = query.ExecuteQuery();
            TridionQueryFactory qx = new TridionQueryFactory();
            TridionQueryFactory<TridionNewsQuery> qo = new TridionQueryFactory<TridionNewsQuery>();
            int schId = 3364; // 36029;
            int aantal = 4;
            TaxonomyKeywordName tkn = new TaxonomyKeywordName() { TaxonomyUri = "tcm:1-2-1024", KeywordName = "KeywordName" };
            // IQueryable q = qo.Query.Where(c => (c.ItemSchemaId == schId || c.ItemSchemaId == 36029 || c.ItemSchemaId == 36029) && c.PublicationId == 8).OrderByDescending(c => c.Sort.ItemTitleSorting);
            // IQueryable q = qo.Query.Where(c => (c.ItemSchemaId == 1 && c.PublicationId == 1) || (c.ItemSchemaId == 2 && c.PublicationId == 2)).OrderByDescending(c => c.Sort.ItemTitleSorting);
            IQueryable q = qo.Query.Where(c => c.ItemSchemaId == schId).OrderByDescending(c => c.Sort.ItemTitle).Skip(aantal).Take(aantal);
            // IQueryable q = qo.Query.Where(c => c.item_publication_date == DateTime.Now).OrderByDescending(c => c.item_publication_date);
            // IQueryable q = qo.Query.Where(c => c.StructureGroupDirectory == new StructureGroupDirectory() { Directory = "system"});
            // IQueryable q = qo.Query.Where(c => c.TaxonomyKeywordName == new TaxonomyKeywordName() { TaxonomyUri = "tcm:1-2-1024", KeywordName = "KeywordName" });
            // IQueryable q = qo.Query.Where(c => c.TaxonomyKeywordName == tkn );
            foreach (var item in q)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }
    }
}

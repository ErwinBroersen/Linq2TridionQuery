using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2TridionQuery
{
    public class TridionQueryFactory
    {
        public Query<TridionQueryObject> Query;

        public TridionQueryFactory()
        {
            Query = new Query<TridionQueryObject>(new TridionQueryProvider());
        }
    }

    public class TridionQueryFactory<T>
    {
        public Query<T> Query;

        public TridionQueryFactory()
        {
            Query = new Query<T>(new TridionQueryProvider());
        }

        public TridionQueryFactory(ITridionQueryObject queryObject)
        {
            Query = new Query<T>(new TridionQueryProvider(), queryObject.GetType());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using tridion = Tridion.ContentDelivery.DynamicContent.Query;

namespace Linq2TridionQuery
{
    public class TridionQueryProvider : QueryProvider
    {
        public override object Execute(Expression expression)
        {
            return Translate(expression).ExecuteQuery();
        }

        private tridion.Query Translate(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            return new TridionQueryTranslator().Translate(expression);
        }
    }
}

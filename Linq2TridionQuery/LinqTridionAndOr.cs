using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tridion = Tridion.ContentDelivery.DynamicContent.Query;

namespace Linq2TridionQuery
{
    public class LinqTridionAndOr
    {
        List<tridion.Criteria> Criteria = null;
        List<LinqTridionAndOr> ChildCriteria = null;

        internal LinqTridionAndOr AddChildCriteria(LinqTridionAndOr child)
        {
            if (ChildCriteria == null)
            {
                ChildCriteria = new List<LinqTridionAndOr>();
            }
            ChildCriteria.Add(child);
            return child;
        }

        internal void AddCriteria(tridion.Criteria crit)
        {
            if(Criteria == null)
            {
                Criteria = new List<tridion.Criteria>();
            }
            Criteria.Add(crit);
        }

        internal tridion.Criteria GetQueryCriteria()
        {
            return ProcessCriteriaRecursive(this);
        }

        private tridion.Criteria ProcessCriteriaRecursive(LinqTridionAndOr crit)
        {
            if(crit.Criteria != null && crit.Criteria.Count > 0)
            {
                if (crit is LinqTridionAnd)
                {
                    return new tridion.AndCriteria(crit.Criteria.ToArray());
                }
                else if (crit is LinqTridionOr)
                {
                    return new tridion.OrCriteria(crit.Criteria.ToArray());
                }
            }
            else if (crit.ChildCriteria != null && crit.ChildCriteria.Count > 0)
            {
                List<tridion.Criteria> criteria = null;
                foreach (var item in crit.ChildCriteria)
                {
                    tridion.Criteria processedCrit = ProcessCriteriaRecursive(item);
                    if (processedCrit != null)
                    {
                        criteria.Add(processedCrit);
                    }
                }

                if (crit is LinqTridionAnd)
                {
                    return new tridion.AndCriteria(criteria.ToArray());
                }
                else if (crit is LinqTridionOr)
                {
                    return new tridion.OrCriteria(criteria.ToArray());
                }
            }
            return null;
        }
    }

    public class LinqTridionAnd : LinqTridionAndOr
    {

    }

    public class LinqTridionOr : LinqTridionAndOr
    {

    }
}

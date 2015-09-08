using Linq2TridionQuery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using tridion = Tridion.ContentDelivery.DynamicContent.Query;

namespace Linq2TridionQuery
{
    internal class TridionQueryTranslator : ExpressionVisitor
    {
        tridion.Query q;
        LinqTridionAndOr criteriaNodes = null;
        LinqTridionAndOr currentCriteriaNode;
        ExpressionType currentExpressionType;

        List<tridion.SortParameter> sort;
        tridion.ResultFilter filter;

        /// <summary>
        /// 
        /// </summary>
        internal TridionQueryTranslator()
        {
            sort = new List<tridion.SortParameter>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal tridion.Query Translate(Expression expression)
        {
            this.Visit(expression);

            // Add all the criteria, at least one should always be present
            if (criteriaNodes == null)
            {
                throw new NotSupportedException("There are no criteria present in the query, a criteria should always be present to execute the query on the broker database");
            }

            this.q = new tridion.Query();
            q.Criteria = criteriaNodes.GetQueryCriteria();

            // Set the ResultFilter for the query if present
            if (filter != null)
            {
                q.SetResultFilter(filter);
            }

            // Set sorting for the query if present
            if (sort != null && sort.Count > 0)
            {
                foreach (var item in sort)
                {
                    q.AddSorting(item);
                }
            }
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) || m.Method.DeclaringType == typeof(Enumerable))
            {
                switch (m.Method.Name)
                {
                    case "Where":
                        return this.BindWhere(m, m.Type, m.Arguments[0], GetLambda(m.Arguments[1]));
                    case "OrderBy":
                        return this.BindOrderBy(m, m.Arguments[0], GetLambda(m.Arguments[1]), tridion.SortParameter.Ascending);
                    case "OrderByDescending":
                        return this.BindOrderBy(m, m.Arguments[0], GetLambda(m.Arguments[1]), tridion.SortParameter.Descending);
                    case "ThenBy":
                        return this.BindOrderBy(m, m.Arguments[0], GetLambda(m.Arguments[1]), tridion.SortParameter.Ascending);
                    case "ThenByDescending":
                        return this.BindOrderBy(m, m.Arguments[0], GetLambda(m.Arguments[1]), tridion.SortParameter.Descending);
                    case "Take":
                        if (m.Arguments.Count == 2)
                        {
                            return this.BindTake(m.Arguments[0], m.Arguments[1]);
                        }
                        break;
                    case "Skip":
                        if (m.Arguments.Count == 2)
                        {
                            return this.BindSkip(m.Arguments[0], m.Arguments[1]);
                        }
                        break;
                }
                throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
            }
            return base.VisitMethodCall(m);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        private Expression BindSkip(Expression source, Expression skip)
        {
            this.Visit(skip);
            // Use the ResultFilter (PagingFilter) as a default
            if (filter != null)
            {
                switch (filter.GetType().Name)
                {
                    case "LimitFilter":
                        filter = new tridion.PagingFilter((Int32)((ConstantExpression)skip).Value, ((tridion.LimitFilter)filter).ResultLimit);
                        break;
                    case "PagingFilter":
                        filter = new tridion.PagingFilter((Int32)((ConstantExpression)skip).Value, ((tridion.PagingFilter)filter).ResultLimit);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                filter = new tridion.PagingFilter((Int32)((ConstantExpression)skip).Value, -1);
            }
            this.Visit(source);
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private Expression BindTake(Expression source, Expression take)
        {
            this.Visit(take);
            // Use the ResultFilter (LimitFilter) as a default
            if (filter != null)
            {
                if (filter is tridion.PagingFilter)
                {
                    filter = new tridion.PagingFilter(((tridion.PagingFilter)filter).FirstResult, (Int32)((ConstantExpression)take).Value);
                }
            }
            else
            {
                filter = new tridion.LimitFilter((Int32)((ConstantExpression)take).Value);
            }
            this.Visit(source);
            return source;
        }

        /// <summary>
        /// Binds the SortColumn to the Tridion Query based on the expression(s) provided
        /// </summary>
        /// <param name="m"></param>
        /// <param name="expression"></param>
        /// <param name="p"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        private Expression BindOrderBy(MethodCallExpression m, Expression expression, LambdaExpression p, tridion.SortDirection orderType)
        {
            this.Visit(expression);
            if (p.Body is MemberExpression)
            {
                MemberExpression MemberExp = (MemberExpression)p.Body;
                string SortFieldName = MemberExp.Member.Name;
                switch (SortFieldName)
                {
                    case "ComponentSchema":
                        sort.Add(new tridion.SortParameter(new tridion.ComponentSchemaColumn(), orderType));
                        break;
                    case "ItemCreationDate":
                        sort.Add(new tridion.SortParameter(new tridion.ItemCreationDateColumn(), orderType));
                        break;
                    case "ItemId":
                        sort.Add(new tridion.SortParameter(new tridion.ItemIdColumn(), orderType));
                        break;
                    case "ItemInitialPublication":
                        sort.Add(new tridion.SortParameter(new tridion.ItemInitialPublicationColumn(), orderType));
                        break;
                    case "ItemLastPublish":
                        sort.Add(new tridion.SortParameter(new tridion.ItemLastPublishColumn(), orderType));
                        break;
                    case "ItemMajorVersion":
                        sort.Add(new tridion.SortParameter(new tridion.ItemMajorVersionColumn(), orderType));
                        break;
                    case "ItemMinorVersion":
                        sort.Add(new tridion.SortParameter(new tridion.ItemMinorVersionColumn(), orderType));
                        break;
                    case "ItemOwningPublication":
                        sort.Add(new tridion.SortParameter(new tridion.ItemOwningPublicationColumn(), orderType));
                        break;
                    case "ItemPublication":
                        sort.Add(new tridion.SortParameter(new tridion.ItemPublicationColumn(), orderType));
                        break;
                    case "ItemTitle":
                        sort.Add(new tridion.SortParameter(new tridion.ItemTitleColumn(), orderType));
                        break;
                    case "ItemTrustee":
                        sort.Add(new tridion.SortParameter(new tridion.ItemTrusteeColumn(), orderType));
                        break;
                    case "ItemType":
                        sort.Add(new tridion.SortParameter(new tridion.ItemTypeColumn(), orderType));
                        break;
                    case "PageFilename":
                        sort.Add(new tridion.SortParameter(new tridion.PageFilenameColumn(), orderType));
                        break;
                    case "PageTemplate":
                        sort.Add(new tridion.SortParameter(new tridion.PageTemplateColumn(), orderType));
                        break;
                    case "PageURL":
                        sort.Add(new tridion.SortParameter(new tridion.PageURLColumn(), orderType));
                        break;
                    default:
                        // Assume Metadata as SortColumn, it has to be attributed with a metadata aatribute and defined as metadata in the schema within Tridion
                        if (CheckMemberAttributes(MemberExp.Member))
                        {
                            // Check the type of the SortFieldName
                            switch (Type.GetTypeCode(MemberExp.Type))
                            {
                                case TypeCode.String:
                                    sort.Add(new tridion.SortParameter(new tridion.CustomMetaKeyColumn(SortFieldName, tridion.MetadataType.STRING), orderType));
                                    break;
                                case TypeCode.Boolean:
                                case TypeCode.Byte:
                                case TypeCode.Decimal:
                                case TypeCode.Double:
                                case TypeCode.Int16:
                                case TypeCode.Int32:
                                case TypeCode.Int64:
                                case TypeCode.SByte:
                                case TypeCode.Single:
                                case TypeCode.UInt16:
                                case TypeCode.UInt32:
                                case TypeCode.UInt64:
                                    sort.Add(new tridion.SortParameter(new tridion.CustomMetaKeyColumn(SortFieldName, tridion.MetadataType.FLOAT), orderType));
                                    break;
                                case TypeCode.DateTime:
                                    sort.Add(new tridion.SortParameter(new tridion.CustomMetaKeyColumn(SortFieldName, tridion.MetadataType.DATE), orderType));
                                    break;
                                default:
                                    // Unknown MetadataSortField provided
                                    throw new NotSupportedException(string.Format("The MetadataSortField type '{0}' for '{1}' is not supported", Type.GetTypeCode(MemberExp.Member.DeclaringType), SortFieldName));
                            }
                        }
                        else
                        {
                            throw new NotSupportedException(string.Format("Unknown SortColumn '{0}' defined", ((MemberExpression)p.Body).Member.Name));
                        }
                        break;
                }
            }
            this.Visit(p.Body);
            return m;
        }

        /// <summary>
        /// Check whether a property is attributed the MetadataAttribute
        /// If so, then it can be used as a column for the Tridion Query
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private bool CheckMemberAttributes(MemberInfo memberInfo)
        {
            bool ContainsMetadataAttribute = false;
            if (memberInfo != null && memberInfo.GetCustomAttributes(typeof(MetadataAttribute)).Count() > 0)
            {
                ContainsMetadataAttribute = true;
            }

            return ContainsMetadataAttribute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="type"></param>
        /// <param name="expression"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private Expression BindWhere(MethodCallExpression m, Type type, Expression expression, LambdaExpression p)
        {
            this.Visit(expression);
            this.Visit(p.Body);
            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static LambdaExpression GetLambda(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            if (e.NodeType == ExpressionType.Constant)
            {
                return ((ConstantExpression)e).Value as LambdaExpression;
            }
            return e as LambdaExpression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            LinqTridionAndOr currentLocalCriteriaNode = null;
            ExpressionType currentLocalExpressionType = ExpressionType.Add;

            if (b.Left is MemberExpression)
            {
                if (criteriaNodes == null)
                {
                    // We have a single where without AndAlso or OrElse if criteriaNodes is empty
                    criteriaNodes = new LinqTridionAnd();
                    currentCriteriaNode = criteriaNodes;
                }

                // CategorizationCriteria

                // KeywordCriteria

                switch (((MemberExpression)b.Left).Member.Name)
                {
                    case "IsMultimedia":
                        currentCriteriaNode.AddCriteria(new tridion.MultimediaCriteria((bool)((ConstantExpression)b.Right).Value));
                        break;
                    case "BinaryType":
                        currentCriteriaNode.AddCriteria(new tridion.BinaryTypeCriteria((string)((ConstantExpression)b.Right).Value));
                        break;
                    case "CategoryName":
                        currentCriteriaNode.AddCriteria(new tridion.CategoryCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;

                    case "ItemId":
                        currentCriteriaNode.AddCriteria(new tridion.ItemReferenceCriteria((int)((ConstantExpression)b.Right).Value));
                        break;
                    case "ItemSchemaId":
                        currentCriteriaNode.AddCriteria(new tridion.ItemSchemaCriteria((int)((ConstantExpression)b.Right).Value));
                        break;
                    case "ItemTemplateId":
                        currentCriteriaNode.AddCriteria(new tridion.ItemTemplateCriteria((int)((ConstantExpression)b.Right).Value));
                        break;
                    case "ItemTitle":
                        currentCriteriaNode.AddCriteria(new tridion.ItemTitleCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;
                    case "ItemType":
                        currentCriteriaNode.AddCriteria(new tridion.ItemTypeCriteria((int)((ConstantExpression)b.Right).Value));
                        break;
                    case "ItemCreationDate":
                        currentCriteriaNode.AddCriteria(new tridion.ItemCreationDateCriteria((DateTime)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;
                    case "ItemInitialPublishDate":
                        currentCriteriaNode.AddCriteria(new tridion.ItemInitialPublishDateCriteria((DateTime)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;
                    case "ItemLastPublishedDate":
                        currentCriteriaNode.AddCriteria(new tridion.ItemLastPublishedDateCriteria((DateTime)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;
                    case "ItemModificationDate":
                        currentCriteriaNode.AddCriteria(new tridion.ItemModificationDateCriteria((DateTime)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;

                    case "PageTemplateId":
                        currentCriteriaNode.AddCriteria(new tridion.PageTemplateCriteria((int)((ConstantExpression)b.Right).Value));
                        break;
                    case "PageURL":
                        currentCriteriaNode.AddCriteria(new tridion.PageURLCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;

                    case "PublicationId":
                        currentCriteriaNode.AddCriteria(new tridion.PublicationCriteria((int)((ConstantExpression)b.Right).Value));
                        break;
                    case "PublicationKey":
                        currentCriteriaNode.AddCriteria(new tridion.PublicationKeyCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;
                    case "PublicationMultimediaPath":
                        currentCriteriaNode.AddCriteria(new tridion.PublicationMultimediaPathCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;
                    case "PublicationMultimediaURL":
                        currentCriteriaNode.AddCriteria(new tridion.PublicationMultimediaURLCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;
                    case "PublicationPath":
                        currentCriteriaNode.AddCriteria(new tridion.PublicationPathCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;
                    case "PublicationTitle":
                        currentCriteriaNode.AddCriteria(new tridion.PublicationTitleCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;
                    case "PublicationURL":
                        currentCriteriaNode.AddCriteria(new tridion.PublicationURLCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;

                    case "SchemaTitle":
                        currentCriteriaNode.AddCriteria(new tridion.SchemaTitleCriteria((string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType)));
                        break;

                    case "StructureGroupUri":
                        IStructureGroupUri sgu = (StructureGroupUri)((ConstantExpression)b.Right).Value;
                        currentCriteriaNode.AddCriteria(new tridion.StructureGroupCriteria(sgu.Uri, sgu.IncludeChild));
                        break;
                    case "StructureGroupDirectory":
                        IStructureGroupDirectory sgd = (StructureGroupDirectory)((ConstantExpression)b.Right).Value;
                        currentCriteriaNode.AddCriteria(new tridion.StructureGroupDirectoryCriteria(sgd.Directory, sgd.Operator ?? tridion.Criteria.Equal, sgd.IncludeChild));
                        break;
                    case "StructureGroupTitle":
                        IStructureGroupTitle sgt = (StructureGroupTitle)((ConstantExpression)b.Right).Value;
                        currentCriteriaNode.AddCriteria(new tridion.StructureGroupTitleCriteria(sgt.Title, sgt.Operator ?? tridion.Criteria.Equal, sgt.IncludeChild));
                        break;

                    case "TaxonomyUsedForIdentification":
                        currentCriteriaNode.AddCriteria(new tridion.TaxonomyUsedForIdentificationCriteria((bool)((ConstantExpression)b.Right).Value));
                        break;
                    case "TaxonomyUri":
                        currentCriteriaNode.AddCriteria(new tridion.TaxonomyCriteria((string)((ConstantExpression)b.Right).Value));
                        break;
                    case "TaxonomyKeyword":
                        ITaxonomyKeyword tk = (TaxonomyKeyword)((ConstantExpression)b.Right).Value;
                        if(tk.PublicationId != null && tk.TaxonomyId != null && tk.KeywordId != null)
                        {
                            currentCriteriaNode.AddCriteria(new tridion.TaxonomyKeywordCriteria(tk.PublicationId.Value, tk.TaxonomyId.Value, tk.KeywordId.Value, tk.IncludeKeywordBranch));
                        }
                        else if(!string.IsNullOrEmpty(tk.TaxonomyUri) && !string.IsNullOrEmpty(tk.KeywordUri))
                        {
                            currentCriteriaNode.AddCriteria(new tridion.TaxonomyKeywordCriteria(tk.TaxonomyUri, tk.KeywordUri, tk.IncludeKeywordBranch));
                        }
                        else
                        {
                            throw new NotSupportedException("Use either all the integer variables (PublicationId, TaxonomyId and KeywordId) or all the string variables (TaxonomyUri and KeywordUri) to create a correct KeywordCriteria");
                        }
                        break;
                    case "TaxonomyKeywordDescription":
                        ITaxonomyKeywordDescription tkd = (TaxonomyKeywordDescription)((ConstantExpression)b.Right).Value;
                        if(tkd.PublicationId != null && tkd.TaxonomyId != null)
                        {
                            currentCriteriaNode.AddCriteria(new tridion.TaxonomyKeywordDescriptionCriteria(tkd.PublicationId.Value, tkd.TaxonomyId.Value, tkd.KeywordDescription, tkd.IncludeKeywordBranch, tkd.Operator ?? tridion.Criteria.Equal));
                        }
                        else if(!string.IsNullOrEmpty(tkd.TaxonomyUri))
                        {
                            currentCriteriaNode.AddCriteria(new tridion.TaxonomyKeywordDescriptionCriteria(tkd.TaxonomyUri, tkd.KeywordDescription, tkd.IncludeKeywordBranch, tkd.Operator ?? tridion.Criteria.Equal));
                        }
                        else
                        {
                            throw new NotSupportedException("Use either all the integer variables (PublicationId, TaxonomyId) or all the string variable (TaxonomyUri) to create a correct KeywordCriteria");
                        }
                        break;
                    case "TaxonomyKeywordKey":
                        ITaxonomyKeywordKey tkk = (TaxonomyKeywordKey)((ConstantExpression)b.Right).Value;
                        if(tkk.PublicationId != null && tkk.TaxonomyId != null)
                        {
                            currentCriteriaNode.AddCriteria(new tridion.TaxonomyKeywordKeyCriteria(tkk.PublicationId.Value, tkk.TaxonomyId.Value, tkk.KeywordKey, tkk.IncludeKeywordBranch, tkk.Operator ?? tridion.Criteria.Equal));
                        }
                        else if(!string.IsNullOrEmpty(tkk.TaxonomyUri))
                        {
                            currentCriteriaNode.AddCriteria(new tridion.TaxonomyKeywordKeyCriteria(tkk.TaxonomyUri, tkk.KeywordKey, tkk.IncludeKeywordBranch, tkk.Operator ?? tridion.Criteria.Equal));
                        }
                        else
                        {
                            throw new NotSupportedException("Use either all the integer variables (PublicationId, TaxonomyId) or all the string variable (TaxonomyUri) to create a correct KeywordCriteria");
                        }
                        break;
                    case "TaxonomyKeywordName":
                        ITaxonomyKeywordName tkn = (TaxonomyKeywordName)((ConstantExpression)b.Right).Value;
                        if(tkn.PublicationId != null && tkn.TaxonomyId != null)
                        {
                            currentCriteriaNode.AddCriteria(new tridion.TaxonomyKeywordNameCriteria(tkn.PublicationId.Value, tkn.TaxonomyId.Value, tkn.KeywordName, tkn.IncludeKeywordBranch, tkn.Operator ?? tridion.Criteria.Equal));
                        }
                        else if(!string.IsNullOrEmpty(tkn.TaxonomyUri))
                        {
                            currentCriteriaNode.AddCriteria(new tridion.TaxonomyKeywordNameCriteria(tkn.TaxonomyUri, tkn.KeywordName, tkn.IncludeKeywordBranch, tkn.Operator ?? tridion.Criteria.Equal));
                        }
                        else
                        {
                            throw new NotSupportedException("Use either all the integer variables (PublicationId, TaxonomyId) or all the string variable (TaxonomyUri) to create a correct KeywordCriteria");
                        }
                        break;
                    default:
                        // It is a Custom Metadata Field
                        if (CheckMemberAttributes(((MemberExpression)b.Left).Member))
                        {
                            // Determine type of field
                            tridion.CustomMetaValueCriteria CustomMetadataCriteria = null;
                            switch (Type.GetTypeCode(((ConstantExpression)b.Right).Value.GetType()))
                            {
                                case TypeCode.DateTime:
                                    CustomMetadataCriteria = new tridion.CustomMetaValueCriteria(new tridion.CustomMetaKeyCriteria(((MemberExpression)b.Left).Member.Name), (DateTime)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType));
                                    break;
                                case TypeCode.Boolean:
                                case TypeCode.Byte:
                                case TypeCode.Decimal:
                                case TypeCode.Double:
                                case TypeCode.Int16:
                                case TypeCode.Int32:
                                case TypeCode.Int64:
                                case TypeCode.SByte:
                                case TypeCode.Single:
                                case TypeCode.UInt16:
                                case TypeCode.UInt32:
                                case TypeCode.UInt64:
                                    CustomMetadataCriteria = new tridion.CustomMetaValueCriteria(new tridion.CustomMetaKeyCriteria(((MemberExpression)b.Left).Member.Name), (float)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType));
                                    break;
                                case TypeCode.String:
                                    CustomMetadataCriteria = new tridion.CustomMetaValueCriteria(new tridion.CustomMetaKeyCriteria(((MemberExpression)b.Left).Member.Name), (string)((ConstantExpression)b.Right).Value, DetermineTridionFieldOperator(b.NodeType));
                                    break;
                                default:
                                    break;
                            }

                            if (CustomMetadataCriteria != null)
                            {
                                currentCriteriaNode.AddCriteria(CustomMetadataCriteria);
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (b.NodeType)
                {
                    case ExpressionType.AndAlso:
                        if (currentExpressionType != b.NodeType)
                        {
                            if (criteriaNodes == null)
                            {
                                criteriaNodes = new LinqTridionAnd();
                                currentCriteriaNode = criteriaNodes;
                            }
                            else
                            {
                                currentCriteriaNode = criteriaNodes.AddChildCriteria(new LinqTridionAnd());
                            }
                            currentLocalCriteriaNode = currentCriteriaNode;
                            currentLocalExpressionType = b.NodeType;
                            currentExpressionType = b.NodeType;
                        }
                        break;
                    case ExpressionType.OrElse:
                        if (currentExpressionType != b.NodeType)
                        {
                            if (criteriaNodes == null)
                            {
                                criteriaNodes = new LinqTridionOr();
                                currentCriteriaNode = criteriaNodes;
                            }
                            else
                            {
                                currentCriteriaNode = criteriaNodes.AddChildCriteria(new LinqTridionOr());
                            }
                            currentLocalCriteriaNode = currentCriteriaNode;
                            currentLocalExpressionType = b.NodeType;
                            currentExpressionType = b.NodeType;
                        }
                        break;
                    case ExpressionType.Equal:
                        break;
                    case ExpressionType.NotEqual:
                        break;
                    case ExpressionType.LessThan:
                        break;
                    case ExpressionType.LessThanOrEqual:
                        break;
                    case ExpressionType.GreaterThan:
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        break;
                    default:
                        throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
                }
                this.Visit(b.Left);
                if (currentLocalCriteriaNode != null)
                {
                    currentCriteriaNode = currentLocalCriteriaNode;
                    currentExpressionType = currentLocalExpressionType;
                }
                this.Visit(b.Right);
            }
            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        private tridion.FieldOperator DetermineTridionFieldOperator(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Equal:
                    return tridion.Criteria.Equal;
                case ExpressionType.NotEqual:
                    return tridion.Criteria.NotEqual;
                case ExpressionType.LessThan:
                    return tridion.Criteria.LessThan;
                case ExpressionType.LessThanOrEqual:
                    return tridion.Criteria.LessThanOrEqual;
                case ExpressionType.GreaterThan:
                    return tridion.Criteria.GreaterThan;
                case ExpressionType.GreaterThanOrEqual:
                    return tridion.Criteria.GreaterThanOrEqual;
                default:
                    return tridion.Criteria.Equal;
            }
        }

        /*
        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;
            if (q != null)
            {
                // assume constant nodes w/ IQueryables are table references
            }
            else if (c.Value == null)
            {
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        break;
                    case TypeCode.String:
                        break;
                    case TypeCode.Int32:

                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                    default:
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                return m;
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }
        */
    }
}

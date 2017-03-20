using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Zit.Utils
{
    public static class DynamicOrderBy
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(orderByProperty)) throw new ArgumentException("orderByProperty");

            string[] arr = orderByProperty.Split('-');

            if (arr.Length < 2) throw new ArgumentException("orderByProperty");

            arr[1] = arr[1].ToUpper();

            string command = null;
            if (arr[1] == "DESC") command = "OrderByDescending";
            if (arr[1] == "ASC") command = "OrderBy";

            if (command == null) throw new ArgumentException("orderByProperty");


            var type = typeof(TEntity);
            var property = type.GetProperty(arr[0]);

            if (property == null) throw new ArgumentException("orderByProperty");

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(
                typeof(Queryable), 
                command, 
                new Type[] { type, property.PropertyType },
                source.Expression, 
                Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IQueryable<TEntity> WhereByInList<TEntity>(this IQueryable<TEntity> source, string fieldName, List<int> listValues) where TEntity : class
        {
            if (fieldName == null) throw new ArgumentNullException("fieldName");
            //Not Filter
            if (listValues == null || listValues.Count < 1) return source;

            var typeEntity = typeof(TEntity);
            var property = typeEntity.GetProperty(fieldName);
            var typeValue = listValues.GetType();
            if (property == null) throw new ArgumentException("fieldName");
            //Create Right
            var rightParam = Expression.Parameter(typeEntity,"p");
            Expression right = rightParam;
            right = Expression.Property(right,property);
            
            //Create Left
            var parameterExpress = Expression.Constant(listValues, typeValue);
            Expression left = parameterExpress;

            string methodName = "Contains";
            var methodInfo = typeValue.GetMethod(methodName);

            if (methodInfo == null) throw new ArgumentException("methodInfo");

            var methodCall = Expression.Call(left, methodInfo,right);

            Expression<Func<TEntity, bool>> myExpression = Expression.Lambda<Func<TEntity, bool>>(methodCall, rightParam);

            return source.Where(myExpression);
        }
    }

}

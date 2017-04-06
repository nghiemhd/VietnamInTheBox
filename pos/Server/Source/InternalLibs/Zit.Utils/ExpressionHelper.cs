using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Zit.Utils
{
    public static class ExpressionHelper
    {
        public static string GetPropertyName<T,TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            MemberExpression member = null;
            if (expression.Body is UnaryExpression)
            {
                UnaryExpression express = (UnaryExpression)expression.Body;
                member = (MemberExpression)express.Operand;
            }
            if (expression.Body is MemberExpression)
            {
                member = expression.Body as MemberExpression;
            }

            if (member == null) throw new InvalidOperationException("Expression must be a member expression");
            string propertyName = member.Member.Name;
            return propertyName;
        }

        /// <summary>
        /// And 2 boolean expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressionOne"></param>
        /// <param name="expressionTwo"></param>
        /// <returns></returns>
        public static Expression<Func<T, Boolean>> And<T>(this Expression<Func<T, Boolean>> expressionOne, 
            Expression<Func<T, Boolean>> expressionTwo)  
        {            
            var invokedSecond = Expression.Invoke(expressionTwo, expressionOne.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, Boolean>>(Expression.And(expressionOne.Body, invokedSecond), expressionOne.Parameters);                         
        }

        /// <summary>
        /// Or 2 boolean expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressionOne"></param>
        /// <param name="expressionTwo"></param>
        /// <returns></returns>
        public static Expression<Func<T, Boolean>> Or<T>(this Expression<Func<T, Boolean>> expressionOne,
            Expression<Func<T, Boolean>> expressionTwo)
        {
            var invokedSecond = Expression.Invoke(expressionTwo, expressionOne.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, Boolean>>(Expression.Or(expressionOne.Body, invokedSecond), expressionOne.Parameters);
        }
    }
}

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DoctorDiet.Data.Extentions
{
    static class DbContextExtensions
    {
        public static void ApplyGlobalFilter<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> filterExpression)
        {
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                if (mutableEntityType.ClrType.IsAssignableTo(typeof(T)))
                {
                    var parameter = Expression.Parameter(mutableEntityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.First(), parameter, filterExpression.Body);
                    var lambdaExpression = Expression.Lambda(body, parameter);

                    mutableEntityType.SetQueryFilter(lambdaExpression);
                }
            }
        }
    }
    }

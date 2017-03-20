using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Zit.Core.Repository {

    public interface IRepository<TEntity> where TEntity : class 
    {
        /// <summary>
        /// Add the Entity Object to the Repository
        /// </summary>
        /// <param name="entity">The Entity object to add</param>
        void Add(TEntity entity);

        /// <summary>
        /// Update changes made to the Entity object in the repository
        /// </summary>
        /// <param name="entity">The Entity object to update</param>
        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        /// <summary>
        /// Delete the Entity object from the repository
        /// </summary>
        /// <param name="entity">The Entity object to delete</param>
        void Delete(TEntity entity);
        /// <summary>
        /// Get object by key
        /// </summary>
        /// <param name="id"></param>
        TEntity GetByID(object id);
    }
}

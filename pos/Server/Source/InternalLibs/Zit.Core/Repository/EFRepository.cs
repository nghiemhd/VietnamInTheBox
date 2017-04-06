using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;
using System.Data.Metadata.Edm;
using Zit.Utils;
using Zit.Entity;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;

namespace Zit.Core.Repository
{
    public abstract class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Vars

        private readonly ObjectContext _objContext;
        private readonly EFUnitOfWork _unitOfWork;

        private ObjectSet<TEntity> _objSet;
        private ObjectSet<TEntity> ObjSet
        {
            get
            {
                if (_objSet == null)
                {
                    _objSet = _objContext.CreateObjectSet<TEntity>();
                }
                return _objSet;
            }
        }

        #endregion

        #region Constructors

        public EFRepository(IUnitOfWork unitOfWork)
        {
            _objContext = ((IObjectContextAdapter)unitOfWork.Context).ObjectContext;
            _unitOfWork = unitOfWork as EFUnitOfWork;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the repository query.
        /// </summary>
        /// <value>The repository query.</value>
        protected virtual IQueryable<TEntity> RepositoryQuery
        {
            get
            {
                return this.ObjSet.AsQueryable<TEntity>();
            }
        }
        /// <summary>
        /// Gets the repository query.
        /// </summary>
        /// <value>The repository query.</value>
        protected virtual IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] navigateProperties)
        {
            ObjectQuery<TEntity> objquery = null;
            foreach (var property in navigateProperties)
            {
                string propertyName = property.GetPropertyName();
                if (objquery == null)
                    objquery = this.ObjSet.Include(propertyName);
                else
                    objquery = objquery.Include(propertyName);
            }
            return objquery;
        }

        protected IEnumerable<T> ExecQuery<T>(string query, params object[] paras)
        {
            return this._objContext.ExecuteStoreQuery<T>(query, paras);
        }

        protected IEnumerable<TEntity> ExecQuery(string query, params object[] paras)
        {
            return this._objContext.ExecuteStoreQuery<TEntity>(query, paras);
        }

        protected void ExecQueryCommand(string query, params object[] paras)
        {
            this._objContext.ExecuteStoreCommand(query, paras);
        }

        #endregion

        #region IRepository<TEntity> Members

        public virtual void Add(TEntity entity)
        {
            this.ObjSet.AddObject(entity);
        }

        public virtual void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            var entityKey = GetEntityKey(entity);

            //Try Get Object From Key
            object dbObj = null;
            if (_objContext.TryGetObjectByKey(entityKey, out dbObj))
            {
                __checkVersion(dbObj, entity);
            }
            else
            {
                throw new ObjectNotFoundException("Object not found");
            }

            //TODO : Object always in context, remove unuse code
            //Flag Update Called
            EntityBase entityBase = entity as EntityBase;
            if (entityBase != null)
            {
                entityBase.UpdateMethodCalled = true;
            }
            //Attach Obj If Entity is Detached
            ObjectStateEntry updateObjState;
            _objContext.ObjectStateManager.TryGetObjectStateEntry(entityKey, out updateObjState);
            bool isAttached = false;
            bool isUpdateMap = false;
            if (updateObjState == null || updateObjState.State == EntityState.Detached)
            {
                this.ObjSet.Attach(entity);
                //Update State
                _objContext.ObjectStateManager.TryGetObjectStateEntry(entity, out updateObjState);
                isAttached = true;
            }
            else
            {
                if (updateObjState.Entity != entity)
                {
                    //Check Version
                    __checkVersion(updateObjState.Entity, entity);
                    if ((updateObjState.Entity as EntityBase) != null) (updateObjState.Entity as EntityBase).UpdateMethodCalled = true;
                    isUpdateMap = true;
                }
            }
            //
            if (properties == null || !properties.Any())
            {
                //Update All Property
                if (isAttached)
                {
                    _objContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
                }
                else
                {
                    if (isUpdateMap)
                    {
                        __copyEntityProperties(entity, updateObjState.Entity);
                    }
                }
            }
            else
            {
                if (!isAttached)
                {
                    //Reset Changed
                    updateObjState.ChangeState(EntityState.Unchanged);
                    _objContext.ObjectStateManager.TryGetObjectStateEntry(entityKey, out updateObjState);

                    if (isUpdateMap)
                    {
                        __copyEntityProperties(entity, updateObjState.Entity, properties);
                    }
                }
                //Set Property
                foreach (var property in properties)
                {
                    string propertyName = property.GetPropertyName();
                    updateObjState.SetModifiedProperty(propertyName);
                }
            }
        }

        public virtual void Delete(TEntity entity)
        {
            TEntity _entity = entity;
            ObjectStateEntry deletedObj;
            _objContext.ObjectStateManager.TryGetObjectStateEntry(GetEntityKey(_entity), out deletedObj);
            if (deletedObj == null || deletedObj.State == EntityState.Detached)
            {
                this.ObjSet.Attach(_entity);
            }
            else
            {
                object obj;
                if (_objContext.TryGetObjectByKey(GetEntityKey(_entity), out obj))
                {
                    __checkVersion(obj, _entity);
                    _entity = obj as TEntity;
                }
                else
                    throw new ArgumentException("entity");
            }

            this.ObjSet.DeleteObject(_entity);
        }

        public virtual TEntity GetByID(object id)
        {
            ReadOnlyMetadataCollection<EdmMember> keyMembers =
                ObjSet.EntitySet.ElementType.KeyMembers;

            if (keyMembers.Count > 1) throw new NotSupportedException("Not Support Composite Key");

            EntityKeyMember keyMem = new EntityKeyMember(keyMembers[0].Name, id);

            var entityKey = new EntityKey(_objSet.EntitySet.EntityContainer
                                    + "." + ObjSet.EntitySet.Name, new[] { keyMem });
            object result = null;
            if (_objContext.TryGetObjectByKey(entityKey, out result))
            {
                return result as TEntity;
            }
            else
                return null;
        }

        #endregion

        #region Private Method

        public EntityKey GetEntityKey(TEntity entity)
        {
            return _objContext.CreateEntityKey(ObjSet.EntitySet.Name, entity);
        }

        private void __checkVersion(object entity, object toEntity)
        {            
            var property = entity.GetType().GetProperty(Consts.VersionCol);
            if (property == null) return;

            var value1 = property.GetValue(entity, null) as byte[];
            var value2 = property.GetValue(toEntity, null) as byte[];

            if (value1 == null || value2 == null)
            {
                throw new OptimisticConcurrencyException("Version NULL");
            }

            if (!value1.SequenceEqual(value2))
                throw new OptimisticConcurrencyException("Version");
        }

        private void __copyEntityProperties(object entity, object toEntity, params Expression<Func<TEntity, object>>[] properties)
        {
            string[] stringProperties = properties.Select(m => m.GetPropertyName()).ToArray();
            __copyEntityProperties(entity, toEntity, stringProperties);
        }

        private void __copyEntityProperties(object entity, object toEntity, string[] properties)
        {
            Type type = entity.GetType();
            foreach (string property in properties)
            {
                var entityProperty = type.GetProperty(property);
                var value = entityProperty.GetValue(entity, null);
                entityProperty.SetValue(toEntity, value, null);
            }
        }

        private void __copyEntityProperties(object entity, object toEntity)
        {
            string[] properties = this.ObjSet.EntitySet.ElementType.Properties.Select(m => m.Name).ToArray();
            __copyEntityProperties(entity, toEntity, properties);
        }

        #endregion

    }
}

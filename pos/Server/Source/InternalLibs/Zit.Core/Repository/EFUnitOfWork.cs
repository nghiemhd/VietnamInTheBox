using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Threading;
using Zit.Entity;
using System.Data.Metadata.Edm;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Zit.Core.Repository {

    public class EFUnitOfWork : IUnitOfWork
    {
        #region Vars

        readonly ObjectContext _context;
        readonly IObjectContext _dbContext;

        #endregion

        #region Constructors

        public EFUnitOfWork(IObjectContext objectContext)
        {
            _dbContext = objectContext;
            var objCtxAd = (objectContext as IObjectContextAdapter);
            _context = (objCtxAd == null) ? null : objCtxAd.ObjectContext;
        }

        #endregion

        #region IUnitOfWork Members

        public IObjectContext Context 
        {
            get { return _dbContext; }
        }

        public void Commit() 
        {
            //POCO Detect Changes
            _context.DetectChanges();

            _context.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added | System.Data.EntityState.Modified).ToList().ForEach(entityState =>
            {
                
                Type entityType = entityState.Entity.GetType();
                var optimisticLockFieldProperty = entityType.GetProperty(Consts.VersionCol);
                var updatedUserFieldProperty = entityType.GetProperty(Consts.UpdatedUser);
                var updatedDateFieldProperty = entityType.GetProperty(Consts.UpdatedDate);

                string userName = Thread.CurrentPrincipal.Identity.IsAuthenticated ? Thread.CurrentPrincipal.Identity.Name : "System";

                #region Add

                if (entityState.State == System.Data.EntityState.Added)
                {

                    var createdUserFieldProperty = entityType.GetProperty(Consts.CreatedUser);
                    var createdDateFieldProperty = entityType.GetProperty(Consts.CreatedDate);

                    if (createdUserFieldProperty != null)
                        createdUserFieldProperty.SetValue(entityState.Entity, userName, null);
                    if (createdDateFieldProperty != null)
                        createdDateFieldProperty.SetValue(entityState.Entity, DateTime.Now, null);
                }

                #endregion

                #region Modified

                if (entityState.State == System.Data.EntityState.Modified)
                {
                    //Check Update Method Called
                    EntityBase entityBase = entityState.Entity as EntityBase;

                    if (entityBase != null)
                    {
                        if (!entityBase.UpdateMethodCalled) throw new Exception("Please Call Repository Update To Update Entity");
                        entityBase.UpdateMethodCalled = false;
                    }

                    //Set Modify When POCO Object Is Detached
                    if (updatedUserFieldProperty != null)
                        entityState.SetModifiedProperty(updatedUserFieldProperty.Name);

                    if (updatedDateFieldProperty != null)
                        entityState.SetModifiedProperty(updatedDateFieldProperty.Name);
                }

                #endregion

                if (updatedUserFieldProperty != null)
                    updatedUserFieldProperty.SetValue(entityState.Entity, userName, null);
                if (updatedDateFieldProperty != null)
                    updatedDateFieldProperty.SetValue(entityState.Entity, DateTime.Now, null);
            });
            _context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
        }

        #endregion

        #region IDisposable Members

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {            
            if (!this.disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose() {
            Dispose(true);
        }

        #endregion

    }
}

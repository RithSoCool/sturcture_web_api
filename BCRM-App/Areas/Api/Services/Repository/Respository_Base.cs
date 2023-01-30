using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Areas.Api.Services;

namespace BCRM_App.Services.RemoteInternal.Repository
{
    public interface IRespository<T> where T : class
    {
        IQueryable<T> Query(Func<T, bool> condition);
        IQueryable<T> All { get; }
        IQueryable<T> Select();

        T Find(params object[] keys);

        T Add(T item);
        T Update(T item);
        T Delete(T item);
    }

    //public class Respository_Base<T> : Service_Base, IDisposable ,IRespository<T> where T : class
    public class Respository_Base<T> : Service_Base, IRespository<T> where T : class
    {
        private readonly DbContext db;

        public Respository_Base(DbContext dbContext)
        {
            this.db = dbContext;
        }

        public DbContext DB => db;

        public virtual IQueryable<T> Select() => db.Set<T>().Select(it => it).AsQueryable();
        public IQueryable<T> All => Query(x => true);
        public virtual IQueryable<T> Query(Func<T, bool> condition) => db.Set<T>().Where(condition).AsQueryable();

        public virtual T Find(params object[] keys) => db.Set<T>().Find(keys);

        public virtual T Add(T item)
        {
            var res = db.Set<T>().Add(item).Entity;
            db.SaveChanges();
            return res;
        }

        public virtual T Delete(T item)
        {
            var res = db.Set<T>().Remove(item).Entity;
            db.SaveChanges();
            return res;
        }
        public virtual T Update(T item)
        {
            var res = db.Set<T>().Update(item).Entity;
            db.SaveChanges();
            return res;
        }

        bool disposed = false;


        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                DB.Dispose();
            }

            disposed = true;
        }
    }

}

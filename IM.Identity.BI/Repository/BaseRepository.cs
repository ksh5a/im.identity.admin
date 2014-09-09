using System;
using System.Data.Entity;

namespace IM.Identity.BI.Repository
{
    public class BaseRepository : IDisposable
    {
        protected readonly DbContext Context;
        private bool _disposed;

        protected BaseRepository(DbContext context)
        {
            Context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

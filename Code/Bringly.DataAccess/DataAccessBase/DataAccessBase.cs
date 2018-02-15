using System;

namespace Bringly.DataAccess.BaseClasses
{
    public abstract class DataAccessBase : IDisposable
    {
        protected Bringly.Data.BringlyEntities _bringlyDbContext;



        /// <summary>
        /// Odin Instance
        /// </summary>
        public DataAccessBase()
        {
            _bringlyDbContext = new Data.BringlyEntities();
        }
        //protected internal DataLayer.BringlyContext _DbContext;
        //public DataAccessBase()
        //{
        //    _DbContext = new DataLayer.BringlyContext();
        //}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    //if (_DbContext != null)
                    //{
                    //    _DbContext.Dispose();
                    //    _DbContext = null;
                    //}
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DataAccessBase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_bringlyDbContext != null)
            {
                _bringlyDbContext.Dispose();
            }
            _bringlyDbContext = null;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        ~DataAccessBase()
        {
            Dispose();
        }
    }
}

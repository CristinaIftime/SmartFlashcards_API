using LibrarieModele;
using System;


namespace NivelAccessDate
{
    public abstract class DataAccessorBase : IDisposable
    {
        protected readonly AppDbContext _ctx;

        protected DataAccessorBase()
        {
            _ctx = new AppDbContext();
            _ctx.Configuration.LazyLoadingEnabled = true;
            _ctx.Configuration.ProxyCreationEnabled = true;
        }

        public void Dispose() => _ctx?.Dispose();
    }

}
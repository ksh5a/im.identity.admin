using System;
using System.Data.Entity;
using IM.Identity.EDM.Entities;

namespace IM.Identity.EDM.Repository
{
    public class UnitOfWork : IDisposable
    {        
        private bool _disposed;
        private readonly IdentityDbContext _context = new IdentityDbContext();
        private AspNetUserRepository _userRepository;
        private AspNetRoleRepository _roleRepository;

        public Database Database { get { return _context.Database; } }

        public AspNetUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new AspNetUserRepository(_context)); }
        }

        public AspNetRoleRepository RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new AspNetRoleRepository(_context)); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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

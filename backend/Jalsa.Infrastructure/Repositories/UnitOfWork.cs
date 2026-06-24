using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Jalsa.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly Galsa_DBDbContext _context;
    private readonly Dictionary<Type,object>_repositories=new();
    private IDbContextTransaction?_transaction;
    private bool _disposed;

    public UnitOfWork(Galsa_DBDbContext context)=>_context=context;
    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if(!_repositories.ContainsKey(type))
        _repositories[type]=new GenericRepository<T>(_context);
        return (IGenericRepository<T>)_repositories[type];
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            await _transaction.RollbackAsync(cancellationToken);
    }
    public void Dispose()
    {
        if (!_disposed)
        {
            _transaction?.Dispose();
            _context.Dispose();
            _disposed = true;
        }
    }
}
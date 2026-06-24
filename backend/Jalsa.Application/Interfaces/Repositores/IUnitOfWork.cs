namespace Jalsa.Application.Interfaces.Repositores;
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T>Repository<T>() where T:class;
    Task<int>SaveChangesAsync(CancellationToken cancellationToken=default);
    Task BeginTransactionAsync (CancellationToken cancellationToken=default);
    Task CommitAsync(CancellationToken cancellationToken=default);
    Task RollbackAsync(CancellationToken cancellationToken=default);
}
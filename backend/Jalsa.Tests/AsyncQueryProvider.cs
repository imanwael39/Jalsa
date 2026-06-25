using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Jalsa.Tests;

internal class AsyncQueryProvider<T> : IAsyncQueryProvider, IQueryable<T>, IAsyncEnumerable<T>, IOrderedQueryable<T>
{
    private readonly IQueryable<T> _inner;

    public AsyncQueryProvider(IQueryable<T> inner)
    {
        _inner = inner;
    }

    public Type ElementType => _inner.ElementType;
    public Expression Expression => _inner.Expression;
    public IQueryProvider Provider => this;

    public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _inner.GetEnumerator();

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new AsyncEnumerator<T>(_inner.GetEnumerator());

    public IQueryable CreateQuery(Expression expression)
        => new AsyncQueryProvider<T>(_inner.Provider.CreateQuery<T>(expression));

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new AsyncQueryProvider<TElement>(_inner.Provider.CreateQuery<TElement>(expression));

    public object? Execute(Expression expression) => _inner.Provider.Execute(expression);

    public TResult Execute<TResult>(Expression expression) => _inner.Provider.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        => _inner.Provider.Execute<TResult>(expression);
}

internal class AsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public AsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

    public T Current => _inner.Current;

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync()
        => ValueTask.FromResult(_inner.MoveNext());
}

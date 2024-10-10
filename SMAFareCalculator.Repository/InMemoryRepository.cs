using System.Linq.Expressions;
using SMAFareCalculator.Common;
using SMAFareCalculator.Domain;
using SMAFareCalculator.Repository.Interface;

namespace SMAFareCalculator.Repository;

internal class InMemoryRepository : IRepository
{
    private readonly List<object> _fakeDb = new();

    public InMemoryRepository()
    {
        this.InitDb().Wait();
    }

    public Task<RepoWriteResponse> BulkInsert<T>(IEnumerable<T> objectsToInsert) where T : BaseDomain
    {
        _fakeDb.AddRange(objectsToInsert);
        return Task.FromResult(this.SuccessWriteResponse(objectsToInsert.Count()));
    }

    public Task<IEnumerable<T>> Search<T>(Expression<Func<T, bool>> searchFilter) where T : BaseDomain
    {
        var a = _fakeDb.Where(o => o.GetType() == typeof(T));
        return Task.FromResult(
            a
                .Select(d => (T)d)
                .Where(domain => searchFilter.Compile().Invoke(domain)));
    }

    public Task<RepoWriteResponse> Update<T>(T objectToUpdate) where T : BaseDomain
    {
        var existingEntity = _fakeDb.Select(o => o.ToType<T>()).FirstOrDefault(domain => domain is not null && domain.Id == objectToUpdate.Id);
        if (existingEntity is null)
            return Task.FromResult(this.FailedToFindExistingEntity());
        
        _fakeDb.Remove(existingEntity);
        _fakeDb.Add(objectToUpdate);
        return Task.FromResult(this.SuccessWriteResponse(1));
    }
}
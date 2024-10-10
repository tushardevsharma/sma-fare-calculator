using System.Linq.Expressions;
using SMAFareCalculator.Domain;

namespace SMAFareCalculator.Repository.Interface;

public interface IRepository
{
    Task<RepoWriteResponse> BulkInsert<T>(IEnumerable<T> objectsToInsert) where T : BaseDomain;
    Task<IEnumerable<T>> Search<T>(Expression<Func<T, bool>> searchFilter) where T : BaseDomain;
    Task<RepoWriteResponse> Update<T>(T objectToUpdate) where T : BaseDomain;
}
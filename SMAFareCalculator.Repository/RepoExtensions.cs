using SMAFareCalculator.Domain;
using SMAFareCalculator.Repository.Interface;

namespace SMAFareCalculator.Repository;

internal static class RepoWriteExtensions
{
    public static RepoWriteResponse FailedToFindExistingEntity(this IRepository repo)
        => new(false, 0, DateTime.Now);

    public static RepoWriteResponse SuccessWriteResponse(this IRepository repo, int affectedRecords)
        => new(true, affectedRecords, DateTime.Now);
}

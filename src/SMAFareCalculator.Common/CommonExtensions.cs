using System.Collections;

namespace SMAFareCalculator.Common;

public static class CommonExtensions
{
    public static TDestType? ToType<TDestType>(this object source)
        => (TDestType?)Convert.ChangeType(source, typeof(TDestType));

    public static bool HasRecords<T>(this IEnumerable<T>? sourceEnumerable)
        => sourceEnumerable is not null && sourceEnumerable.Any();
}
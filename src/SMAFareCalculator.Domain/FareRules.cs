namespace SMAFareCalculator.Domain;

public record Line(int Id, string Name) : BaseDomain(Id)
{
    public virtual bool Equals(Line? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && string.Equals(Name.Trim(), other.Name.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(base.GetHashCode());
        hashCode.Add(Name, StringComparer.OrdinalIgnoreCase);
        return hashCode.ToHashCode();
    }
}
public record FareRule(int Id, int Ref_FromLine, int Ref_ToLine, decimal PeakFare, decimal NonPeakFare, decimal DailyCap, decimal WeeklyCap) : BaseDomain(Id);
public record PeakHour(int Id, DayOfWeek DayOfWeek, TimeOnly FromTime, TimeOnly ToTime) : BaseDomain(Id);
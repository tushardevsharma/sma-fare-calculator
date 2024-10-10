namespace SMAFareCalculator.Domain;

public record BaseDomain(int Id);
public record RepoWriteResponse(bool IsSuccessful, long AffectedRecords, DateTime TimeOfTransaction);
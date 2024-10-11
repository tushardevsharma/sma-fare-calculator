namespace SMAFareCalculator.Domain;

// We will need this domain model when we introduce persistence into the system
// and need to calculate fare considering the history of rides for a customer
public record Ride(int Id, int Ref_RiderId, int FromLine, int ToLine, DateTime EntryTime, DateTime ExitTime) : BaseDomain(Id);
namespace EmployeePerformanceApp.Models;

public sealed class EmployeeEvaluation
{
    public required string FullName { get; init; }
    public required string Department { get; init; }
    public decimal Kpi { get; init; }
    public decimal Quality { get; init; }
    public decimal Discipline { get; init; }
    public decimal Initiative { get; init; }
    public decimal TotalScore { get; init; }
    public required string PerformanceLevel { get; init; }
}

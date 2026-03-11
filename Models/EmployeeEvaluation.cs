namespace EmployeePerformanceApp.Models;

public sealed class EmployeeEvaluation
{
    public required string FullName { get; init; }
    public int CompletedTasks { get; init; }
    public int NotCompletedTasks { get; init; }
    public int TotalTasks => CompletedTasks + NotCompletedTasks;
    public decimal Kpi { get; init; }
    public required string Analysis { get; init; }
}

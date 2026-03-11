namespace EmployeePerformanceApp.Services;

public static class PerformanceCalculator
{
    // KPI = (выполненные задачи / все задачи) * 100
    public static decimal CalculateKpi(int completedTasks, int notCompletedTasks)
    {
        var total = completedTasks + notCompletedTasks;
        if (total <= 0)
        {
            return 0m;
        }

        return Math.Round((decimal)completedTasks / total * 100m, 2);
    }

    public static string GetAnalysis(decimal kpi) => kpi switch
    {
        >= 90m => "Отличный результат",
        >= 75m => "Хороший результат",
        >= 60m => "Средний результат",
        _ => "Низкий результат, требуется внимание"
    };
}
